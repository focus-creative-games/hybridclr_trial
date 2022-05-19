using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace HuaTuo.Generators
{
    public enum CallConventionType
    {
        X64,
        Arm32,
        Arm64,
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

        private readonly HashSet<MethodBridgeSig> _methodSet = new HashSet<MethodBridgeSig>();

        private List<MethodBridgeSig> _methodList;

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
                CallConventionType.Arm32 => new PlatformAdaptor_Arm32(),
                CallConventionType.Arm64 => new PlatformAdaptor_Arm64(),
                CallConventionType.X64 => new PlatformAdaptor_X64(),
                _ => throw new NotSupportedException(),
            };
        }

        private string GetTemplateFile()
        {
            string tplFile = _callConvention switch
            {
                CallConventionType.X64 => "x64",
                CallConventionType.Arm32 => "arm32",
                CallConventionType.Arm64 => "arm64",
                _ => throw new NotSupportedException(),
            };
            return $"{Application.dataPath}/Editor/Huatuo/Templates/MethodBridge_{tplFile}.cpp";
        }

        public IEnumerable<TypeGenInfo> GetGenerateTypes()
        {
            return new List<TypeGenInfo>();
        }

        public List<MethodBridgeSig> GetGenerateMethods()
        {
            return _methodList;
        }

        private MethodBridgeSig CreateMethodBridgeSig(MethodInfo method)
        {
            var paramInfos = new List<ParamInfo>();
            if (!method.IsStatic)
            {
                // FIXME arm32 is s_i4u4
                paramInfos.Add(new ParamInfo() { Type = TypeInfo.s_i8u8 });
            }
            foreach(var paramInfo in method.GetParameters())
            {
                paramInfos.Add(new ParamInfo() { Type = _platformAdaptor.Create(paramInfo, false) });
            }
            var mbs = new MethodBridgeSig()
            {
                Method = method,
                ReturnInfo = new ReturnInfo() { Type = _platformAdaptor.Create(method.ReturnParameter, true)},
                ParamInfos = paramInfos,
            };
            return mbs;
        }

        private void AddMethod(MethodBridgeSig method)
        {
            if (_methodSet.Add(method))
            {
                method.Init();
            }
        }

        public void PrepareFromAssemblies()
        {
            foreach (var ass in _assemblies)
            {
                //Debug.Log("prepare assembly:" + ass.FullName);
                foreach (var type in ass.GetTypes())
                {
                    if (type.IsGenericTypeDefinition)
                    {
                        continue;
                    }
                    foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        if (!method.IsGenericMethodDefinition)
                        {
                            var mbs = CreateMethodBridgeSig(method);
                            AddMethod(mbs);
                        }
                    }
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
                        var mbs = new MethodBridgeSig() { Method = null, ReturnInfo = rt, ParamInfos =  paramInfos};
                        AddMethod(mbs);
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
            TypeInfo typeStructRef = new TypeInfo(null, ParamOrReturnType.STRUCTURE_AS_REF_PARAM);

            int maxParamCount = 4;

            var argTypes = new TypeInfo[] { typeLong, typeDouble, typeStructRef };
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
                        var mbs = new MethodBridgeSig() { Method = null, ReturnInfo = rt, ParamInfos = paramInfos };
                        AddMethod(mbs);
                    }
                }
            }
        }

        public List<Type> PrepareCustomGenericTypes()
        {
            return new List<Type>
            {

            };
        }

        public void PrepareMethods()
        {
            PrepareCommon1();
            PrepareCommon2();
            foreach(var method in _platformAdaptor.GetPreserveMethods())
            {
                AddMethod(method);
            }
            PrepareFromAssemblies();

            var methodsByName = _methodSet.ToDictionary(e => e.CreateCallSigName(), e => e);
            _methodList = methodsByName.Values.ToList();
        }

        public void Generate()
        {
            var frr = new FileRegionReplace(GetTemplateFile());

            List<string> lines = new List<string>(20_0000);

            Debug.LogFormat("== method count:{0}", GetGenerateMethods().Count);

            _platformAdaptor.Generate(GetGenerateMethods(), lines);

            frr.Replace("INVOKE_STUB", string.Join("\n", lines));

            Directory.CreateDirectory(Path.GetDirectoryName(_outputFile));

            frr.Commit(_outputFile);
        }

    }
}
