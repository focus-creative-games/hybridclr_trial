using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        ///// <summary>
        /////
        ///// </summary>
        //[MenuItem("HuaTuo/Build/BuildDLLAssetBundle", false, 1)]
        //public static void BuildDLLAssetBundle()
        //{
        //    string _dllPath = Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll";
        //    string _tarDir = Application.dataPath + "/HuaTuo/Temp/";
        //    string _tarPath = $"{_tarDir}HotFix.bytes";
        //    string _outPutPath = Application.dataPath + "/HuaTuo/Output/";

        //    if (!Directory.Exists(_tarDir))
        //    {
        //        Directory.CreateDirectory(_tarDir);
        //    }

        //    if (!Directory.Exists(_outPutPath))
        //    {
        //        Directory.CreateDirectory(_outPutPath);
        //    }

        //    if (File.Exists(_tarPath))
        //    {
        //        File.Delete(_tarPath);
        //    }

        //    File.Copy(_dllPath, _tarPath, true);

        //    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

        //    string _assetPath = _tarPath.Substring(_tarPath.IndexOf("Assets/", StringComparison.Ordinal));

        //    List<AssetBundleBuild> _list = new List<AssetBundleBuild>();
        //    AssetBundleBuild _ab = new AssetBundleBuild
        //    {
        //        assetBundleName = "huatuo",
        //        assetNames = new[] { _assetPath }
        //    };
        //    _list.Add(_ab);

        //    BuildPipeline.BuildAssetBundles(_outPutPath, _list.ToArray(), BuildAssetBundleOptions.None,
        //        EditorUserBuildSettings.activeBuildTarget);

        //    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

        //    AssetDatabase.CopyAsset("Assets/HuaTuo/Output/huatuo", "Assets/StreamingAssets/huatuo");
        //}

        public static string ToReleateAssetPath(string s)
        {
            return s.Substring(s.IndexOf("Assets/"));
        }

        [MenuItem("HuaTuo/BuildBundles", false, 1)]
        public static void BuildSeneAssetBundle()
        {
            string tempDir = Application.dataPath + "/HuaTuo/Temp";
            string outPutDir = Application.dataPath + "/HuaTuo/Output";

            string dllPath = Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll";
            string dllBytesPath = $"{tempDir}/HotFix.dll.bytes";
            string testPrefab = $"{Application.dataPath}/Prefabs/HotUpdatePrefab.prefab";
            string testScene = $"{Application.dataPath}/Scenes/HotUpdateScene.unity";

            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            if (!Directory.Exists(outPutDir))
            {
                Directory.CreateDirectory(outPutDir);
            }


            File.Copy(dllPath, dllBytesPath, true);

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            string[] notSceneAssets =
            {
                dllBytesPath,
                testPrefab,
            };

            List<AssetBundleBuild> abs = new List<AssetBundleBuild>();
            AssetBundleBuild notSceneAb = new AssetBundleBuild
            {
                assetBundleName = "common",
                assetNames = notSceneAssets.Select(s => ToReleateAssetPath(s)).ToArray(),
            };
            abs.Add(notSceneAb);

            string[] sceneAssets =
            {
                testScene,
            };
            AssetBundleBuild sceneAb = new AssetBundleBuild
            {
                assetBundleName = "scene",
                assetNames = sceneAssets.Select(s => s.Substring(s.IndexOf("Assets/"))).ToArray(),
            };

            abs.Add(sceneAb);

            BuildPipeline.BuildAssetBundles(outPutDir,
                abs.ToArray(),
                BuildAssetBundleOptions.None,
                EditorUserBuildSettings.activeBuildTarget);

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            foreach(var ab in abs)
            {
                AssetDatabase.CopyAsset(ToReleateAssetPath($"{outPutDir}/{ab.assetBundleName}"),
                    ToReleateAssetPath($"{Application.streamingAssetsPath}/{ab.assetBundleName}"));
            }
        }
    }
}