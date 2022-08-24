using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.Linq;
using MonoHook;
using System.IO;

namespace HybridCLR.Editor.BuildProcessors
{
    // 有需求时可以打开，也可以手动按需注册Hook
    [InitializeOnLoad]
    public class HookEditorStripAOTAction
    {
        /// <summary>
        /// 裁剪执行完毕的回调，可能会被调用多次，一般而言同一次打包只需要处理第一次回调
        /// </summary>
        //private static event Action<string, BuildPostProcessArgs, BeeDriverResult> OnAssemblyStripped;

        public static event Action<string, BuildTarget> OnAssembliyScripped2;

        // 尝试 Hook 4个函数，至少一个被调用就可以达到要求
        private static MethodHook _hook_PostprocessBuildPlayer_CompleteBuild;
        private static MethodHook _hook_Default_PostProcess;
        private static MethodHook _hook_ReportBuildResults;
        private static MethodHook _hook_StripAssembliesTo;

#region Fake Internal Structures
        public struct BuildPostProcessArgs
        {
            public BuildTarget target;
            public int subTarget;
            public string stagingArea;
            public string stagingAreaData;
            public string stagingAreaDataManaged;
            public string playerPackage;
            public string installPath;
            public string companyName;
            public string productName;
            public Guid productGUID;
            public BuildOptions options;
            public UnityEditor.Build.Reporting.BuildReport report;
            internal /*RuntimeClassRegistry*/object usedClassRegistry;
        }

        public sealed class BeeDriverResult
        {
            public /*NodeResult*/object[] NodeResults { get; set; }
            public bool Success { get; set; }
            public /*Message*/object[] BeeDriverMessages { get; set; }
            public override string ToString() => Success.ToString();
        }
        #endregion

        public static string GetStripAssembliesDir_2021_6_OR_OLD(BuildTarget target)
        {
            string projectDir = BuildConfig.ProjectDir;
#if UNITY_STANDALONE_WIN
            return $"{projectDir}/Library/Bee/artifacts/WinPlayerBuildProgram/ManagedStripped";
#elif UNITY_ANDROID
            return $"{projectDir}/Library/Bee/artifacts/Android/ManagedStripped";
//#elif UNITY_IOS
//            return $"{projectDir}/Temp/StagingArea/Data/Managed";
#elif UNITY_WEBGL
            return $"{projectDir}/Library/Bee/artifacts/WebGL/ManagedStripped";
#else
            throw new NotSupportedException("GetOriginBuildStripAssembliesDir");
#endif
        }

        static string GetStripAssembliesOutputDir(BuildTarget target, string outputFolder, BuildPostProcessArgs? args, BeeDriverResult result)
        {
            if (!string.IsNullOrEmpty(outputFolder))
            {
                return outputFolder;
            }
            if (args != null)
            {
                return args.Value.stagingAreaDataManaged;
            }
            if (result != null)
            {
                return GetStripAssembliesDir_2021_6_OR_OLD(target);// result.
            }
            throw new Exception($"unknown stripped AOT assemblies dir");
        }

        /// <summary>
        /// 示例裁剪回调函数
        /// </summary>
        /// <param name="outputFolder"></param>
        /// <param name="args"></param>
        /// <param name="result"></param>
        static void OnAssemblyStripped(string outputFolder, BuildPostProcessArgs? args, BeeDriverResult result)
        {
            if (result != null && !result.Success)
            {
                return;
            }
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            string strippedAssemblyDir = GetStripAssembliesOutputDir(target, outputFolder, args, result);
            Debug.Log($"[HookEditorStripAOTAction] strippedAssemblyDir:{strippedAssemblyDir}");
            OnAssembliyScripped2?.Invoke(strippedAssemblyDir, target);
        }

        public static void InstallHook()
        {
            do
            {
                Type type = Type.GetType("UnityEditor.PostprocessBuildPlayer,UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
                if (type == null)
                {
                    Debug.LogError($"can not find type: UnityEditor.PostprocessBuildPlayer");
                    break;
                }

                MethodInfo miTarget = type.GetMethod("PostProcessCompletedBuild", BindingFlags.Static | BindingFlags.Public);

                if (miTarget == null)
                {
                    Debug.LogError($"can not find method: UnityEditor.PostprocessBuildPlayer.PostProcessCompletedBuild");
                    break;
                }

                MethodInfo miReplace = typeof(HookEditorStripAOTAction).GetMethod(nameof(PostprocessBuildPlayer_CompleteBuild_Replace), BindingFlags.Static | BindingFlags.NonPublic);
                MethodInfo miProxy = typeof(HookEditorStripAOTAction).GetMethod(nameof(PostprocessBuildPlayer_CompleteBuild_Proxy), BindingFlags.Static | BindingFlags.NonPublic);

                _hook_PostprocessBuildPlayer_CompleteBuild = new MethodHook(miTarget, miReplace, miProxy);
                _hook_PostprocessBuildPlayer_CompleteBuild.Install();

                Debug.Log("Hook BuildPipeline_StripDll_HookTest.PostprocessBuildPlayer_CompleteBuild installed");
            } while (false);

            do
            {
                Type type = Type.GetType("UnityEditor.Modules.DefaultBuildPostprocessor,UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
                if (type == null)
                {
                    Debug.LogError($"can not find type: UnityEditor.Modules.DefaultBuildPostprocessor");
                    break;
                }

                MethodInfo[] miTargets = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                MethodInfo miTarget = (from mi in miTargets where mi.Name == "PostProcess" && mi.GetParameters().Length == 2 select mi).FirstOrDefault();

                if (miTarget == null)
                {
                    Debug.LogError($"can not find method: UnityEditor.Modules.DefaultBuildPostprocessor.PostProcess");
                    break;
                }

                MethodInfo miReplace = typeof(HookEditorStripAOTAction).GetMethod(nameof(Default_PostProcess_Replace), BindingFlags.Static | BindingFlags.NonPublic);
                MethodInfo miProxy = typeof(HookEditorStripAOTAction).GetMethod(nameof(Default_PostProcess_Proxy), BindingFlags.Static | BindingFlags.NonPublic);

                _hook_Default_PostProcess = new MethodHook(miTarget, miReplace, miProxy);
                _hook_Default_PostProcess.Install();

                Debug.Log("Hook BuildPipeline_StripDll_HookTest.PostProcess installed");
            } while (false);

            do
            {
                Type type = Type.GetType("UnityEditor.Modules.BeeBuildPostprocessor,UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
                if (type == null)
                {
                    Debug.LogError($"can not find type: UnityEditor.Modules.BeeBuildPostprocessor");
                    break;
                }

                MethodInfo miTarget = type.GetMethod("ReportBuildResults", BindingFlags.Instance | BindingFlags.NonPublic);
                if (miTarget == null)
                {
                    Debug.LogError($"can not find method: UnityEditor.Modules.BeeBuildPostprocessor.ReportBuildResults");
                    break;
                }

                MethodInfo miReplace = typeof(HookEditorStripAOTAction).GetMethod(nameof(ReportBuildResults_Replace), BindingFlags.Static | BindingFlags.NonPublic);
                MethodInfo miProxy = typeof(HookEditorStripAOTAction).GetMethod(nameof(ReportBuildResults_Proxy), BindingFlags.Static | BindingFlags.NonPublic);

                _hook_ReportBuildResults = new MethodHook(miTarget, miReplace, miProxy);
                _hook_ReportBuildResults.Install();

                Debug.Log("Hook BuildPipeline_StripDll_HookTest.ReportBuildResults installed");
            } while (false);

            do
            {
                Type type = Type.GetType("UnityEditorInternal.AssemblyStripper,UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
                if (type == null)
                {
                    Debug.LogError($"can not find type: UnityEditorInternal.AssemblyStripper");
                    break;
                }

                MethodInfo miTarget = type.GetMethod("StripAssembliesTo", BindingFlags.Static | BindingFlags.NonPublic);
                if (miTarget == null)
                {
                    Debug.LogError($"can not find method: UnityEditorInternal.AssemblyStripper.StripAssembliesTo");
                    break;
                }

                MethodInfo miReplace = typeof(HookEditorStripAOTAction).GetMethod(nameof(StripAssembliesTo_Replace), BindingFlags.Static | BindingFlags.NonPublic);
                MethodInfo miProxy = typeof(HookEditorStripAOTAction).GetMethod(nameof(StripAssembliesTo_Proxy), BindingFlags.Static | BindingFlags.NonPublic);

                _hook_StripAssembliesTo = new MethodHook(miTarget, miReplace, miProxy);
                _hook_StripAssembliesTo.Install();

                Debug.Log("Hook BuildPipeline_StripDll_HookTest.StripAssembliesTo installed");
            } while (false);
        }

        public static void UninstallHook()
        {
            _hook_PostprocessBuildPlayer_CompleteBuild?.Uninstall();
            _hook_Default_PostProcess?.Uninstall();
            _hook_ReportBuildResults?.Uninstall();
            _hook_StripAssembliesTo?.Uninstall();
        }

        static void PostprocessBuildPlayer_CompleteBuild_Replace(BuildPostProcessArgs args)
        {
            Debug.Log("PostprocessBuildPlayer_CompleteBuild_Replace called");
//#if !UNITY_IOS
//            OnAssemblyStripped(null, args, null);
//#endif
            PostprocessBuildPlayer_CompleteBuild_Proxy(args);
        }

        static void Default_PostProcess_Replace(object obj, BuildPostProcessArgs args, out /*BuildProperties*/ object outProperties)
        {
            try
            {
                // 注意：此函数中途可能会被 Unity throw Exception
                Default_PostProcess_Proxy(obj, args, out outProperties);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                Debug.Log("PostProcess_Replace called");
//#if !UNITY_IOS
//                OnAssemblyStripped(null, args, null);
//#endif
            }
        }

        static void ReportBuildResults_Replace(object obj, BeeDriverResult result)
        {
            // TODO: 可以在这里把 Library\Bee\artifacts\WinPlayerBuildProgram\ManagedStripped 目录下的文件复制出来
            Debug.Log("ReportBuildResults_Replace called");

            OnAssemblyStripped(null, null, result);
            ReportBuildResults_Proxy(obj, result);
        }

        static bool StripAssembliesTo_Replace(string outputFolder, out string output, out string error, IEnumerable<string> linkXmlFiles, /*UnityLinkerRunInformation*/ object runInformation)
        {
            bool ret = StripAssembliesTo_Proxy(outputFolder, out output, out error, linkXmlFiles, runInformation);

            // TODO: 可以在这里把 Temp\StagingArea\Data\Managed\tempStrip 目录下的文件复制出来
            Debug.Log("StripAssembliesTo_Replace called");

            OnAssemblyStripped(outputFolder, null, null);
            return ret;
        }

#region Proxy Methods
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void PostprocessBuildPlayer_CompleteBuild_Proxy(BuildPostProcessArgs args)
        {
            Debug.Log("dummy code" + 200);
            Debug.Log(args.companyName);
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Default_PostProcess_Proxy(object obj, BuildPostProcessArgs args, out /*BuildProperties*/ object outProperties)
        {
            Debug.Log("dummy code" + 100);
            outProperties = null;
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ReportBuildResults_Proxy(object obj, /*BeeDriverResult*/ object result)
        {
            // dummy code
            Debug.Log("something" + obj.ToString() + result.ToString() + 2);
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        static bool StripAssembliesTo_Proxy(string outputFolder, out string output, out string error, IEnumerable<string> linkXmlFiles, /*UnityLinkerRunInformation*/ object runInformation)
        {
            Debug.Log("StripAssembliesTo_Proxy called");
            output = null;
            error = null;
            return true;
        }
#endregion
    }
}
