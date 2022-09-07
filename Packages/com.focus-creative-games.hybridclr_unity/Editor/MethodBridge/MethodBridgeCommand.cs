using HybridCLR.Editor;
using HybridCLR.Editor.MethodBridge;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace HybridCLR.Editor.MethodBridge
{
    internal class MethodBridgeCommand
    {

        private static void CleanIl2CppBuildCache()
        {
            string il2cppBuildCachePath = SettingsUtil.Il2CppBuildCacheDir;
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
            var allAssByName = new Dictionary<string, Assembly>();
            foreach(var ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                allAssByName[ass.GetName().Name] = ass;
            }
            //CompileDllHelper.CompileDllActiveBuildTarget();

            var rootAssemblies = SettingsUtil.HotUpdateAssemblies
                .Select(dll => Path.GetFileNameWithoutExtension(dll))
                .Where(name => allAssByName.ContainsKey(name)).Select(name => allAssByName[name]).ToList();
            //var rootAssemblies = GeneratorConfig.GetExtraAssembiles()
            //    .Where(name => allAssByName.ContainsKey(name)).Select(name => allAssByName[name]).ToList();
            CollectDependentAssemblies(allAssByName, rootAssemblies);
            rootAssemblies.Sort((a, b) => a.GetName().Name.CompareTo(b.GetName().Name));
            Debug.Log($"assembly count:{rootAssemblies.Count}");
            foreach(var ass in rootAssemblies)
            {
                //Debug.Log($"scan assembly:{ass.GetName().Name}");
            }
            return rootAssemblies;
        }

        private static void GenerateMethodBridgeCppFile(PlatformABI platform, string fileName, bool optimized)
        {
            string outputFile = $"{SettingsUtil.MethodBridgeCppDir}/{fileName}.cpp";
            var g = new MethodBridgeGenerator(new MethodBridgeGeneratorOptions()
            {
                CallConvention = platform,
                HotfixAssemblies = SettingsUtil.HotUpdateAssemblies.Select(name =>
                    AppDomain.CurrentDomain.GetAssemblies().First(ass => ass.GetName().Name + ".dll" == name)).ToList(),
                AllAssemblies = optimized ? GetScanAssembiles() : AppDomain.CurrentDomain.GetAssemblies().ToList(),
                TemplateCode = GetTemplateCode(platform),
                OutputFile = outputFile,
                Optimized = optimized,
            });

            g.PrepareMethods();
            g.Generate();
            Debug.LogFormat("== output:{0} ==", outputFile);
            CleanIl2CppBuildCache();
        }

        private static string GetTemplateCode(PlatformABI platform)
        {
            string tplFile;

            switch (platform)
            {
                case PlatformABI.Universal32: tplFile = "Universal32"; break;
                case PlatformABI.Universal64: tplFile = "Universal64"; break;
                case PlatformABI.Arm64: tplFile = "Arm64"; break;
                default: throw new NotSupportedException();
            };
            return Resources.Load<TextAsset>($"Templates/MethodBridge_{tplFile}.cpp").text;
        }

        public static void GenerateMethodBridgeAll(bool optimized)
        {
            GenerateMethodBridgeCppFile(PlatformABI.Arm64, "MethodBridge_Arm64", optimized);
            GenerateMethodBridgeCppFile(PlatformABI.Universal64, "MethodBridge_Universal64", optimized);
            GenerateMethodBridgeCppFile(PlatformABI.Universal32, "MethodBridge_Universal32", optimized);
        }

        [MenuItem("HybridCLR/MethodBridge/All_高度精简")]
        public static void MethodBridge_All_Optimized()
        {
            GenerateMethodBridgeAll(true);
        }

        [MenuItem("HybridCLR/MethodBridge/All_完整(新手及开发期推荐)")]
        public static void MethodBridge_All_Normal()
        {
            GenerateMethodBridgeAll(false);
        }
    }
}
