using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridCLR.Generators
{

    public class ParamInfo
    {
        public TypeInfo Type { get; set; }

        public int Index { get; set; }

        //public bool IsNative2ManagedByAddress => Type.PorType >= ParamOrReturnType.STRUCT_NOT_PASS_AS_VALUE;
        public bool IsPassByAddress => Type.GetParamSlotNum() > 1;

        public int GetParamSlotNum(CallConventionType canv)
        {
            return 1;
        }

        public string Native2ManagedParamValue(CallConventionType canv)
        {
            return IsPassByAddress ? $"(uint64_t)&__arg{Index}" : $"*(uint64_t*)&__arg{Index}";
        }

        public string Managed2NativeParamValue(CallConventionType canv)
        {
            return $"*({Type.GetTypeName()}*)(localVarBase+argVarIndexs[{Index}])";
        }
    }

    public class ReturnInfo
    {
        public TypeInfo Type { get; set; }

        public bool IsVoid => Type.PorType == ParamOrReturnType.VOID;

        public int GetParamSlotNum(CallConventionType canv)
        {
            return Type.GetParamSlotNum();
        }
    }
}
