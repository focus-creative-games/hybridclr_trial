using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HybridCLR.Editor
{
    class AssemblyCollector
    {
        private readonly List<Assembly> _rootAssemblies;

        private readonly ModuleContext _modCtx;
        private readonly AssemblyResolver _asmResolver;

        public Dictionary<string, ModuleDefMD> LoadedModules { get; } = new Dictionary<string, ModuleDefMD>();

        public AssemblyCollector(List<Assembly> rootAssemblies)
        {
            _rootAssemblies = rootAssemblies;
            _modCtx = ModuleDef.CreateModuleContext();
            _asmResolver = (AssemblyResolver)_modCtx.AssemblyResolver;
            _asmResolver.EnableTypeDefCache = true;
            _asmResolver.UseGAC = false;
            LoadAllAssembiles();
        }

        private void LoadAllAssembiles()
        {
            foreach (var asm in _rootAssemblies)
            {
                LoadModule(asm.GetName().Name);
            }
        }

        private string GetDllPathByModuleName(string modName)
        {
            //foreach (var path in _conf.AsmSearchPaths)
            //{
            //    var fullName = Path.Combine(path, modName + ".dll");
            //    if (File.Exists(fullName))
            //    {
            //        return fullName;
            //    }
            //}
            var refAss = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == modName);
            if (refAss != null)
            {
                return refAss.Location;
            }
            return Assembly.Load(modName).Location;
            //throw new Exception($"not find module:{modName}");
        }

        private ModuleDefMD LoadModule(string moduleName)
        {
            // Debug.Log($"load module:{moduleName}");
            if (LoadedModules.TryGetValue(moduleName, out var mod))
            {
                return mod;
            }
            mod = DoLoadModule(GetDllPathByModuleName(moduleName));
            LoadedModules.Add(moduleName, mod);

            foreach (var refAsm in mod.GetAssemblyRefs())
            {
                LoadModule(refAsm.Name);
            }
            return mod;
        }

        private ModuleDefMD DoLoadModule(string dllPath)
        {
            //Debug.Log($"do load module:{dllPath}");
            ModuleDefMD mod = ModuleDefMD.Load(dllPath, _modCtx);
            _asmResolver.AddToCache(mod);
            return mod;
        }

    }
}
