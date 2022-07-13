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
    public static class BuildConfig
    {
#if !UNITY_IOS
        [InitializeOnLoadMethod]
        private static void Setup()
        {
            ///
            /// unity允许使用UNITY_IL2CPP_PATH环境变量指定il2cpp的位置，因此我们不再直接修改安装位置的il2cpp，
            /// 而是在本地目录
            ///
            var localIl2cppDir = LocalIl2CppDir;
            if (!Directory.Exists(localIl2cppDir))
            {
                Debug.LogError($"本地il2cpp目录:{localIl2cppDir} 不存在，未安装本地il2cpp。请手动执行一次 {BuildConfig.HybridCLRDataDir} 目录下的 init_local_il2cpp_data.bat 或者 init_local_il2cpp_data.sh 文件");
            }
            Environment.SetEnvironmentVariable("UNITY_IL2CPP_PATH", localIl2cppDir);
        }
#endif

        /// <summary>
        /// 需要在Prefab上挂脚本的热更dll名称列表，不需要挂到Prefab上的脚本可以不放在这里
        /// 但放在这里的dll即使勾选了 AnyPlatform 也会在打包过程中被排除
        /// 
        /// 另外请务必注意： 需要挂脚本的dll的名字最好别改，因为这个列表无法热更（上线后删除或添加某些非挂脚本dll没问题）。
        /// 
        /// 注意：多热更新dll不是必须的！大多数项目完全可以只有HotFix.dll这一个热更新模块,纯粹出于演示才故意设计了两个热更新模块。
        /// 另外，是否热更新跟dll名毫无关系，凡是不打包到主工程的，都可以是热更新dll。
        /// </summary>
        public static List<string> MonoHotUpdateDllNames { get; } = new List<string>()
        {
            "HotFix.dll",
        };

        /// <summary>
        /// 所有热更新dll列表。放到此列表中的dll在打包时OnFilterAssemblies回调中被过滤。
        /// </summary>
        public static List<string> AllHotUpdateDllNames { get; } = MonoHotUpdateDllNames.Concat(new List<string>
        {
            // 这里放除了s_monoHotUpdateDllNames以外的脚本不需要挂到资源上的dll列表
            "HotFix2.dll",
        }).ToList();

        public static List<string> AOTMetaDlls { get; } = new List<string>()
        {
            "mscorlib.dll",
            "System.dll",
            "System.Core.dll", // 如果使用了Linq，需要这个
        };

        public static List<string> AssetBundleFiles { get; } = new List<string>
        {
            "common",
        };

        public static string ProjectDir => Directory.GetParent(Application.dataPath).ToString();

        public static string ScriptingAssembliesJsonFile { get; } = "ScriptingAssemblies.json";

        public static string HybridCLRBuildCacheDir => Application.dataPath + "/HybridCLRBuildCache";

        public static string HotFixDllsOutputDir => $"{HybridCLRDataDir}/HotFixDlls";

        public static string AssetBundleOutputDir => $"{HybridCLRBuildCacheDir}/AssetBundleOutput";

        public static string AssetBundleSourceDataTempDir => $"{HybridCLRBuildCacheDir}/AssetBundleSourceData";

        public static string HybridCLRDataDir { get; } = $"{ProjectDir}/HybridCLRData";

        public static string AssembliesPostIl2CppStripDir => $"{HybridCLRDataDir}/AssembliesPostIl2CppStrip";

        public static string LocalIl2CppDir => $"{HybridCLRDataDir}/LocalIl2CppData/il2cpp";

        public static string MethodBridgeCppDir => $"{LocalIl2CppDir}/libil2cpp/huatuo/interpreter";

        public static string Il2CppBuildCacheDir { get; } = $"{ProjectDir}/Library/Il2cppBuildCache";

        public static string GetHotFixDllsOutputDirByTarget(BuildTarget target)
        {
            return $"{HotFixDllsOutputDir}/{target}";
        }

        public static string GetAssembliesPostIl2CppStripDir(BuildTarget target)
        {
            return $"{AssembliesPostIl2CppStripDir}/{target}";
        }

        public static string GetOriginBuildStripAssembliesDir(BuildTarget target)
        {
            return ProjectDir + "/" + (target == BuildTarget.Android ? "Temp/StagingArea/assets/bin/Data/Managed" : "Temp/StagingArea/Data/Managed/");
        }

        public static string GetAssetBundleOutputDirByTarget(BuildTarget target)
        {
            return $"{AssetBundleOutputDir}/{target}";
        }

        public static string GetAssetBundleTempDirByTarget(BuildTarget target)
        {
            return $"{AssetBundleSourceDataTempDir}/{target}";
        }

    }
}
