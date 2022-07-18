using HybridCLR.Generators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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


        private static void GenerateMethodBridgeCppFile(CallConventionType platform, string fileName)
        {
            string outputFile = $"{BuildConfig.MethodBridgeCppDir}/{fileName}.cpp";
            var g = new MethodBridgeGenerator(new MethodBridgeGeneratorOptions()
            {
                CallConvention = platform,
                Assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList(),
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
