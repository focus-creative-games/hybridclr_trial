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

            FileStreamCopy(_dllPath, _tarPath);

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

        public static void FileStreamCopy(string _src, string _tar)
        {
            using (FileStream _read = new FileStream(_src, FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (FileStream _write = new FileStream(_tar, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    byte[] _buffer = new byte[1024 * 1024 * 2];
                    int _count = 0;

                    while ((_count = _read.Read(_buffer, 0, _buffer.Length)) > 0)
                    {
                        _write.Write(_buffer, 0, _count);
                    }
                }
            }
        }
    }
}