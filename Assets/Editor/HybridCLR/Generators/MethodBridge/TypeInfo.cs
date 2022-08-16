using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HybridCLR.Generators.MethodBridge
{
    public class TypeInfo : IEquatable<TypeInfo>
    {

        public static readonly TypeInfo s_void = new TypeInfo(typeof(void), ParamOrReturnType.VOID);
        public static readonly TypeInfo s_i4u4 = new TypeInfo(null, ParamOrReturnType.I4_U4);
        public static readonly TypeInfo s_i8u8 = new TypeInfo(null, ParamOrReturnType.I8_U8);
        public static readonly TypeInfo s_i16 = new TypeInfo(null, ParamOrReturnType.I16);
        public static readonly TypeInfo s_ref = new TypeInfo(null, ParamOrReturnType.STRUCTURE_AS_REF_PARAM);

        public TypeInfo(Type type, ParamOrReturnType portype)
        {
            this.Type = type;
            PorType = portype;
            Size = 0;
        }

        public TypeInfo(Type type, ParamOrReturnType portype, int size)
        {
            this.Type = type;
            PorType = portype;
            Size = size;
        }

        public Type Type { get; }

        public ParamOrReturnType PorType { get; }

        public int Size { get; }

        public bool Equals(TypeInfo other)
        {
            return PorType == other.PorType && Size == other.Size;
        }

        public override bool Equals(object obj)
        {
            return Equals((TypeInfo)obj);
        }

        public override int GetHashCode()
        {
            return (int)PorType * 23 + Size;
        }

        public string CreateSigName()
        {
            return PorType switch
            {
                ParamOrReturnType.VOID => "v",
                ParamOrReturnType.I1_U1 => "i1",
                ParamOrReturnType.I2_U2 => "i2",
                ParamOrReturnType.I4_U4 => "i4",
                ParamOrReturnType.I8_U8 => "i8",
                ParamOrReturnType.R4 => "r4",
                ParamOrReturnType.R8 => "r8",
                ParamOrReturnType.I16 => "i16",
                ParamOrReturnType.STRUCTURE_AS_REF_PARAM => "sr",
                ParamOrReturnType.ARM64_HFA_FLOAT_2 => "vf2",
                ParamOrReturnType.ARM64_HFA_FLOAT_3 => "vf3",
                ParamOrReturnType.ARM64_HFA_FLOAT_4 => "vf4",
                ParamOrReturnType.ARM64_HFA_DOUBLE_2 => "vd2",
                ParamOrReturnType.ARM64_HFA_DOUBLE_3 => "vd3",
                ParamOrReturnType.ARM64_HFA_DOUBLE_4 => "vd4",
                ParamOrReturnType.STRUCTURE_ALIGN1 => "S" + Size,
                ParamOrReturnType.STRUCTURE_ALIGN2 => "A" + Size,
                ParamOrReturnType.STRUCTURE_ALIGN4 => "B" + Size,
                ParamOrReturnType.STRUCTURE_ALIGN8 => "C" + Size,
                _ => throw new NotSupportedException(PorType.ToString()),
            };
        }

        public string GetTypeName()
        {
            return PorType switch
            {
                ParamOrReturnType.VOID => "void",
                ParamOrReturnType.I1_U1 => "int8_t",
                ParamOrReturnType.I2_U2 => "int16_t",
                ParamOrReturnType.I4_U4 => "int32_t",
                ParamOrReturnType.I8_U8 => "int64_t",
                ParamOrReturnType.R4 => "float",
                ParamOrReturnType.R8 => "double",
                ParamOrReturnType.I16 => "ValueTypeSize16",
                ParamOrReturnType.STRUCTURE_AS_REF_PARAM => "uint64_t",
                ParamOrReturnType.ARM64_HFA_FLOAT_2 => "HtVector2f",
                ParamOrReturnType.ARM64_HFA_FLOAT_3 => "HtVector3f",
                ParamOrReturnType.ARM64_HFA_FLOAT_4 => "HtVector4f",
                ParamOrReturnType.ARM64_HFA_DOUBLE_2 => "HtVector2d",
                ParamOrReturnType.ARM64_HFA_DOUBLE_3 => "HtVector3d",
                ParamOrReturnType.ARM64_HFA_DOUBLE_4 => "HtVector4d",
                ParamOrReturnType.STRUCTURE_ALIGN1 => $"ValueTypeSize<{Size}>",
                ParamOrReturnType.STRUCTURE_ALIGN2 => $"ValueTypeSizeAlign2<{Size}>",
                ParamOrReturnType.STRUCTURE_ALIGN4 => $"ValueTypeSizeAlign4<{Size}>",
                ParamOrReturnType.STRUCTURE_ALIGN8 => $"ValueTypeSizeAlign8<{Size}>",
                _ => throw new NotImplementedException(PorType.ToString()),
            };
        }
        public int GetParamSlotNum()
        {
            switch (PorType)
            {
                case ParamOrReturnType.VOID: return 0;
                case ParamOrReturnType.I16: return 2;
                case ParamOrReturnType.STRUCTURE_AS_REF_PARAM: return 1;
                case ParamOrReturnType.ARM64_HFA_FLOAT_3: return 2;
                case ParamOrReturnType.ARM64_HFA_FLOAT_4: return 2;
                case ParamOrReturnType.ARM64_HFA_DOUBLE_2: return 2;
                case ParamOrReturnType.ARM64_HFA_DOUBLE_3: return 3;
                case ParamOrReturnType.ARM64_HFA_DOUBLE_4: return 4;
                case ParamOrReturnType.ARM64_HVA_8:
                case ParamOrReturnType.ARM64_HVA_16: throw new NotSupportedException();
                case ParamOrReturnType.STRUCTURE_ALIGN1:
                case ParamOrReturnType.STRUCTURE_ALIGN2:
                case ParamOrReturnType.STRUCTURE_ALIGN4:
                case ParamOrReturnType.STRUCTURE_ALIGN8: return (Size + 7) / 8;
                default:
                    {
                        Debug.Assert(PorType < ParamOrReturnType.STRUCT_NOT_PASS_AS_VALUE);
                        Debug.Assert(Size <= 8);
                        return 1;
                    }
            }
        }
    }
}
