using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace HybridCLR.Generators
{
    public enum CallConventionType
    {
        General32,
        General64,
    }

    public class TypeGenInfo
    {
        public Type Type { get; set; }

        public List<MethodInfo> GenericMethods { get; set; }
    }

    public class MethodBridgeGeneratorOptions
    {
        public List<Assembly> Assemblies { get; set; }

        public CallConventionType CallConvention { get; set; }

        public string OutputFile { get; set; }
    }

    public class MethodBridgeGenerator
    {
        private readonly List<Assembly> _assemblies;

        private readonly CallConventionType _callConvention;

        private readonly string _outputFile;

        private readonly IPlatformAdaptor _platformAdaptor;

        private readonly HashSet<MethodBridgeSig> _callMethodSet = new HashSet<MethodBridgeSig>();

        private List<MethodBridgeSig> _callMethodList;

        private readonly HashSet<MethodBridgeSig> _invokeMethodSet = new HashSet<MethodBridgeSig>();

        private List<MethodBridgeSig> _invokeMethodList;

        public MethodBridgeGenerator(MethodBridgeGeneratorOptions options)
        {
            _assemblies = options.Assemblies;
            _callConvention = options.CallConvention;
            _outputFile = options.OutputFile;
            _platformAdaptor = CreatePlatformAdaptor(options.CallConvention);
        }

        private static IPlatformAdaptor CreatePlatformAdaptor(CallConventionType type)
        {
            return type switch
            {
                CallConventionType.General32 => new PlatformAdaptor_General32(),
                CallConventionType.General64 => new PlatformAdaptor_General64(),
                _ => throw new NotSupportedException(),
            };
        }

        private string GetTemplateFile()
        {
            string tplFile = _callConvention switch
            {
                CallConventionType.General32 => "general32",
                CallConventionType.General64 => "general64",
                _ => throw new NotSupportedException(),
            };
            return $"{Application.dataPath}/Editor/HybridCLR/Generators/Templates/MethodBridge_{tplFile}.cpp";
        }

        public IEnumerable<TypeGenInfo> GetGenerateTypes()
        {
            return new List<TypeGenInfo>();
        }

        private MethodBridgeSig CreateMethodBridgeSig(bool isStatic, ParameterInfo returnType, ParameterInfo[] parameters)
        {
            var paramInfos = new List<ParamInfo>();
            if (!isStatic)
            {
                // FIXME arm32 is s_i4u4
                paramInfos.Add(new ParamInfo() { Type = _platformAdaptor.IsArch32 ? TypeInfo.s_i4u4 : TypeInfo.s_i8u8 });
            }
            foreach (var paramInfo in parameters)
            {
                paramInfos.Add(new ParamInfo() { Type = _platformAdaptor.CreateTypeInfo(paramInfo.ParameterType, false) });
            }
            var mbs = new MethodBridgeSig()
            {
                ReturnInfo = new ReturnInfo() { Type = returnType != null ? _platformAdaptor.CreateTypeInfo(returnType.ParameterType, true) : TypeInfo.s_void },
                ParamInfos = paramInfos,
            };
            return mbs;
        }

        private void AddCallMethod(MethodBridgeSig method)
        {
            if (_callMethodSet.Add(method))
            {
                method.Init();
            }
        }

        private void AddInvokeMethod(MethodBridgeSig method)
        {
            if (_invokeMethodSet.Add(method))
            {
                method.Init();
            }
        }

        private void ScanType(Type type)
        {
            var typeDel = typeof(Delegate);
            if (type.IsGenericTypeDefinition)
            {
                return;
            }
            foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public
| BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.InvokeMethod | BindingFlags.FlattenHierarchy))
            {
                if (method.IsGenericMethodDefinition)
                {
                    continue;
                }
                var callMethod = CreateMethodBridgeSig(method.IsStatic, method.ReturnParameter, method.GetParameters());
                AddCallMethod(callMethod);

                var invokeMethod = CreateMethodBridgeSig(true, method.ReturnParameter, method.GetParameters());
                AddInvokeMethod(invokeMethod);
            }

            foreach (var method in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public
| BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.InvokeMethod | BindingFlags.FlattenHierarchy))
            {
                var callMethod = CreateMethodBridgeSig(false, null, method.GetParameters());
                AddCallMethod(callMethod);

                var invokeMethod = CreateMethodBridgeSig(true, null, method.GetParameters());
                AddInvokeMethod(invokeMethod);
            }

            foreach (var subType in type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic))
            {
                ScanType(subType);
            }
        }

        public void PrepareFromAssemblies()
        {
            foreach (var ass in _assemblies)
            {
                //Debug.Log("prepare assembly:" + ass.FullName);
                foreach (var type in ass.GetTypes())
                {
                    ScanType(type);
                }
            }
        }

        public void PrepareCommon1()
        {
            // (void + int64 + float) * (int64 + float) * (0 - 20) = 120
            TypeInfo typeVoid = new TypeInfo(typeof(void), ParamOrReturnType.VOID);
            TypeInfo typeLong = new TypeInfo(typeof(long), ParamOrReturnType.I8_U8);
            TypeInfo typeDouble = new TypeInfo(typeof(double), ParamOrReturnType.R8);
            int maxParamCount = 20;
                
            foreach (var returnType in new TypeInfo[] { typeVoid, typeLong, typeDouble })
            {
                var rt = new ReturnInfo() { Type = returnType };
                foreach (var argType in new TypeInfo[] { typeLong, typeDouble })
                {
                    for (int paramCount = 0; paramCount <= maxParamCount; paramCount++)
                    {
                        var paramInfos = new List<ParamInfo>();
                        for (int i = 0; i < paramCount; i++)
                        {
                            paramInfos.Add(new ParamInfo() { Type = argType });
                        }
                        var mbs = new MethodBridgeSig() { ReturnInfo = rt, ParamInfos =  paramInfos};
                        AddCallMethod(mbs);
                    }
                }
            }
        }

        public void PrepareCommon2()
        {
            // (void + int64 + float) * (int64 + float + sr) ^ (0 - 4) = 363
            TypeInfo typeVoid = new TypeInfo(typeof(void), ParamOrReturnType.VOID);
            TypeInfo typeLong = new TypeInfo(typeof(long), ParamOrReturnType.I8_U8);
            TypeInfo typeDouble = new TypeInfo(typeof(double), ParamOrReturnType.R8);

            int maxParamCount = 4;

            var argTypes = new TypeInfo[] { typeLong, typeDouble };
            int paramTypeNum = argTypes.Length;
            foreach (var returnType in new TypeInfo[] { typeVoid, typeLong, typeDouble })
            {
                var rt = new ReturnInfo() { Type = returnType };
                for(int paramCount = 1; paramCount <= maxParamCount; paramCount++)
                {
                    int totalCombinationNum = (int)Math.Pow(paramTypeNum, paramCount);

                    for (int k = 0; k < totalCombinationNum; k++)
                    {
                        var paramInfos = new List<ParamInfo>();
                        int c = k;
                        for(int i = 0; i < paramCount; i++)
                        {
                            paramInfos.Add(new ParamInfo { Type = argTypes[c % paramTypeNum] });
                            c /= paramTypeNum;
                        }
                        var mbs = new MethodBridgeSig() { ReturnInfo = rt, ParamInfos = paramInfos };
                        AddCallMethod(mbs);
                    }
                }
            }
        }

        private void PrepareMethodsFromCustomeGenericTypes()
        {
            foreach (var type in GeneratorConfig.PrepareCustomGenericTypes())
            {
                ScanType(type);
            }
        }

        public void PrepareMethods()
        {
            PrepareCommon1();
            PrepareCommon2();
            PrepareMethodsFromCustomeGenericTypes();


            foreach(var methodSig in _platformAdaptor.IsArch32 ? GeneratorConfig.PrepareCustomMethodSignatures32() : GeneratorConfig.PrepareCustomMethodSignatures64())
            {
                var method = MethodBridgeSig.CreateBySignatuer(methodSig);
                AddCallMethod(method);
                AddInvokeMethod(method);
            }
            foreach(var method in _platformAdaptor.GetPreserveMethods())
            {
                AddCallMethod(method);
                AddInvokeMethod(method);
            }
            PrepareFromAssemblies();

            {
                var sortedMethods = new SortedDictionary<string, MethodBridgeSig>();
                foreach (var method in _callMethodSet)
                {
                    sortedMethods.Add(method.CreateCallSigName(), method);
                }
                _callMethodList = sortedMethods.Values.ToList();
            }
            {
                var sortedMethods = new SortedDictionary<string, MethodBridgeSig>();
                foreach (var method in _invokeMethodSet)
                {
                    sortedMethods.Add(method.CreateCallSigName(), method);
                }
                _invokeMethodList = sortedMethods.Values.ToList();
            }
        }

        public void Generate()
        {
            var frr = new FileRegionReplace(GetTemplateFile());

            List<string> lines = new List<string>(20_0000);

            Debug.LogFormat("== call method count:{0}", _callMethodList.Count);

            foreach(var method in _callMethodList)
            {
                _platformAdaptor.GenerateCall(method, lines);
            }

            Debug.LogFormat("== invoke method count:{0}", _invokeMethodList.Count);
            foreach (var method in _invokeMethodList)
            {
                _platformAdaptor.GenerateInvoke(method, lines);
            }

            _platformAdaptor.GenCallStub(_callMethodList, lines);
            _platformAdaptor.GenInvokeStub(_invokeMethodList, lines);

            frr.Replace("INVOKE_STUB", string.Join("\n", lines));

            Directory.CreateDirectory(Path.GetDirectoryName(_outputFile));

            frr.Commit(_outputFile);
        }

    }
}
