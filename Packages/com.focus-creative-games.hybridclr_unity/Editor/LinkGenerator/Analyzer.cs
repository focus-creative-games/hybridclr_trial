using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using HybridCLR.Editor.Meta;

namespace HybridCLR.Editor.LinkGenerator
{
    class Analyzer
    {

        private readonly HashSet<dnlib.DotNet.TypeDef> _refTypesByAssembly = new HashSet<dnlib.DotNet.TypeDef>();


        public Analyzer()
        {
        }

        public HashSet<TypeRef> CollectRefs(List<Assembly> rootAssemblies)
        {
            var assCollector = new AssemblyCollector(rootAssemblies);
            var rootAssemblyName = new HashSet<string>();
            foreach(var ass in rootAssemblies)
            {
                if (!rootAssemblyName.Add(ass.GetName().Name))
                {
                    throw new Exception($"assembly:{ass.GetName().Name} 重复");
                }
            }

            var typeRefs = new HashSet<TypeRef>(TypeEqualityComparer.Instance);
            foreach (var rootAss in rootAssemblies)
            {
                var dnAss = assCollector.LoadedModules[rootAss.GetName().Name];
                foreach(var type in dnAss.GetTypeRefs())
                {
                    if (!rootAssemblyName.Contains(type.DefinitionAssembly.Name))
                    {
                        typeRefs.Add(type);
                    }
                }
            }
            return typeRefs;
        }
    }
}
