using dnlib.DotNet;
using System.Collections.Generic;

namespace HybridCLR.Editor.MethodBridgeGenerator
{
    public class GenericMethod
    {
        public MethodDef Method { get; }

        public List<TypeSig> KlassInst { get; }

        public List<TypeSig> MethodInst { get; }

        private readonly int _hashCode;

        public GenericMethod(MethodDef method, List<TypeSig> classInst, List<TypeSig> methodInst)
        {
            Method = method;
            KlassInst = classInst;
            MethodInst = methodInst;
            _hashCode = ComputHashCode();
        }

        public GenericMethod ToGenericShare()
        {
            return new GenericMethod(Method, MetaUtil.ToShareTypeSigs(KlassInst), MetaUtil.ToShareTypeSigs(MethodInst));
        }

        public override bool Equals(object obj)
        {
            GenericMethod o = (GenericMethod)obj;
            return Method == o.Method
                && EqualityUtil.EqualsTypeSigArray(KlassInst, o.KlassInst)
                && EqualityUtil.EqualsTypeSigArray(MethodInst, o.MethodInst);
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        private int ComputHashCode()
        {
            int hash = MethodEqualityComparer.CompareDeclaringTypes.GetHashCode(Method);
            if (KlassInst != null)
            {
                hash = HashUtil.CombineHash(hash, HashUtil.ComputHash(KlassInst));
            }
            if (MethodInst != null)
            {
                hash = HashUtil.CombineHash(hash, HashUtil.ComputHash(MethodInst));
            }
            return hash;
        }
    }
}
