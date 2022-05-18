using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HuaTuo.Generators
{
    internal class PlatformAdaptor_Arm64 : PlatformAdaptorBase
    {

        private static readonly Dictionary<Type, TypeInfo> s_typeInfoCaches = new Dictionary<Type, TypeInfo>()
        {
            { typeof(void), new TypeInfo(typeof(void), ParamOrReturnType.VOID)},
            { typeof(bool), new TypeInfo(typeof(bool), ParamOrReturnType.I8_U8)},
            { typeof(byte), new TypeInfo(typeof(byte), ParamOrReturnType.I8_U8)},
            { typeof(sbyte), new TypeInfo(typeof(sbyte), ParamOrReturnType.I8_U8) },
            { typeof(short), new TypeInfo(typeof(short), ParamOrReturnType.I8_U8) },
            { typeof(ushort), new TypeInfo(typeof(ushort), ParamOrReturnType.I8_U8) },
            { typeof(char), new TypeInfo(typeof(char), ParamOrReturnType.I8_U8) },
            { typeof(int), new TypeInfo(typeof(int), ParamOrReturnType.I8_U8) },
            { typeof(uint), new TypeInfo(typeof(uint), ParamOrReturnType.I8_U8) },
            { typeof(long), new TypeInfo(typeof(long), ParamOrReturnType.I8_U8) },
            { typeof(ulong), new TypeInfo(typeof(ulong), ParamOrReturnType.I8_U8)},
            { typeof(float), new TypeInfo(typeof(float), ParamOrReturnType.R8)},
            { typeof(double), new TypeInfo(typeof(double), ParamOrReturnType.R8)},
            { typeof(IntPtr), new TypeInfo(null, ParamOrReturnType.I8_U8)},
            { typeof(UIntPtr), new TypeInfo(null, ParamOrReturnType.I8_U8)},
            { typeof(Vector2), new TypeInfo(typeof(Vector2), ParamOrReturnType.ARM64_HFA_FLOAT_2) },
            { typeof(Vector3), new TypeInfo(typeof(Vector3), ParamOrReturnType.ARM64_HFA_FLOAT_3) },
            { typeof(Vector4), new TypeInfo(typeof(Vector4), ParamOrReturnType.ARM64_HFA_FLOAT_4) },
            { typeof(System.Numerics.Vector2), new TypeInfo(typeof(System.Numerics.Vector2), ParamOrReturnType.ARM64_HFA_FLOAT_2) },
            { typeof(System.Numerics.Vector3), new TypeInfo(typeof(System.Numerics.Vector3), ParamOrReturnType.ARM64_HFA_FLOAT_3) },
            { typeof(System.Numerics.Vector4), new TypeInfo(typeof(System.Numerics.Vector4), ParamOrReturnType.ARM64_HFA_FLOAT_4) },
        };

        public CallConventionType CallConventionType { get; } = CallConventionType.Arm64;

        public override TypeInfo Create(ParameterInfo param, bool returnValue)
        {
            var type = param.ParameterType;
            if (type.IsByRef)
            {
                return TypeInfo.s_i8u8;
            }
            if (type == typeof(void))
            {
                return TypeInfo.s_void;
            }
            if (!type.IsValueType)
            {
                return TypeInfo.s_i8u8;
            }
            if (s_typeInfoCaches.TryGetValue(type, out var cache))
            {
                return cache;
            }
            var ti = CreateValueType(type, returnValue);
            // s_typeInfoCaches.Add(type, ti);
            return ti;
        }

        private static TypeInfo CreateNormalValueTypeBySize(Type type, int typeSize, bool returnValue)
        {
            if (typeSize <= 8)
            {
                return new TypeInfo(type, ParamOrReturnType.I8_U8);
            }
            if (typeSize <= 16)
            {
                return new TypeInfo(type, ParamOrReturnType.STRUCTURE_SIZE_LE_16);
            }
            else if(returnValue)
            {
                return new TypeInfo(type, ParamOrReturnType.STRUCTURE_SIZE_GT_16, typeSize);
            }
            else
            {
                return TypeInfo.s_valueTypeAsParam;
            }
        }

        private static TypeInfo CreateValueType(Type type, bool returnValue)
        {
            int typeSize = ComputeSizeOf(type);
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToArray();
            if (fields.Length < 2 || fields.Length > 4)
            {
                return CreateNormalValueTypeBySize(type, typeSize, returnValue);
            }

            // FIXME 未处理 HSV
            Type fieldType = null;
            bool isHFA = true;
            foreach(var field in fields)
            {
                if (field.FieldType != fieldType)
                {
                    if (fieldType == null)
                    {
                        fieldType = field.FieldType;
                        if (fieldType != typeof(float) && fieldType != typeof(double))
                        {
                            isHFA = false;
                            break;
                        }
                    }
                    else
                    {
                        isHFA = false;
                        break;
                    }
                }
            }
            if (isHFA)
            {
                if (fieldType == typeof(float))
                {
                    if (typeSize != fields.Length * 4)
                    {
                        isHFA = false;
                    }
                }
                else if (fieldType == typeof(double))
                {
                    if (typeSize != fields.Length * 8)
                    {
                        isHFA = false;
                    }
                }
            }
            if (isHFA)
            {
                if (fieldType == typeof(float))
                {
                    switch(fields.Length)
                    {
                        case 2: return new TypeInfo(type, ParamOrReturnType.ARM64_HFA_FLOAT_2);
                        case 3: return new TypeInfo(type, ParamOrReturnType.ARM64_HFA_FLOAT_3);
                        case 4: return new TypeInfo(type, ParamOrReturnType.ARM64_HFA_FLOAT_4);
                        default: throw new NotSupportedException();
                    }
                }
                else if (fieldType == typeof(double))
                {
                    switch (fields.Length)
                    {
                        case 2: return new TypeInfo(type, ParamOrReturnType.ARM64_HFA_DOUBLE_2);
                        case 3: return new TypeInfo(type, ParamOrReturnType.ARM64_HFA_DOUBLE_3);
                        case 4: return new TypeInfo(type, ParamOrReturnType.ARM64_HFA_DOUBLE_4);
                        default: throw new NotSupportedException();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                return CreateNormalValueTypeBySize(type, typeSize, returnValue);
            }

        }

        public IEnumerable<MethodBridgeSig> PrepareCommon2()
        {
            // (void + int64 + float + v2f + v3f + v4f + s2) * (int64 + float + v2f + v3f + v4f + s2 + sr) ^ (0 - 2) = 399
            TypeInfo typeVoid = new TypeInfo(typeof(void), ParamOrReturnType.VOID);
            TypeInfo typeLong = new TypeInfo(typeof(long), ParamOrReturnType.I8_U8);
            TypeInfo typeDouble = new TypeInfo(typeof(double), ParamOrReturnType.R8);
            TypeInfo typeV2f = new TypeInfo(typeof(Vector2), ParamOrReturnType.ARM64_HFA_FLOAT_2);
            TypeInfo typeV3f = new TypeInfo(typeof(Vector3), ParamOrReturnType.ARM64_HFA_FLOAT_3);
            TypeInfo typeV4f = new TypeInfo(typeof(Vector4), ParamOrReturnType.ARM64_HFA_FLOAT_4);
            TypeInfo typeStructLe16 = new TypeInfo(null, ParamOrReturnType.STRUCTURE_SIZE_LE_16);
            TypeInfo typeStructRef = new TypeInfo(null, ParamOrReturnType.STRUCTURE_AS_REF_PARAM);

            int maxParamCount = 2;

            var argTypes = new TypeInfo[] { typeLong, typeDouble, typeV2f, typeV3f, typeV4f, typeStructLe16, typeStructRef };
            int paramTypeNum = argTypes.Length;
            foreach (var returnType in new TypeInfo[] { typeVoid, typeLong, typeDouble, typeV2f, typeV3f, typeV4f, typeStructRef })
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
                        var mbs = new MethodBridgeSig() { Method = null, ReturnInfo = rt, ParamInfos = paramInfos };
                        yield return mbs;
                    }
                }
            }
        }

        public override IEnumerable<MethodBridgeSig> GetPreserveMethods()
        {
            foreach(var method in PrepareCommon2())
            {
                yield return method;
            }
        }

        protected override void GenMethod(MethodBridgeSig method, List<string> lines)
        {
            int totalQuadWordNum = method.ParamInfos.Sum(p => p.GetParamSlotNum(CallConventionType.X64)) + method.ReturnInfo.GetParamSlotNum(CallConventionType.X64);


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

            //if (method.ReturnInfo.PassReturnAsParam)
            {
//                string paramAndReturnListStr = string.Join(", ", new string[] { "void* __ret" }.Concat(method.ParamInfos.Select(p => $"{p.Type.GetTypeName()} __arg{p.Index}").Concat(new string[] { "const MethodInfo* method" })));
//                string paramAndReturnNameListStr = string.Join(", ", new string[] { "__ret" }.Concat(method.ParamInfos.Select(p => p.Managed2NativeParamValue(this.CallConventionType)).Concat(new string[] { "method" })));

//                lines.Add($@"
//static void __Native2ManagedCall_{method.CreateCallSigName()}({paramAndReturnListStr})
//{{
//    StackObject args[{Math.Max(totalQuadWordNum, 1)}] = {{{string.Join(", ", method.ParamInfos.Select(p => p.Native2ManagedParamValue(this.CallConventionType)))} }};
//    Interpreter::Execute(method, args, __ret);
//}}

//static void __Native2ManagedCall_AdjustorThunk_{method.CreateCallSigName()}({paramAndReturnListStr})
//{{
//    StackObject args[{Math.Max(totalQuadWordNum, 1)}] = {{{string.Join(", ", method.ParamInfos.Select(p => (p.Index == 0 ? $"*(uint8_t**)&__arg{p.Index} + sizeof(Il2CppObject)" : p.Native2ManagedParamValue(this.CallConventionType))))} }};
//    Interpreter::Execute(method, args, __ret);
//}}

//static void __Managed2NativeCall_{method.CreateCallSigName()}(const MethodInfo* method, uint16_t* argVarIndexs, StackObject* localVarBase, void* __ret)
//{{
//    if (huatuo::metadata::IsInstanceMethod(method) && !localVarBase[argVarIndexs[0]].obj)
//    {{
//        il2cpp::vm::Exception::RaiseNullReferenceException();
//    }}
//    Interpreter::RuntimeClassCCtorInit(method);
//    typedef void (*NativeMethod)({paramListStr});
//    asm(""mov x8, %[ret]"" :: [ret] ""r"" (__ret) :);
//    ((NativeMethod)(method->methodPointer))({paramNameListStr});
//}}
//");
            }
            //else
            {
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
    StackObject args[{Math.Max(totalQuadWordNum, 1)}] = {{{string.Join(", ", method.ParamInfos.Select(p => (p.Index == 0 ? $"*(uint8_t**)&__arg{p.Index} + sizeof(Il2CppObject)" : p.Native2ManagedParamValue(this.CallConventionType))))} }};
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

            lines.Add($@"
#ifdef HUATUO_UNITY_2021_OR_NEW
static void __Invoke_instance_{method.CreateCallSigName()}(Il2CppMethodPointer __methodPtr, const MethodInfo* __method, void* __this, void** __args, void* __ret)
{{
    StackObject args[{totalQuadWordNum + 1}] = {{ AdjustValueTypeSelfPointer(({ConstStrings.typeObjectPtr})__this, __method)}};
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
    StackObject args[{totalQuadWordNum + 1}] = {{ AdjustValueTypeSelfPointer(({ConstStrings.typeObjectPtr})__this, __method)}};
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
