using HybridCLR.Editor.Commands;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HybridCLR.Editor
{
    public class BuildPlayerCommand
    {
        public static void CopyAssetBundles(string outputDir)
        {
            Directory.CreateDirectory(outputDir);

            foreach(var srcFile in Directory.GetFiles(Application.streamingAssetsPath))
            {
                string dstFile = $"{outputDir}/{Path.GetFileName(srcFile)}";
                File.Copy(srcFile, dstFile, true);
            }
        }

        [MenuItem("HybridCLR/Build/Win64")]
        public static void Build_Win64()
        {
            BuildTarget target = BuildTarget.StandaloneWindows64;
            BuildTarget activeTarget = EditorUserBuildSettings.activeBuildTarget;
            if (activeTarget != BuildTarget.StandaloneWindows64 && activeTarget != BuildTarget.StandaloneWindows)
            {
                Debug.LogError("请先切到Win平台再打包");
                return;
            }
            // Get filename.
            string outputPath = $"{SettingsUtil.ProjectDir}/Release-Win64";

            var buildOptions = BuildOptions.CompressWithLz4;

            string location = $"{outputPath}/HybridCLRTrial.exe";

            PrebuildCommand.GenerateAll();
            Debug.Log("====> Build App");
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions()
            {
                scenes = new string[] { "Assets/Scenes/main.unity" },
                locationPathName = location,
                options = buildOptions,
                target = target,
                targetGroup = BuildTargetGroup.Standalone,
            };

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                Debug.LogError("打包失败");
                return;
            }

            Debug.Log("====> Build AssetBundle");
            AssetBundleBuildCommand.BuildAssetBundleByTarget(target, true);
            Debug.Log("====> 复制 AssetBundle");
            CopyAssetBundles($"{outputPath}/HybridCLRTrial_Data/StreamingAssets");

#if UNITY_EDITOR
            Application.OpenURL($"file:///{location}");
#endif
        }

        [MenuItem("HybridCLR/Build/Win32")]
        public static void Build_Win32()
        {
            BuildTarget target = BuildTarget.StandaloneWindows;
            BuildTarget activeTarget = EditorUserBuildSettings.activeBuildTarget;
            if (activeTarget != BuildTarget.StandaloneWindows64 && activeTarget != BuildTarget.StandaloneWindows)
            {
                Debug.LogError("请先切到Win平台再打包");
                return;
            }
            // Get filename.
            string outputPath = $"{SettingsUtil.ProjectDir}/Release-Win32";

            PrebuildCommand.GenerateAll();

            var buildOptions = BuildOptions.CompressWithLz4;

            string location = $"{outputPath}/HybridCLRTrial.exe";

            Debug.Log("====> Build App");
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions()
            {
                scenes = new string[] { "Assets/Scenes/main.unity" },
                locationPathName = location,
                options = buildOptions,
                target = target,
                targetGroup = BuildTargetGroup.Standalone,
            };

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                Debug.LogError("打包失败");
                return;
            }

            Debug.Log("====> Build AssetBundle");
            AssetBundleBuildCommand.BuildAssetBundleByTarget(target, true);
            Debug.Log("====> 复制 AssetBundle");
            CopyAssetBundles($"{outputPath}/HybridCLRTrial_Data/StreamingAssets");

#if UNITY_EDITOR
            Application.OpenURL($"file:///{outputPath}");
#endif
        }

        [MenuItem("HybridCLR/Build/Android64")]
        public static void Build_Android64()
        {
            BuildTarget target = BuildTarget.Android;
            BuildTarget activeTarget = EditorUserBuildSettings.activeBuildTarget;
            if (activeTarget != BuildTarget.Android)
            {
                Debug.LogError("请先切到Android平台再打包");
                return;
            }
            // Get filename.
            string outputPath = $"{SettingsUtil.ProjectDir}/Release-Android";

            var buildOptions = BuildOptions.CompressWithLz4;

            string location = outputPath + "/HybridCLRTrial.apk";

            PrebuildCommand.GenerateAll();

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions()
            {
                scenes = new string[] { "Assets/Scenes/main.unity" },
                locationPathName = location,
                options = buildOptions,
                target = target,
                targetGroup = BuildTargetGroup.Android,
            };

            Debug.Log("====> 第1次 Build App(为了生成补充AOT元数据dll)");
            BuildPipeline.BuildPlayer(buildPlayerOptions);
            Debug.Log("====> Build AssetBundle");
            AssetBundleBuildCommand.BuildAssetBundleByTarget(target, true);

            Debug.Log("====> 第2次打包");
            BuildPipeline.BuildPlayer(buildPlayerOptions);
#if UNITY_EDITOR
            Application.OpenURL($"file:///{outputPath}");
#endif
        }
    }
}
