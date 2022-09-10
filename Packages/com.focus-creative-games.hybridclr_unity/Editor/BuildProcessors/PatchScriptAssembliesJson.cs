using HybridCLR.Editor.UnityBinFileReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Android;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Il2Cpp;
using UnityEditor.UnityLinker;
using UnityEngine;
using UnityFS;

namespace HybridCLR.Editor.BuildProcessors
{
    public class PatchScriptingAssemblyList :
#if UNITY_ANDROID
        IPostGenerateGradleAndroidProject,
#endif
        IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;




        public void OnPostGenerateGradleAndroidProject(string path)
        {
            // 如果直接打包apk，没有机会在PostprocessBuild中修改ScriptingAssemblies.json。
            // 因此需要在这个时机处理
            PathScriptingAssembilesFile(path);
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            // 如果target为Android,由于已经在OnPostGenerateGradelAndroidProject中处理过，
            // 这里不再重复处理
#if !UNITY_ANDROID

            PathScriptingAssembilesFile(report.summary.outputPath);
#endif
        }

        private void PathScriptingAssembilesFile(string path)
        {
            Debug.Log($"PathScriptingAssembilesFile. path:{path}");
            // File.Exists及Directory.Exist在Mac有下bug，所以使用这种办法
            var file = new FileInfo(path);
            if (path.EndsWith(".exe") || path.EndsWith(".apk") || !file.Exists || !file.Attributes.HasFlag(FileAttributes.Directory))
            {
                path = Path.GetDirectoryName(path);
            }
#if UNITY_2020_1_OR_NEWER
            AddHotFixAssembliesToScriptingAssembliesJson(path);
#else
            AddHotFixAssembliesToBinFile(path);
#endif
        }

        private void AddHotFixAssembliesToScriptingAssembliesJson(string path)
        {
            Debug.Log($"AddHotFixAssembliesToJson. path:{path}");
            /*
             * ScriptingAssemblies.json 文件中记录了所有的dll名称，此列表在游戏启动时自动加载，
             * 不在此列表中的dll在资源反序列化时无法被找到其类型
             * 因此 OnFilterAssemblies 中移除的条目需要再加回来
             */
            string[] jsonFiles = Directory.GetFiles(path, SettingsUtil.ScriptingAssembliesJsonFile, SearchOption.AllDirectories);

            if (jsonFiles.Length == 0)
            {
                Debug.LogError($"can not find file {SettingsUtil.ScriptingAssembliesJsonFile}");
                return;
            }

            foreach (string file in jsonFiles)
            {
                var patcher = new ScriptingAssembliesJsonPatcher();
                patcher.Load(file);
                patcher.AddScriptingAssemblies(SettingsUtil.HotUpdateAssemblies);
                patcher.Save(file);
            }
        }

        private void AddHotFixAssembliesToBinFile(string path)
        {
#if UNITY_ANDROID
            AddHotFixAssembliesTodataunity3d(path);
#else
            AddHotFixAssembliesToGlobalgamemanagers(path);
#endif
        }

        private void AddHotFixAssembliesToGlobalgamemanagers(string path)
        {
            string[] binFiles = Directory.GetFiles(Path.GetDirectoryName(path), SettingsUtil.GlobalgamemanagersBinFile, SearchOption.AllDirectories);

            if (binFiles.Length == 0)
            {
                Debug.LogError($"can not find file {SettingsUtil.GlobalgamemanagersBinFile}");
                return;
            }

            foreach (string binPath in binFiles)
            {
                var binFile = new UnityBinFile();
                binFile.Load(binPath);
                binFile.AddScriptingAssemblies(SettingsUtil.HotUpdateAssemblies);
                binFile.Save(binPath);
                Debug.Log($"[PatchScriptAssembliesJson] patch {binPath}");
            }
        }

        private void AddHotFixAssembliesTodataunity3d(string path)
        {
            string[] binFiles = Directory.GetFiles(Path.GetDirectoryName(path), SettingsUtil.Dataunity3dBinFile, SearchOption.AllDirectories);

            if (binFiles.Length == 0)
            {
                Debug.LogError($"can not find file {SettingsUtil.Dataunity3dBinFile}");
                return;
            }

            foreach (string binPath in binFiles)
            {
                var patcher = new Dataunity3dPatcher();
                patcher.ApplyPatch(binPath, SettingsUtil.HotUpdateAssemblies);
                Debug.Log($"[PatchScriptAssembliesJson] patch {binPath}");
            }
        }
    }
}
