using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HybridCLR.Generators
{
    internal class PlatformAdaptor_General32 : PlatformAdaptorBase
    {

        private static readonly Dictionary<Type, TypeInfo> s_typeInfoCaches = new Dictionary<Type, TypeInfo>()
        {
            { typeof(void), new TypeInfo(typeof(void), ParamOrReturnType.VOID)},
            { typeof(bool), new TypeInfo(typeof(bool), ParamOrReturnType.I1_U1)},
            { typeof(byte), new TypeInfo(typeof(byte), ParamOrReturnType.I1_U1)},
            { typeof(sbyte), new TypeInfo(typeof(sbyte), ParamOrReturnType.I1_U1) },
            { typeof(short), new TypeInfo(typeof(short), ParamOrReturnType.I2_U2) },
            { typeof(ushort), new TypeInfo(typeof(ushort), ParamOrReturnType.I2_U2) },
            { typeof(char), new TypeInfo(typeof(char), ParamOrReturnType.I2_U2) },
            { typeof(int), new TypeInfo(typeof(int), ParamOrReturnType.I4_U4) },
            { typeof(uint), new TypeInfo(typeof(uint), ParamOrReturnType.I4_U4) },
            { typeof(long), new TypeInfo(typeof(long), ParamOrReturnType.I8_U8) },
            { typeof(ulong), new TypeInfo(typeof(ulong), ParamOrReturnType.I8_U8)},
            { typeof(float), new TypeInfo(typeof(float), ParamOrReturnType.R4)},
            { typeof(double), new TypeInfo(typeof(double), ParamOrReturnType.R8)},
            { typeof(IntPtr), new TypeInfo(null, ParamOrReturnType.I4_U4)},
            { typeof(UIntPtr), new TypeInfo(null, ParamOrReturnType.I4_U4)},
        };


        public CallConventionType CallConventionType { get; } = CallConventionType.General32;

        public override bool IsArch32 => true;

        public override TypeInfo PointerType => TypeInfo.s_i4u4;

        protected override Dictionary<Type, TypeInfo> CacheTypes => s_typeInfoCaches;

        protected override TypeInfo CreateValueType(Type type)
        {
            (int typeSize, int typeAligment) = ComputeSizeAndAligmentOfArch32(type);
            int actualAliment = typeAligment <= 4 ? 1 : 8;
            return CreateGeneralValueType(type, typeSize, actualAliment);
        }

        public IEnumerable<MethodBridgeSig> PrepareCommon1()
        {
            // (void + int32 + int64 + float + double) * (int32 + int64 + float + double) * (0 - 20) = 420
            TypeInfo typeVoid = new TypeInfo(typeof(void), ParamOrReturnType.VOID);
            TypeInfo typeInt = new TypeInfo(typeof(int), ParamOrReturnType.I4_U4);
            TypeInfo typeLong = new TypeInfo(typeof(long), ParamOrReturnType.I8_U8);
            TypeInfo typeFloat = new TypeInfo(typeof(float), ParamOrReturnType.R4);
            TypeInfo typeDouble = new TypeInfo(typeof(double), ParamOrReturnType.R8);
            int maxParamCount = 20;

            foreach (var returnType in new TypeInfo[] { typeVoid, typeInt, typeLong, typeFloat, typeDouble })
            {
                var rt = new ReturnInfo() { Type = returnType };
                foreach (var argType in new TypeInfo[] { typeInt, typeLong, typeFloat, typeDouble })
                {
                    for (int paramCount = 0; paramCount <= maxParamCount; paramCount++)
                    {
                        var paramInfos = new List<ParamInfo>();
                        for (int i = 0; i < paramCount; i++)
                        {
                            paramInfos.Add(new ParamInfo() { Type = argType });
                        }
                        var mbs = new MethodBridgeSig() { ReturnInfo = rt, ParamInfos = paramInfos };
                        yield return mbs;
                    }
                }
            }
        }

        public IEnumerable<MethodBridgeSig> PrepareCommon2()
        {
            // (void + int32 + int64 + float + double + v2f + v3f + v4f + s2) * (int32 + int64 + float + double + v2f + v3f + v4f + s2 + sr) ^ (0 - 2) = 399
            TypeInfo typeVoid = new TypeInfo(typeof(void), ParamOrReturnType.VOID);
            TypeInfo typeInt = new TypeInfo(typeof(int), ParamOrReturnType.I4_U4);
            TypeInfo typeLong = new TypeInfo(typeof(long), ParamOrReturnType.I8_U8);
            TypeInfo typeFloat = new TypeInfo(typeof(float), ParamOrReturnType.R4);
            TypeInfo typeDouble = new TypeInfo(typeof(double), ParamOrReturnType.R8);
            //TypeInfo typeStructRef = new TypeInfo(null, ParamOrReturnType.STRUCTURE_AS_REF_PARAM);

            int maxParamCount = 2;

            var argTypes = new TypeInfo[] { typeInt, typeLong, typeFloat, typeDouble };
            int paramTypeNum = argTypes.Length;
            foreach (var returnType in new TypeInfo[] { typeVoid, typeInt, typeLong, typeFloat, typeDouble })
            {
                var rt = new ReturnInfo() { Type = returnType };
                for (int paramCount = 0; paramCount <= maxParamCount; paramCount++)
                {
                    int totalCombinationNum = (int)Math.Pow(paramTypeNum, paramCount);

                    for (int k = 0; k < totalCombinationNum; k++)
                    {
                        var paramInfos = new List<ParamInfo>();
                        int c = k;
                        for (int i = 0; i < paramCount; i++)
                        {
                            paramInfos.Add(new ParamInfo { Type = argTypes[c % paramTypeNum] });
                            c /= paramTypeNum;
                        }
                        var mbs = new MethodBridgeSig() { ReturnInfo = rt, ParamInfos = paramInfos };
                        yield return mbs;
                    }
                }
            }
        }

        public override IEnumerable<MethodBridgeSig> GetPreserveMethods()
        {
            foreach (var method in PrepareCommon1())
            {
                yield return method;
            }
            foreach (var method in PrepareCommon2())
            {
                yield return method;
            }
        }

        public override void GenerateCall(MethodBridgeSig method, List<string> lines)
        {
            //int totalQuadWordNum = method.ParamInfos.Sum(p => p.GetParamSlotNum(this.CallConventionType)) + method.ReturnInfo.GetParamSlotNum(this.CallConventionType);
            int totalQuadWordNum = method.ParamInfos.Count + method.ReturnInfo.GetParamSlotNum(this.CallConventionType);

            string paramListStr = string.Join(", ", method.ParamInfos.Select(p => $"{p.Type.GetTypeName()} __arg{p.Index}").Concat(new string[] { "const MethodInfo* method" }));
            string paramTypeListStr = string.Join(", ", method.ParamInfos.Select(p => $"{p.Type.GetTypeName()}").Concat(new string[] { "const MethodInfo*" })); ;
            string paramNameListStr = string.Join(", ", method.ParamInfos.Select(p => p.Managed2NativeParamValue(this.CallConventionType)).Concat(new string[] { "method" }));

            string invokeAssignArgs = @$"
	if (huatuo::IsInstanceMethod(method))
	{{
        args[0].ptr = __this;
{string.Join("\n", method.ParamInfos.Skip(1).Select(p => $"\t\targs[{p.Index}].u64 = *(uint64_t*)__args[{p.Index - 1}];"))}
    }}
	else
	{{
{string.Join("\n", method.ParamInfos.Select(p => $"\t\targs[{p.Index}].u64 = *(uint64_t*)__args[{p.Index}];"))}
    }}
";

            lines.Add($@"
static {method.ReturnInfo.Type.GetTypeName()} __Native2ManagedCall_{method.CreateCallSigName()}({paramListStr})
{{
    StackObject args[{Math.Max(totalQuadWordNum, 1)}] = {{{string.Join(", ", method.ParamInfos.Select(p => p.Native2ManagedParamValue(this.CallConventionType)))} }};
    StackObject* ret = {(method.ReturnInfo.IsVoid ? "nullptr" : "args + " + method.ParamInfos.Count)};
    Interpreter::Execute(method, args, ret);
    {(!method.ReturnInfo.IsVoid ? $"return *({method.ReturnInfo.Type.GetTypeName()}*)ret;" : "")}
}}

static {method.ReturnInfo.Type.GetTypeName()} __Native2ManagedCall_AdjustorThunk_{method.CreateCallSigName()}({paramListStr})
{{
    StackObject args[{Math.Max(totalQuadWordNum, 1)}] = {{{string.Join(", ", method.ParamInfos.Select(p => (p.Index == 0 ? $"(uint64_t)(*(uint8_t**)&__arg{p.Index} + sizeof(Il2CppObject))" : p.Native2ManagedParamValue(this.CallConventionType))))} }};
    StackObject* ret = {(method.ReturnInfo.IsVoid ? "nullptr" : "args + " + method.ParamInfos.Count)};
    Interpreter::Execute(method, args, ret);
    {(!method.ReturnInfo.IsVoid ? $"return *({method.ReturnInfo.Type.GetTypeName()}*)ret;" : "")}
}}

static void __Managed2NativeCall_{method.CreateCallSigName()}(const MethodInfo* method, uint16_t* argVarIndexs, StackObject* localVarBase, void* ret)
{{
    if (huatuo::metadata::IsInstanceMethod(method) && !localVarBase[argVarIndexs[0]].obj)
    {{
        il2cpp::vm::Exception::RaiseNullReferenceException();
    }}
    Interpreter::RuntimeClassCCtorInit(method);
    typedef {method.ReturnInfo.Type.GetTypeName()} (*NativeMethod)({paramListStr});
    {(!method.ReturnInfo.IsVoid ? $"*({method.ReturnInfo.Type.GetTypeName()}*)ret = " : "")}((NativeMethod)(method->methodPointer))({paramNameListStr});
}}
");

        }


        public override void GenerateInvoke(MethodBridgeSig method, List<string> lines)
        {
            //int totalQuadWordNum = method.ParamInfos.Sum(p => p.GetParamSlotNum(this.CallConventionType)) + method.ReturnInfo.GetParamSlotNum(this.CallConventionType);
            int totalQuadWordNum = method.ParamInfos.Count + method.ReturnInfo.GetParamSlotNum(this.CallConventionType);

            string paramListStr = string.Join(", ", method.ParamInfos.Select(p => $"{p.Type.GetTypeName()} __arg{p.Index}").Concat(new string[] { "const MethodInfo* method" }));
            string paramTypeListStr = string.Join(", ", method.ParamInfos.Select(p => $"{p.Type.GetTypeName()}").Concat(new string[] { "const MethodInfo*" })); ;
            string paramNameListStr = string.Join(", ", method.ParamInfos.Select(p => p.Managed2NativeParamValue(this.CallConventionType)).Concat(new string[] { "method" }));

            string invokeAssignArgs = @$"
	if (huatuo::IsInstanceMethod(method))
	{{
        args[0].ptr = __this;
{string.Join("\n", method.ParamInfos.Skip(1).Select(p => $"\t\targs[{p.Index}].u64 = *(uint64_t*)__args[{p.Index - 1}];"))}
    }}
	else
	{{
{string.Join("\n", method.ParamInfos.Select(p => $"\t\targs[{p.Index}].u64 = *(uint64_t*)__args[{p.Index}];"))}
    }}
";


            lines.Add($@"
#ifdef HUATUO_UNITY_2021_OR_NEW
static void __Invoke_instance_{method.CreateCallSigName()}(Il2CppMethodPointer __methodPtr, const MethodInfo* __method, void* __this, void** __args, void* __ret)
{{
    StackObject args[{totalQuadWordNum + 1}] = {{ (uint64_t)AdjustValueTypeSelfPointer(({ConstStrings.typeObjectPtr})__this, __method)}};
    ConvertInvokeArgs(args+1, __method, __args);
    Interpreter::Execute(__method, args, __ret);
}}

static void __Invoke_static_{method.CreateCallSigName()}(Il2CppMethodPointer __methodPtr, const MethodInfo* __method, void* __this, void** __args, void* __ret)
{{
    StackObject args[{Math.Max(totalQuadWordNum, 1)}] = {{ }};
    ConvertInvokeArgs(args, __method, __args);
    Interpreter::Execute(__method, args, __ret);
}}
#else
static void* __Invoke_instance_{method.CreateCallSigName()}(Il2CppMethodPointer __methodPtr, const MethodInfo* __method, void* __this, void** __args)
{{
    StackObject args[{totalQuadWordNum + 1}] = {{ (uint64_t)AdjustValueTypeSelfPointer(({ConstStrings.typeObjectPtr})__this, __method)}};
    ConvertInvokeArgs(args+1, __method, __args);
    StackObject* ret = {(!method.ReturnInfo.IsVoid ? "args + " + (method.ParamInfos.Count + 1) : "nullptr")};
    Interpreter::Execute(__method, args, ret);
    return {(!method.ReturnInfo.IsVoid ? $"TranslateNativeValueToBoxValue(__method->return_type, ret)" : "nullptr")};
}}

static void* __Invoke_static_{method.CreateCallSigName()}(Il2CppMethodPointer __methodPtr, const MethodInfo* __method, void* __this, void** __args)
{{
    StackObject args[{Math.Max(totalQuadWordNum, 1)}] = {{ }};
    ConvertInvokeArgs(args, __method, __args);
    StackObject* ret = {(!method.ReturnInfo.IsVoid ? "args + " + method.ParamInfos.Count : "nullptr")};
    Interpreter::Execute(__method, args, ret);
    return {(!method.ReturnInfo.IsVoid ? $"TranslateNativeValueToBoxValue(__method->return_type, ret)" : "nullptr")};
}}
#endif
");
        }
    }

}
