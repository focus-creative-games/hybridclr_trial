using HybridCLR.Editor.GlobalManagers;
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

namespace HybridCLR.Editor.BuildProcessors
{
    public class BPPatchScriptAssembliesJson : IPreprocessBuildWithReport,
#if UNITY_ANDROID
        IPostGenerateGradleAndroidProject,
#endif
        IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;


        [Serializable]
        private class ScriptingAssemblies
        {
            public List<string> names;
            public List<int> types;
        }

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
            AddBackHotFixAssembliesToBinFile(path);
#endif
        }

        private void AddHotFixAssembliesToScriptingAssembliesJson(string path)
        {
            Debug.Log($"AddBackHotFixAssembliesToJson. path:{path}");
            /*
             * ScriptingAssemblies.json 文件中记录了所有的dll名称，此列表在游戏启动时自动加载，
             * 不在此列表中的dll在资源反序列化时无法被找到其类型
             * 因此 OnFilterAssemblies 中移除的条目需要再加回来
             */
            string[] jsonFiles = Directory.GetFiles(path, BuildConfig.ScriptingAssembliesJsonFile, SearchOption.AllDirectories);

            if (jsonFiles.Length == 0)
            {
                Debug.LogError($"can not find file {BuildConfig.ScriptingAssembliesJsonFile}");
                return;
            }

            foreach (string file in jsonFiles)
            {
                string content = File.ReadAllText(file);
                ScriptingAssemblies scriptingAssemblies = JsonUtility.FromJson<ScriptingAssemblies>(content);
                foreach (string name in BuildConfig.HotUpdateAssemblies)
                {
                    if (!scriptingAssemblies.names.Contains(name))
                    {
                        scriptingAssemblies.names.Add(name);
                        scriptingAssemblies.types.Add(16); // user dll type
                        Debug.Log($"[PatchScriptAssembliesJson] add hotfix assembly:{name} to {file}");
                    }
                }
                content = JsonUtility.ToJson(scriptingAssemblies);

                File.WriteAllText(file, content);
            }
        }

        private void AddBackHotFixAssembliesToBinFile(string path)
        {
#if UNITY_ANDROID
            AddBackHotFixAssembliesTodataunity3d(path);
#else
            AddBackHotFixAssembliesToGlobalgamemanagers(path);
#endif
        }

        private void AddBackHotFixAssembliesToGlobalgamemanagers(string path)
        {
            string[] binFiles = Directory.GetFiles(Path.GetDirectoryName(path), "globalgamemanagers", SearchOption.AllDirectories);

            if (binFiles.Length == 0)
            {
                Debug.LogError($"can not find file {BuildConfig.GlobalgamemanagersBinFile}");
                return;
            }

            foreach (string binPath in binFiles)
            {
                var binFile = new UnityBinFile();
                binFile.LoadFromFile(binPath);
                binFile.AddScriptingAssemblies(BuildConfig.HotUpdateAssemblies);
                binFile.RebuildAndFlushToFile(binPath);
                Debug.Log($"[PatchScriptAssembliesJson] patch {binPath}");
            }
        }

        private void AddBackHotFixAssembliesTodataunity3d(string path)
        {
            string[] binFiles = Directory.GetFiles(Path.GetDirectoryName(path), "data.unity3d", SearchOption.AllDirectories);

            if (binFiles.Length == 0)
            {
                Debug.LogError("can not find file data.unity3d");
                return;
            }

            foreach (string binPath in binFiles)
            {
                var patcher = new Dataunity3dPatcher();
                patcher.ApplyPatch(binPath);
                Debug.Log($"[PatchScriptAssembliesJson] patch {binPath}");
            }
        }

#region useless

        public void OnPreprocessBuild(BuildReport report)
        {

        }

#endregion
    }
}
