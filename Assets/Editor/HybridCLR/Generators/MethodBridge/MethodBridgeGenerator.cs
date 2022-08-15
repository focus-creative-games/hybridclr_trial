using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace HybridCLR.Generators.MethodBridge
{

    public class TypeGenInfo
    {
        public Type Type { get; set; }

        public List<MethodInfo> GenericMethods { get; set; }
    }

    public class MethodBridgeGeneratorOptions
    {
        public List<Assembly> Assemblies { get; set; }

        public PlatformABI CallConvention { get; set; }

        public string OutputFile { get; set; }
    }

    public class MethodBridgeGenerator
    {
        private readonly List<Assembly> _assemblies;

        private readonly PlatformABI _callConvention;

        private readonly string _outputFile;

        private readonly IPlatformAdaptor _platformAdaptor;

        private readonly HashSet<MethodBridgeSig> _callMethodSet = new HashSet<MethodBridgeSig>();

        private List<MethodBridgeSig> _callMethodList;

        private readonly HashSet<MethodBridgeSig> _adjustThunkMethodSet = new HashSet<MethodBridgeSig>();

        private List<MethodBridgeSig> _adjustThunkMethodList;

        public MethodBridgeGenerator(MethodBridgeGeneratorOptions options)
        {
            _assemblies = options.Assemblies;
            _callConvention = options.CallConvention;
            _outputFile = options.OutputFile;
            _platformAdaptor = CreatePlatformAdaptor(options.CallConvention);
        }

        private static IPlatformAdaptor CreatePlatformAdaptor(PlatformABI type)
        {
            return type switch
            {
                PlatformABI.Universal32 => new PlatformAdaptor_Universal32(),
                PlatformABI.Universal64 => new PlatformAdaptor_Universal64(),
                PlatformABI.Arm64 => new PlatformAdaptor_Arm64(),
                _ => throw new NotSupportedException(),
            };
        }

        private string GetTemplateFile()
        {
            string tplFile = _callConvention switch
            {
                PlatformABI.Universal32 => "Universal32",
                PlatformABI.Universal64 => "Universal64",
                PlatformABI.Arm64 => "Arm64",
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

        private void AddAdjustThunkMethod(MethodBridgeSig method)
        {
            if (_adjustThunkMethodSet.Add(method))
            {
                method.Init();
            }
        }

        private void ScanType(Type type)
        {
            if (type.IsGenericTypeDefinition)
            {
                return;
            }
            var typeDel = typeof(MulticastDelegate);
            if (typeDel.IsAssignableFrom(type))
            {
                var method = type.GetMethod("Invoke");
                if (method == null)
                {
                    //Debug.LogError($"delegate:{typeDel.FullName} Invoke not exists");
                    return;
                }
                var instanceCallMethod = CreateMethodBridgeSig(false, method.ReturnParameter, method.GetParameters());
                AddCallMethod(instanceCallMethod);
                var staticCallMethod = CreateMethodBridgeSig(true, method.ReturnParameter, method.GetParameters());
                AddCallMethod(staticCallMethod);
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

                if (type.IsValueType && !method.IsStatic)
                {
                    var adjustThunkMethod = CreateMethodBridgeSig(true, method.ReturnParameter, method.GetParameters());
                    AddAdjustThunkMethod(adjustThunkMethod);
                }
            }

            foreach (var method in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public
| BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.InvokeMethod | BindingFlags.FlattenHierarchy))
            {
                var callMethod = CreateMethodBridgeSig(false, null, method.GetParameters());
                AddCallMethod(callMethod);

                if (type.IsValueType && !method.IsStatic)
                {
                    var invokeMethod = CreateMethodBridgeSig(true, null, method.GetParameters());
                    AddAdjustThunkMethod(invokeMethod);
                }
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

        private void PrepareMethodsFromCustomeGenericTypes()
        {
            foreach (var type in GeneratorConfig.PrepareCustomGenericTypes())
            {
                ScanType(type);
            }
        }

        public void PrepareMethods()
        {
            PrepareMethodsFromCustomeGenericTypes();


            foreach(var methodSig in _platformAdaptor.IsArch32 ? GeneratorConfig.PrepareCustomMethodSignatures32() : GeneratorConfig.PrepareCustomMethodSignatures64())
            {
                var method = MethodBridgeSig.CreateBySignatuer(methodSig);
                AddCallMethod(method);
                AddAdjustThunkMethod(method);
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
                foreach (var method in _adjustThunkMethodSet)
                {
                    sortedMethods.Add(method.CreateCallSigName(), method);
                }
                _adjustThunkMethodList = sortedMethods.Values.ToList();
            }
        }

        public void Generate()
        {
            var frr = new FileRegionReplace(GetTemplateFile());

            List<string> lines = new List<string>(20_0000);

            Debug.LogFormat("== call method count:{0}", _callMethodList.Count);

            foreach(var method in _callMethodList)
            {
                _platformAdaptor.GenerateNormalMethod(method, lines);
            }

            _platformAdaptor.GenerateNormalStub(_callMethodList, lines);

            Debug.LogFormat("== adjustThunk method count:{0}", _adjustThunkMethodList.Count);

            foreach (var method in _adjustThunkMethodList)
            {
                _platformAdaptor.GenerateAdjustThunkMethod(method, lines);
            }

            _platformAdaptor.GenerateAdjustThunkStub(_adjustThunkMethodList, lines);

            frr.Replace("INVOKE_STUB", string.Join("\n", lines));

            Directory.CreateDirectory(Path.GetDirectoryName(_outputFile));

            frr.Commit(_outputFile);
        }

    }
}
