using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using FileMode = System.IO.FileMode;

namespace HuaTuo
{
    /// <summary>
    /// 这里仅仅是一个流程展示
    /// 简单说明如果你想将huatuo的dll做成自动化的简单实现
    /// </summary>
    public class HuaTuoEditorHelper
    {
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            //string hotfixDll = Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll";
            //string target1 = Application.streamingAssetsPath + "/HotFix.dll";
            //File.Copy(hotfixDll, target1, true); 
            //string target2 = Application.dataPath + "/../build-win64/build/bin/HuatuoTest_Data/StreamingAssets/HotFix.dll";
            //File.Copy(hotfixDll, target2, true);
            //Debug.Log("copy hotfix.dll finish");
            UnityEngine.Debug.Log("compile succ");
        }
        
        /// <summary>
        ///
        /// </summary>
        [MenuItem("HuaTuo/Build/BuildDLLAssetBundle", false, 1)]
        public static void BuildDLLAssetBundle()
        {
            string _dllPath = Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll";
            string _tarDir = Application.dataPath + "/HuaTuo/Temp/";
            string _tarPath = $"{_tarDir}HotFix.bytes";
            string _outPutPath = Application.dataPath + "/HuaTuo/Output/";

            if (!Directory.Exists(_tarDir))
            {
                Directory.CreateDirectory(_tarDir);
            }

            if (!Directory.Exists(_outPutPath))
            {
                Directory.CreateDirectory(_outPutPath);
            }

            if (File.Exists(_tarPath))
            {
                File.Delete(_tarPath);
            }

            File.Copy(_dllPath, _tarPath, true);

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            string _assetPath = _tarPath.Substring(_tarPath.IndexOf("Assets/", StringComparison.Ordinal));

            List<AssetBundleBuild> _list = new List<AssetBundleBuild>();
            AssetBundleBuild _ab = new AssetBundleBuild
            {
                assetBundleName = "huatuo",
                assetNames = new[] { _assetPath }
            };
            _list.Add(_ab);

            BuildPipeline.BuildAssetBundles(_outPutPath, _list.ToArray(), BuildAssetBundleOptions.None,
                EditorUserBuildSettings.activeBuildTarget);

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            AssetDatabase.CopyAsset("Assets/HuaTuo/Output/huatuo", "Assets/StreamingAssets/huatuo");
        }

        [MenuItem("HuaTuo/Build/BuildSceneBundle", false, 1)]
        public static void BuildSeneAssetBundle()
        {
            string _dllPath = Application.dataPath + "/Scenes/HotUpdate.unity";
            string _tarDir = Application.dataPath + "/HuaTuo/Temp/";
            string _tarPath = $"{_tarDir}HotUpdate.bytes";
            string _outPutPath = Application.dataPath + "/HuaTuo/Output/";

            if (!Directory.Exists(_tarDir))
            {
                Directory.CreateDirectory(_tarDir);
            }

            if (!Directory.Exists(_outPutPath))
            {
                Directory.CreateDirectory(_outPutPath);
            }

            if (File.Exists(_tarPath))
            {
                File.Delete(_tarPath);
            }

            File.Copy(_dllPath, _tarPath, true);

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            string _assetPath = _tarPath.Substring(_tarPath.IndexOf("Assets/", StringComparison.Ordinal));

            List<AssetBundleBuild> _list = new List<AssetBundleBuild>();
            AssetBundleBuild _ab = new AssetBundleBuild
            {
                assetBundleName = "huatuo",
                assetNames = new[] { _assetPath }
            };
            _list.Add(_ab);

            BuildPipeline.BuildAssetBundles(_outPutPath, _list.ToArray(), BuildAssetBundleOptions.None,
                EditorUserBuildSettings.activeBuildTarget);

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            AssetDatabase.CopyAsset("Assets/HuaTuo/Output/huatuo", "Assets/StreamingAssets/huatuo");
        }
    }
}