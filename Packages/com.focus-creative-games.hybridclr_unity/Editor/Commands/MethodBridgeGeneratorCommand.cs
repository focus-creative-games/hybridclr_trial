using HybridCLR.Editor;
using HybridCLR.Editor.Meta;
using HybridCLR.Editor.MethodBridgeGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace HybridCLR.Editor.Commands
{
    public class MethodBridgeGeneratorCommand
    {

        public static void CleanIl2CppBuildCache()
        {
            string il2cppBuildCachePath = SettingsUtil.Il2CppBuildCacheDir;
            if (!Directory.Exists(il2cppBuildCachePath))
            {
                return;
            }
            Debug.Log($"clean il2cpp build cache:{il2cppBuildCachePath}");
            Directory.Delete(il2cppBuildCachePath, true);
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

        private static void GenerateMethodBridgeCppFile(Analyzer analyzer, PlatformABI platform, string fileName)
        {
            string outputFile = $"{SettingsUtil.MethodBridgeCppDir}/{fileName}.cpp";
            var g = new Generator(new Generator.Options()
            {
                CallConvention = platform,
                TemplateCode = GetTemplateCode(platform),
                OutputFile = outputFile,
                GenericMethods = analyzer.GenericMethods,
                NotGenericMethods = analyzer.NotGenericMethods,
            });

            g.PrepareMethods();
            g.Generate();
            Debug.LogFormat("== output:{0} ==", outputFile);
            CleanIl2CppBuildCache();
        }

        [MenuItem("HybridCLR/GenerateMethodBridge", priority = 15)]
        public static void GenerateMethodBridge()
        {
            // 此处理论会有点问题，打每个平台的时候，都得针对当前平台生成桥接函数
            // 但影响不大，先这样吧
            CompileDllCommand.CompileDllActiveBuildTarget();


            var analyzer = new Analyzer(new Analyzer.Options
            {
                MaxIterationCount = Math.Min(20, SettingsUtil.GlobalSettings.maxMethodBridgeGenericIteration),
                Collector = new AssemblyReferenceDeepCollector(MetaUtil.CreateBuildTargetAssemblyResolver(EditorUserBuildSettings.activeBuildTarget), SettingsUtil.HotUpdateAssemblyNames),
            });

            analyzer.Run();

            GenerateMethodBridgeCppFile(analyzer, PlatformABI.Arm64, "MethodBridge_Arm64");
            GenerateMethodBridgeCppFile(analyzer, PlatformABI.Universal64, "MethodBridge_Universal64");
            GenerateMethodBridgeCppFile(analyzer, PlatformABI.Universal32, "MethodBridge_Universal32");
        }
    }
}
