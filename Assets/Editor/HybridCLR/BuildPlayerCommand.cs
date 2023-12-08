using HybridCLR.Editor.Commands;
using HybridCLR.Editor.Installer;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HybridCLR.Editor
{
    public class BuildPlayerCommand
    {
        public static void CopyAssets(string outputDir)
        {
            Directory.CreateDirectory(outputDir);

            foreach(var srcFile in Directory.GetFiles(Application.streamingAssetsPath))
            {
                string dstFile = $"{outputDir}/{Path.GetFileName(srcFile)}";
                File.Copy(srcFile, dstFile, true);
            }
        }

        public static void InstallFromRepo()
        {
            var ic = new InstallerController();
            ic.InstallDefaultHybridCLR();
        }

        public static void InstallBuildWin64()
        {
            InstallFromRepo();
            Build_Win64(true);
        }

        [MenuItem("Build/Win64")]
        public static void Build_Win64()
        {
            Build_Win64(false);
        }

        public static void Build_Win64(bool existWhenCompleted)
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
                if (existWhenCompleted)
                {
                    EditorApplication.Exit(1);
                }
                return;
            }

            Debug.Log("====> 复制热更新资源和代码");
            BuildAssetsCommand.BuildAndCopyABAOTHotUpdateDlls();
            BashUtil.CopyDir(Application.streamingAssetsPath, $"{outputPath}/HybridCLRTrial_Data/StreamingAssets", true);
        }
    }
}
