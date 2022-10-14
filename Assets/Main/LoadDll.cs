using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class LoadDll : MonoBehaviour
{


    public static List<string> AOTMetaAssemblyNames { get; } = new List<string>()
    {
        "mscorlib",
        "System",
        "System.Core",
    };

    void Start()
    {
        StartCoroutine(DownLoadAssets(this.StartGame));
    }

    private static Dictionary<string, byte[]> s_assetDatas = new Dictionary<string, byte[]>();

    public static byte[] GetAssetData(string dllName)
    {
        return s_assetDatas[dllName];
    }

    private string GetWebRequestPath(string asset)
    {
        var path = $"{Application.streamingAssetsPath}/{asset}";
        if (!path.Contains("://"))
        {
            path = "file://" + path;
        }
        return path;
    }

    IEnumerator DownLoadAssets(Action onDownloadComplete)
    {
        var assets = new List<string>
        {
            "prefabs",
            "HotUpdateAssemblies/Assembly-CSharp.dll",
        }.Concat(AOTMetaAssemblyNames.Select(n => $"AOTAssemblies/{n}.dll")).ToList();

        foreach (var asset in assets)
        {
            string dllPath = GetWebRequestPath(asset);
            Debug.Log($"start download asset:{dllPath}");
            UnityWebRequest www = UnityWebRequest.Get(dllPath);
            yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
#else
            if (www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
            }
#endif
            else
            {
                // Or retrieve results as binary data
                byte[] assetData = www.downloadHandler.data;
                Debug.Log($"dll:{asset}  size:{assetData.Length}");
                s_assetDatas[asset] = assetData;
            }
        }

        onDownloadComplete();
    }


    void StartGame()
    {
        LoadMetadataForAOTAssemblies();

#if !UNITY_EDITOR
        var gameAss = System.Reflection.Assembly.Load(GetAssetData("HotUpdateAssemblies/Assembly-CSharp.dll"));
#else
        var gameAss = AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "Assembly-CSharp");
#endif

        AssetBundle prefabAb = AssetBundle.LoadFromMemory(GetAssetData("prefabs"));
        GameObject testPrefab = Instantiate(prefabAb.LoadAsset<GameObject>("HotUpdatePrefab.prefab"));
    }



    /// <summary>
    /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
    /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
    /// </summary>
    private static void LoadMetadataForAOTAssemblies()
    {
        // 可以加载任意aot assembly的对应的dll。但要求dll必须与unity build过程中生成的裁剪后的dll一致，而不能直接使用原始dll。
        // 我们在BuildProcessors里添加了处理代码，这些裁剪后的dll在打包时自动被复制到 {项目目录}/HybridCLRData/AssembliesPostIl2CppStrip/{Target} 目录。

        /// 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
        /// 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
        /// 
        foreach (var aotDllName in AOTMetaAssemblyNames)
        {
            byte[] dllBytes = GetAssetData($"AOTAssemblies/{aotDllName}.dll");
            // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
        }
    }
}
