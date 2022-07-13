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

        [MenuItem("HybridCLR/MethodBridge/Generate_X64")]
        public static void MethodBridge_X86()
        {
            GenerateMethodBridgeCppFile(CallConventionType.X64, "MethodBridge_x64");
        }

        [MenuItem("HybridCLR/MethodBridge/Generate_Arm64")]
        public static void MethodBridge_Arm64()
        {
            GenerateMethodBridgeCppFile(CallConventionType.Arm64, "MethodBridge_arm64");
        }

        [MenuItem("HybridCLR/MethodBridge/Generate_Armv7")]
        public static void MethodBridge_Armv7()
        {
            GenerateMethodBridgeCppFile(CallConventionType.Armv7, "MethodBridge_armv7");
        }
    }
}
