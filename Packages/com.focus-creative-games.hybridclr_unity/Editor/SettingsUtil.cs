using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace HybridCLR.Editor
{
    public static class SettingsUtil
    {
        public static bool Enable => GlobalSettings.enable;

        public static string ProjectDir => Directory.GetParent(Application.dataPath).ToString();

        public static string ScriptingAssembliesJsonFile { get; } = "ScriptingAssemblies.json";

        public static string GlobalgamemanagersBinFile { get; } = "globalgamemanagers";

        public static string Dataunity3dBinFile { get; } = "data.unity3d";

        public static string HotFixDllsOutputDir => $"{HybridCLRDataDir}/{GlobalSettings.hotfixDllOutputDir}";

        public static string HybridCLRDataDir => $"{ProjectDir}/{GlobalSettings.hybridCLRDataDir}";

        public static string AssembliesPostIl2CppStripDir => $"{HybridCLRDataDir}/{GlobalSettings.strippedAssemblyDir}";

        public static string LocalIl2CppDir => $"{HybridCLRDataDir}/LocalIl2CppData/il2cpp";

        public static string MethodBridgeCppDir => $"{LocalIl2CppDir}/libil2cpp/hybridclr/interpreter";

        public static string Il2CppBuildCacheDir { get; } = $"{ProjectDir}/Library/Il2cppBuildCache";

        public static string GetHotFixDllsOutputDirByTarget(BuildTarget target)
        {
            return $"{HotFixDllsOutputDir}/{target}";
        }

        public static string GetAssembliesPostIl2CppStripDir(BuildTarget target)
        {
            return $"{SettingsUtil.AssembliesPostIl2CppStripDir}/{target}";
        }

        /// <summary>
        /// 所有热更新dll列表。放到此列表中的dll在打包时OnFilterAssemblies回调中被过滤。
        /// </summary>
        public static List<string> HotUpdateAssemblies
        {
            get
            {
                var gs = GlobalSettings;
                var hotfixAssNames = (gs.hotfixAssemblyDefinitions ?? Array.Empty<AssemblyDefinitionAsset>()).Select(ad => JsonUtility.FromJson<AssemblyDefinitionData>(ad.text));

                var hotfixAssembles = new List<string>();
                foreach (var assName in hotfixAssNames)
                {
                    hotfixAssembles.Add(assName.name);
                }
                hotfixAssembles.AddRange(gs.hotfixAssemblies ?? Array.Empty<string>());
                return hotfixAssembles.Select(dll => dll + ".dll").ToList();
            }
        }

        public static List<string> AOTMetaAssemblies => (GlobalSettings.AOTMetadataDlls ?? Array.Empty<string>()).Select(dll => dll + ".dll").ToList();

        private static T GetSingletonAssets<T>() where T : ScriptableObject, new()
        {
            string assetType = typeof(T).Name;
            string[] globalAssetPaths = AssetDatabase.FindAssets($"t:{assetType}");
            if (globalAssetPaths == null || globalAssetPaths.Length == 0)
            {
                string defaultNewAssetPath = $"Assets/{typeof(T).Name}.asset";
                Debug.LogWarning($"没找到 {assetType} asset，自动创建创建一个:{defaultNewAssetPath}.");

                var newAsset = new T();
                AssetDatabase.CreateAsset(newAsset, defaultNewAssetPath);
                return newAsset;
            }
            if (globalAssetPaths.Length > 1)
            {
                foreach (var assetPath in globalAssetPaths)
                {
                    Debug.LogError($"不能有多个 {assetType}. 路径: {AssetDatabase.GUIDToAssetPath(assetPath)}");
                }
                throw new Exception($"不能有多个 {assetType}");
            }
            string assPath = AssetDatabase.GUIDToAssetPath(globalAssetPaths[0]);
            //Debug.Log($"find asset:{assPath}");
            return AssetDatabase.LoadAssetAtPath<T>(assPath);
        }

        public static HybridCLRGlobalSettings GlobalSettings => GetSingletonAssets<HybridCLRGlobalSettings>();

        public static HybridCLRLinkSettings LinkSettings => GetSingletonAssets<HybridCLRLinkSettings>();

        public static HybridCLRMethodBridgeSettings MethodBridgeSettings => GetSingletonAssets<HybridCLRMethodBridgeSettings>();
    }
}
