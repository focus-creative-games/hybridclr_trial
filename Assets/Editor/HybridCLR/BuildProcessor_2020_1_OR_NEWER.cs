#if UNITY_2020_1_OR_NEWER
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;
using System;
using UnityEditor.UnityLinker;
using System.Reflection;
using UnityEditor.Il2Cpp;
#if UNITY_ANDROID
using UnityEditor.Android;
#endif

namespace HybridCLR
{
    public class BuildProcessor_2020_1_OR_NEWER : IPreprocessBuildWithReport,
#if UNITY_ANDROID
        IPostGenerateGradleAndroidProject,
#endif
        IPostprocessBuildWithReport,
        IFilterBuildAssemblies,
        IUnityLinkerProcessor
#if !UNITY_2021_1_OR_NEWER
    , IIl2CppProcessor
#endif
    {
        public int callbackOrder => 0;

        public string[] OnFilterAssemblies(BuildOptions buildOptions, string[] assemblies)
        {
            // 将热更dll从打包列表中移除
            return assemblies.Where(ass => BuildConfig.AllHotUpdateDllNames.All(dll => !ass.EndsWith(dll, StringComparison.OrdinalIgnoreCase))).ToArray();
        }


        [Serializable]
        private class ScriptingAssemblies
        {
            public List<string> names;
            public List<int> types;
        }

        public void OnBeforeConvertRun(BuildReport report, Il2CppBuildPipelineData data)
        {
            Debug.Log("==== OnBeforeConvertRun");
            // 此回调只在 2020中调用
            CopyStripDlls(data.target);
        }

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            Debug.Log("==== OnPostGenerateGradleAndroidProject");
            // 由于 Android 平台在 OnPostprocessBuild 调用时已经生成完 apk 文件，因此需要提前调用
            AddBackHotFixAssembliesToJson(null, path);

            // 对于 2020, 已经在OnBeforeConvertRun中复制了，因此这儿不再复制
//#if UNITY_2021_1_OR_NEWER
//            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
//            CopyStripDlls(target);
//#endif
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            Debug.Log("==== OnPostprocessBuild");
#if !UNITY_ANDROID

            AddBackHotFixAssembliesToJson(report, report.summary.outputPath);
#endif
        // Unity 2021开始，不再清理这个Temp目录，因此在这个时机复制较为合适
#if UNITY_2021_1_OR_NEWER
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            CopyStripDlls(target);
#endif
        }
        
        private void AddBackHotFixAssembliesToJson(BuildReport report, string path)
        {
            /*
             * ScriptingAssemblies.json 文件中记录了所有的dll名称，此列表在游戏启动时自动加载，
             * 不在此列表中的dll在资源反序列化时无法被找到其类型
             * 因此 OnFilterAssemblies 中移除的条目需要再加回来
             */
            string[] jsonFiles = Directory.GetFiles(Path.GetDirectoryName(path), BuildConfig.ScriptingAssembliesJsonFile, SearchOption.AllDirectories);

            if (jsonFiles.Length == 0)
            {
                Debug.LogError($"can not find file {BuildConfig.ScriptingAssembliesJsonFile}");
                return;
            }

            foreach (string file in jsonFiles)
            {
                string content = File.ReadAllText(file);
                ScriptingAssemblies scriptingAssemblies = JsonUtility.FromJson<ScriptingAssemblies>(content);
                foreach (string name in BuildConfig.MonoHotUpdateDllNames)
                {
                    if(!scriptingAssemblies.names.Contains(name))
                    {
                        scriptingAssemblies.names.Add(name);
                        scriptingAssemblies.types.Add(16); // user dll type
                    }
                }
                content = JsonUtility.ToJson(scriptingAssemblies);

                File.WriteAllText(file, content);
            }
        }

        private void CopyStripDlls(BuildTarget target)
        {
            var dstPath = BuildConfig.GetAssembliesPostIl2CppStripDir(target);

            Directory.CreateDirectory(dstPath);

            string srcStripDllPath = BuildConfig.GetOriginBuildStripAssembliesDir(target);

            foreach (var fileFullPath in Directory.GetFiles(srcStripDllPath, "*.dll"))
            {
                var file = Path.GetFileName(fileFullPath);
                Debug.Log($"copy strip dll {fileFullPath} ==> {dstPath}/{file}");
                File.Copy($"{fileFullPath}", $"{dstPath}/{file}", true);
            }
        }

        #region useless

        private static void BuildExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
        {

        }

        public void OnPreprocessBuild(BuildReport report)
        {

        }

        public string GenerateAdditionalLinkXmlFile(BuildReport report, UnityLinkerBuildPipelineData data)
        {
            return String.Empty;
        }

        public void OnBeforeRun(BuildReport report, UnityLinkerBuildPipelineData data)
        {

        }

        public void OnAfterRun(BuildReport report, UnityLinkerBuildPipelineData data)
        {
        }


#if UNITY_IOS
    // hook UnityEditor.BuildCompletionEventsHandler.ReportPostBuildCompletionInfo() ? 因为没有 mac 打包平台因此不清楚
#endif
        #endregion
    }

}
#endif