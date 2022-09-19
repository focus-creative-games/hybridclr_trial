using dnlib.DotNet;
using HybridCLR.Editor.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HybridCLR.Editor.MethodBridgeGenerator
{

    public class Analyzer
    {
        public class Options
        {
            public AssemblyReferenceDeepCollector Collector { get; set; }

            public int MaxIterationCount { get; set; }
        }

        private readonly int _maxInterationCount;
        private readonly AssemblyReferenceDeepCollector _assemblyCollector;

        private readonly List<TypeDef> _typeDefs = new List<TypeDef>();
        private readonly List<MethodDef> _notGenericMethods = new List<MethodDef>();

        private readonly HashSet<GenericClass> _genericTypes = new HashSet<GenericClass>();
        private readonly HashSet<GenericMethod> _genericMethods = new HashSet<GenericMethod>();

        private List<GenericMethod> _processingMethods = new List<GenericMethod>();
        private List<GenericMethod> _newMethods = new List<GenericMethod>();

        public IReadOnlyList<TypeDef> TypeDefs => _typeDefs;

        public IReadOnlyList<MethodDef> NotGenericMethods => _notGenericMethods;

        public IReadOnlyCollection<GenericClass> GenericTypes => _genericTypes;

        public IReadOnlyCollection<GenericMethod> GenericMethods => _genericMethods;

        public Analyzer(Options options)
        {
            _maxInterationCount = options.MaxIterationCount;
            _assemblyCollector = options.Collector;
        }

        private void TryAddAndWalkGenericType(GenericClass gc)
        {
            gc = gc.ToGenericShare();
            if (_genericTypes.Add(gc))
            {
                WalkType(gc);
            }
        }

        private void WalkType(GenericClass gc)
        {
            //Debug.Log($"typespec:{sig} {sig.GenericType} {sig.GenericType.TypeDefOrRef.ResolveTypeDef()}");
            //Debug.Log($"== walk generic type:{new GenericInstSig(gc.Type.ToTypeSig().ToClassOrValueTypeSig(), gc.KlassInst)}");
            ITypeDefOrRef baseType = gc.Type.BaseType;
            if (baseType != null && baseType.TryGetGenericInstSig() != null)
            {
                GenericClass parentType = ResolveClass((TypeSpec)baseType, new GenericArgumentContext(gc.KlassInst, null));
                TryAddAndWalkGenericType(parentType);
            }
            foreach (var method in gc.Type.Methods)
            {
                if (method.HasGenericParameters || !method.HasBody || method.Body.Instructions == null)
                {
                    continue;
                }
                var gm = new GenericMethod(method, gc.KlassInst, null).ToGenericShare();
                //Debug.Log($"add method:{gm.Method} {gm.KlassInst}");
                
                if (_genericMethods.Add(gm))
                {
                    _newMethods.Add(gm);
                }
            }
        }

        private void WalkType(TypeDef typeDef)
        {
            _typeDefs.Add(typeDef);
            if (typeDef.HasGenericParameters)
            {
                return;
            }
            ITypeDefOrRef baseType = typeDef.BaseType;
            if (baseType != null && baseType.TryGetGenericInstSig() != null)
            {
                GenericClass gc = ResolveClass((TypeSpec)baseType, null);
                TryAddAndWalkGenericType(gc);
            }
            foreach (var method in typeDef.Methods)
            {
                if (method.HasGenericParameters)
                {
                    continue;
                }
                _notGenericMethods.Add(method);
                //if (method.HasBody && method.Body.Instructions != null)
                //{
                //    //WalkMethod(method, null, null);
                //}
            }
        }

        private void Prepare()
        {
            // 将所有非泛型函数全部加入函数列表，同时立马walk这些method。
            // 后续迭代中将只遍历MethodSpec
            foreach(var ass in _assemblyCollector.LoadedModules.Values)
            {
                Debug.Log($"module:{ass.Name}");
            }
            foreach (var ass in _assemblyCollector.LoadedModules.Values)
            {
                foreach (TypeDef typeDef in ass.GetTypes())
                {
                    WalkType(typeDef);
                }

                for (uint rid = 1, n = ass.Metadata.TablesStream.TypeSpecTable.Rows; rid <= n; rid++)
                {
                    var ts = ass.ResolveTypeSpec(rid);
                    if (!ts.ContainsGenericParameter)
                    {
                        var cs = ResolveClass(ts, null)?.ToGenericShare();
                        if (cs != null)
                        {
                            TryAddAndWalkGenericType(cs);
                        }
                    }
                }

                for (uint rid = 1, n = ass.Metadata.TablesStream.MethodSpecTable.Rows; rid <= n; rid++)
                {
                    var ms = ass.ResolveMethodSpec(rid);
                    if(ms.DeclaringType.ContainsGenericParameter || ms.GenericInstMethodSig.ContainsGenericParameter)
                    {
                        continue;
                    }
                    var gm = ResolveMethod(ms, null)?.ToGenericShare();
                    if (gm == null)
                    {
                        continue;
                    }

                    if (_genericMethods.Add(gm))
                    {
                        _newMethods.Add(gm);
                    }
                    //if (gm.KlassInst != null)
                    //{
                    //    TryAddAndWalkGenericType(new GenericClass(gm.Method.DeclaringType, gm.KlassInst));
                    //}
                }
            }
            Debug.Log($"PostPrepare allMethods:{_notGenericMethods.Count} newMethods:{_newMethods.Count}");
        }

        private void RecursiveCollect()
        {
            for (int i = 0; i < _maxInterationCount && _newMethods.Count > 0; i++)
            {
                var temp = _processingMethods;
                _processingMethods = _newMethods;
                _newMethods = temp;
                _newMethods.Clear();

                foreach (var method in _processingMethods)
                {
                    WalkMethod(method.Method, method.KlassInst, method.MethodInst);
                }
                Debug.Log($"iteration:[{i}] allMethods:{_notGenericMethods.Count} genericClass:{_genericTypes.Count} genericMethods:{_genericMethods.Count} newMethods:{_newMethods.Count}");
            }
        }

        private GenericMethod ResolveMethod(IMethod method, GenericArgumentContext ctx)
        {
            //Debug.Log($"== resolve method:{method}");
            TypeDef typeDef = null;
            List<TypeSig> klassInst = null;
            List<TypeSig> methodInst = null;

            MethodDef methodDef = null;


            var decalringType = method.DeclaringType;
            typeDef = decalringType.ResolveTypeDef();
            if (typeDef == null)
            {
                throw new Exception($"{decalringType}");
            }
            GenericInstSig gis = decalringType.TryGetGenericInstSig();
            if (gis != null)
            {
                klassInst = ctx != null ? gis.GenericArguments.Select(ga => MetaUtil.Inflate(ga, ctx)).ToList() : gis.GenericArguments.ToList();
            }
            methodDef = method.ResolveMethodDef();
            if (methodDef == null)
            {
                return null;
            }
            if (method is MethodSpec methodSpec)
            {
                methodInst = ctx != null ? methodSpec.GenericInstMethodSig.GenericArguments.Select(ga => MetaUtil.Inflate(ga, ctx)).ToList()
                    : methodSpec.GenericInstMethodSig.GenericArguments.ToList();
            }
            return new GenericMethod(methodDef, klassInst, methodInst);
        }

        private GenericClass ResolveClass(TypeSpec type, GenericArgumentContext ctx)
        {
            var sig = type.TypeSig.ToGenericInstSig();
            if (sig == null)
            {
                return null;
            }
            TypeDef def = type.ResolveTypeDef();
            var klassInst = ctx != null ? sig.GenericArguments.Select(ga => MetaUtil.Inflate(ga, ctx)).ToList() : sig.GenericArguments.ToList();
            return new GenericClass(def, klassInst);
        }

        private Dictionary<MethodDef, List<IMethod>> _methodEffectInsts = new Dictionary<MethodDef, List<IMethod>>();

        private void WalkMethod(MethodDef method, List<TypeSig> klassGenericInst, List<TypeSig> methodGenericInst)
        {
            if (klassGenericInst != null || methodGenericInst != null)
            {
                //var typeSig = klassGenericInst != null ? new GenericInstSig(method.DeclaringType.ToTypeSig().ToClassOrValueTypeSig(), klassGenericInst) : method.DeclaringType?.ToTypeSig();
                //Debug.Log($"== walk generic method {typeSig}::{method.Name} {method.MethodSig}");
            }
            else
            {
                //Debug.Log($"== walk not geneeric method:{method}");
            }
            var ctx = new GenericArgumentContext(klassGenericInst, methodGenericInst);

            if (_methodEffectInsts.TryGetValue(method, out var effectInsts))
            {
                foreach(var met in effectInsts)
                {
                    var resolveMet = ResolveMethod(met, ctx)?.ToGenericShare();
                    if (_genericMethods.Add(resolveMet))
                    {
                        _newMethods.Add(resolveMet);
                    }
                    if (resolveMet.KlassInst != null)
                    {
                        TryAddAndWalkGenericType(new GenericClass(resolveMet.Method.DeclaringType, resolveMet.KlassInst));
                    }
                }
                return;
            }

            var body = method.Body;
            if (body == null || !body.HasInstructions)
            {
                return;
            }

            effectInsts = new List<IMethod>();
            foreach (var inst in body.Instructions)
            {
                if (inst.Operand == null)
                {
                    continue;
                }
                switch (inst.Operand)
                {
                    case IMethod met:
                    {
                        if (!met.IsMethod)
                        {
                            continue;
                        }
                        var resolveMet = ResolveMethod(met, ctx)?.ToGenericShare();
                        if (resolveMet == null)
                        {
                            continue;
                        }
                        effectInsts.Add(met);
                        if (_genericMethods.Add(resolveMet))
                        {
                            _newMethods.Add(resolveMet);
                        }
                        if (resolveMet.KlassInst != null)
                        {
                            TryAddAndWalkGenericType(new GenericClass(resolveMet.Method.DeclaringType, resolveMet.KlassInst));
                        }
                        break;
                    }
                    case ITokenOperand token:
                    {
                        //GenericParamContext paramContext = method.HasGenericParameters || method.DeclaringType.HasGenericParameters ?
                        //            new GenericParamContext(method.DeclaringType, method) : default;
                        //method.Module.ResolveToken(token.MDToken, paramContext);
                        break;
                    }
                }
            }
            _methodEffectInsts.Add(method, effectInsts);
        }

        public void Run()
        {
            Prepare();
            RecursiveCollect();
        }
    }
}
