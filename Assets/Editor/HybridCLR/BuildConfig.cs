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
    public static partial class BuildConfig
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
                Debug.LogError($"本地il2cpp目录:{localIl2cppDir} 不存在，未安装本地il2cpp。请手动执行一次 {HybridCLRDataDir} 目录下的 init_local_il2cpp_data.bat 或者 init_local_il2cpp_data.sh 文件");
            }
            Environment.SetEnvironmentVariable("UNITY_IL2CPP_PATH", localIl2cppDir);
        }
#endif

        public static string ProjectDir => Directory.GetParent(Application.dataPath).ToString();

        public static string ScriptingAssembliesJsonFile { get; } = "ScriptingAssemblies.json";

        public static string HybridCLRBuildCacheDir => Application.dataPath + "/HybridCLRBuildCache";

        public static string HotFixDllsOutputDir => $"{HybridCLRDataDir}/HotFixDlls";

        public static string AssetBundleOutputDir => $"{HybridCLRBuildCacheDir}/AssetBundleOutput";

        public static string AssetBundleSourceDataTempDir => $"{HybridCLRBuildCacheDir}/AssetBundleSourceData";

        public static string HybridCLRDataDir { get; } = $"{ProjectDir}/HybridCLRData";

        public static string AssembliesPostIl2CppStripDir => $"{HybridCLRDataDir}/AssembliesPostIl2CppStrip";

        public static string LocalIl2CppDir => $"{HybridCLRDataDir}/LocalIl2CppData/il2cpp";

        public static string MethodBridgeCppDir => $"{LocalIl2CppDir}/libil2cpp/hybridclr/interpreter";

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
#if UNITY_2021_1_OR_NEWER
#if UNITY_STANDALONE_WIN
            return $"{ProjectDir}/Library/Bee/artifacts/WinPlayerBuildProgram/ManagedStripped";
#elif UNITY_ANDROID
            return $"{ProjectDir}/Library/Bee/artifacts/Android/ManagedStripped";
#elif UNITY_IOS
            return $"{ProjectDir}/Library/PlayerDataCache/iOS/Data/Managed";
#elif UNITY_WEBGL
            return $"{ProjectDir}/Library/Bee/artifacts/WebGL/ManagedStripped";
#else
            throw new NotSupportedException("GetOriginBuildStripAssembliesDir");
#endif
#else
            return target == BuildTarget.Android ?
                $"{ProjectDir}/Temp/StagingArea/assets/bin/Data/Managed" :
                $"{ProjectDir}/Temp/StagingArea/Data/Managed/";
#endif
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
