using dnlib.DotNet;
using System.Collections.Generic;
using System.Linq;

namespace HybridCLR.Editor.MethodBridgeGenerator
{
    public class GenericClass
    {
        public TypeDef Type { get; }

        public List<TypeSig> KlassInst { get; }

        private readonly int _hashCode;

        public GenericClass(TypeDef type, List<TypeSig> classInst)
        {
            Type = type;
            KlassInst = classInst;
            _hashCode = ComputHashCode();
        }

        public GenericClass ToGenericShare()
        {
            return new GenericClass(Type, MetaUtil.ToShareTypeSigs(KlassInst));
        }

        public override bool Equals(object obj)
        {
            if (obj is GenericClass gc)
            {
                return Type == gc.Type && EqualityUtil.EqualsTypeSigArray(KlassInst, gc.KlassInst);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        private int ComputHashCode()
        {
            int hash = TypeEqualityComparer.Instance.GetHashCode(Type);
            if (KlassInst != null)
            {
                hash = HashUtil.CombineHash(hash, HashUtil.ComputHash(KlassInst));
            }
            return hash;
        }
    }
}
