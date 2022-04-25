#if UNITY_2020_1_OR_NEWER && !UNITY_2021_3_OR_NEWER
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

namespace HuaTuo
{
    public class HuaTuo_BuildProcessor_2020_1_OR_NEWER : IPreprocessBuildWithReport, IPostprocessBuildWithReport
#if UNITY_ANDROID
        , IPostGenerateGradleAndroidProject
#endif
        , IProcessSceneWithReport, IFilterBuildAssemblies, IPostBuildPlayerScriptDLLs, IUnityLinkerProcessor
    {
        /// <summary>
        /// 需要在Prefab上挂脚本的热更dll名称列表，不需要挂到Prefab上的脚本可以不放在这里
        /// 但放在这里的dll即使勾选了 AnyPlatform 也会在打包过程中被排除
        /// 
        /// 另外请务必注意！： 需要挂脚本的dll的名字最好别改，因为这个列表无法热更（上线后删除或添加某些非挂脚本dll没问题）
        /// </summary>
        static List<string> monoDllNames = new List<string>() { "UnitTest.dll"};

        int IOrderedCallback.callbackOrder => 0;

        void IPreprocessBuildWithReport.OnPreprocessBuild(BuildReport report)
        {
            //if (!Application.isBatchMode && !EditorUtility.DisplayDialog("确认", "建议 Build 之前先打包 AssetBundle\r\n是否继续?", "继续", "取消"))
            //{
            //    s_BuildReport_AddMessage.Invoke(report, new object[] { LogType.Exception, "用户取消", "BuildFailedException" });
            //    return;
            //}
        }

        string[] IFilterBuildAssemblies.OnFilterAssemblies(BuildOptions buildOptions, string[] assemblies)
        {
            // 将热更dll从打包列表中移除
            List<string> newNames = new List<string>(assemblies.Length);

            foreach(string assembly in assemblies)
            {
                bool found = false;
                foreach(string removeName in monoDllNames)
                {
                    if(assembly.EndsWith(removeName, StringComparison.OrdinalIgnoreCase))
                    {
                        found = true;
                        break;
                    }
                }

                if(!found)
                    newNames.Add(assembly);
            }
            
            return newNames.ToArray();
        }


        [Serializable]
        public class ScriptingAssemblies
        {
            public List<string> names;
            public List<int> types;
        }

        void IPostprocessBuildWithReport.OnPostprocessBuild(BuildReport report)
        {
            AddBackHotFixAssembliesToJson(report, null);
        }

#if UNITY_ANDROID
        void IPostGenerateGradleAndroidProject.OnPostGenerateGradleAndroidProject(string path)
        {
            // 由于 Android 平台在 OnPostprocessBuild 调用时已经生成完 apk 文件，因此需要提前调用
            AddBackHotFixAssembliesToJson(null, path);
        }
#endif

        private void AddBackHotFixAssembliesToJson(BuildReport report, string path)
        {
            /*
             * ScriptingAssemblies.json 文件中记录了所有的dll名称，此列表在游戏启动时自动加载，
             * 不在此列表中的dll在资源反序列化时无法被找到其类型
             * 因此 OnFilterAssemblies 中移除的条目需要再加回来
             */
#if UNITY_ANDROID
            string[] jsonFiles = new string[] { "Temp/gradleOut/unityLibrary/src/main/assets/bin/Data/ScriptingAssemblies.json" }; // report.files 不包含 Temp/gradleOut 等目录
#else
            // 直接出包和输出vs工程时路径不同，report.summary.outputPath 记录的是前者路径
            string[] jsonFiles = Directory.GetFiles(Path.GetDirectoryName(report.summary.outputPath), "ScriptingAssemblies.json", SearchOption.AllDirectories);
#endif

            if (jsonFiles.Length == 0)
            {
                Debug.LogError("can not find file ScriptingAssemblies.json");
                return;
            }

            foreach (string file in jsonFiles)
            {
                string content = File.ReadAllText(file);
                ScriptingAssemblies scriptingAssemblies = JsonUtility.FromJson<ScriptingAssemblies>(content);
                foreach (string name in monoDllNames)
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


        void IProcessSceneWithReport.OnProcessScene(Scene scene, BuildReport report)
        {

        }

        void IPostBuildPlayerScriptDLLs.OnPostBuildPlayerScriptDLLs(BuildReport report)
        {

        }

        string IUnityLinkerProcessor.GenerateAdditionalLinkXmlFile(BuildReport report, UnityLinkerBuildPipelineData data)
        {
            return String.Empty;
        }

        void IUnityLinkerProcessor.OnBeforeRun(BuildReport report, UnityLinkerBuildPipelineData data)
        {
        }

        void IUnityLinkerProcessor.OnAfterRun(BuildReport report, UnityLinkerBuildPipelineData data)
        {
        }


#if UNITY_IOS
    // hook UnityEditor.BuildCompletionEventsHandler.ReportPostBuildCompletionInfo() ? 因为没有 mac 打包平台因此不清楚
#endif
    }

}
#endif