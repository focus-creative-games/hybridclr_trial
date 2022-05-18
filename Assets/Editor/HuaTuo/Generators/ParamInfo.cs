using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaTuo.Generators
{

    public class ParamInfo
    {
        public TypeInfo Type { get; set; }

        public int Index { get; set; }

        public bool IsNative2ManagedByAddress => Type.PorType >= ParamOrReturnType.STRUCT_NOT_PASS_AS_VALUE;
        public bool IsManaged2NativeDereference => Type.PorType != ParamOrReturnType.STRUCTURE_AS_REF_PARAM;

        public int GetParamSlotNum(CallConventionType canv)
        {
            return 1;
        }

        public string Native2ManagedParamValue(CallConventionType canv)
        {
            switch(canv)
            {
                case CallConventionType.X64:
                    {
                        return IsNative2ManagedByAddress ? $"(void*)&__arg{Index}" : $"*(void**)&__arg{Index}";
                    }
                case CallConventionType.Arm64:
                    {
                        return IsNative2ManagedByAddress ? $"(void*)&__arg{Index}" : $"*(void**)&__arg{Index}";
                    }
                case CallConventionType.Arm32:
                    {
                        throw new NotImplementedException();
                    }
                default:
                    throw new NotSupportedException();
            }
        }

        public string Managed2NativeParamValue(CallConventionType canv)
        {
            return IsManaged2NativeDereference ?  $"*({Type.GetTypeName()}*)(localVarBase+argVarIndexs[{Index}])" : $"({Type.GetTypeName()})(localVarBase+argVarIndexs[{Index}])";
        }
    }

    public class ReturnInfo
    {
        public TypeInfo Type { get; set; }

        public bool IsVoid => Type.PorType == ParamOrReturnType.VOID;

        public bool PassReturnAsParam => Type.PorType == ParamOrReturnType.STRUCTURE_AS_REF_PARAM;

        public int GetParamSlotNum(CallConventionType canv)
        {
             return Type.PorType == ParamOrReturnType.VOID ? 0 : 1;
        }
    }
}
