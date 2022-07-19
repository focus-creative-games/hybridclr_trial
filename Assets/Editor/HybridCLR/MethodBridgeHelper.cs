using HybridCLR.Generators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace HybridCLR
{
    internal class MethodBridgeHelper
    {

        private static void CleanIl2CppBuildCache()
        {
            string il2cppBuildCachePath = BuildConfig.Il2CppBuildCacheDir;
            if (!Directory.Exists(il2cppBuildCachePath))
            {
                return;
            }
            Debug.Log($"clean il2cpp build cache:{il2cppBuildCachePath}");
            Directory.Delete(il2cppBuildCachePath, true);
        }

        private static List<Assembly> CollectDependentAssemblies(Dictionary<string, Assembly> allAssByName, List<Assembly> dlls)
        {
            for(int i = 0; i < dlls.Count; i++)
            {
                Assembly ass = dlls[i];
                foreach (var depAssName in ass.GetReferencedAssemblies())
                {
                    if (!allAssByName.ContainsKey(depAssName.Name))
                    {
                        Debug.Log($"ignore ref assembly:{depAssName.Name}");
                        continue;
                    }
                    Assembly depAss = allAssByName[depAssName.Name];
                    if (!dlls.Contains(depAss))
                    {
                        dlls.Add(depAss);
                    }
                }
            }
            return dlls;
        }

        private static List<Assembly> GetScanAssembiles()
        {
            var allAssByName = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(a => a.GetName().Name);
            CompileDllHelper.CompileDllActiveBuildTarget();

            var rootAssemblies = Directory.GetFiles(BuildConfig.GetHotFixDllsOutputDirByTarget(EditorUserBuildSettings.activeBuildTarget), "*.dll")
                .Select(dll => Path.GetFileNameWithoutExtension(dll)).Concat(GeneratorConfig.GetExtraAssembiles())
                .Where(name => allAssByName.ContainsKey(name)).Select(name => allAssByName[name]).ToList();
            CollectDependentAssemblies(allAssByName, rootAssemblies);
            return rootAssemblies;
        }

        private static void GenerateMethodBridgeCppFile(CallConventionType platform, string fileName)
        {
            string outputFile = $"{BuildConfig.MethodBridgeCppDir}/{fileName}.cpp";
            var g = new MethodBridgeGenerator(new MethodBridgeGeneratorOptions()
            {
                CallConvention = platform,
                Assemblies = GetScanAssembiles(),
                OutputFile = outputFile,
            });

            g.PrepareMethods();
            g.Generate();
            Debug.LogFormat("== output:{0} ==", outputFile);
            CleanIl2CppBuildCache();
        }

        [MenuItem("HybridCLR/MethodBridge/Arm64")]
        public static void MethodBridge_Arm64()
        {
            GenerateMethodBridgeCppFile(CallConventionType.Arm64, "MethodBridge_Arm64");
        }

        [MenuItem("HybridCLR/MethodBridge/General64")]
        public static void MethodBridge_General64()
        {
            GenerateMethodBridgeCppFile(CallConventionType.General64, "MethodBridge_General64");
        }

        [MenuItem("HybridCLR/MethodBridge/General32")]
        public static void MethodBridge_General32()
        {
            GenerateMethodBridgeCppFile(CallConventionType.General32, "MethodBridge_General32");
        }
    }
}
