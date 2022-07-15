using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class LoadDll : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DownLoadDlls(this.StartGame));
    }

    private static Dictionary<string, byte[]> s_abBytes = new Dictionary<string, byte[]>();

    public static byte[] GetAbBytes(string dllName)
    {
        return s_abBytes[dllName];
    }

    IEnumerator DownLoadDlls(Action onDownloadComplete)
    {
        var abs = new string[]
        {
            "common",
        };
        foreach (var ab in abs)
        {
            string dllPath = $"{Application.streamingAssetsPath}/{ab}";
            Debug.Log($"start download ab:{ab}");
            UnityWebRequest www = UnityWebRequest.Get(dllPath);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Or retrieve results as binary data
                byte[] abBytes = www.downloadHandler.data;
                Debug.Log($"dll:{ab}  size:{abBytes.Length}");
                s_abBytes[ab] = abBytes;
            }
        }

        onDownloadComplete();
    }


    void StartGame()
    {
        LoadGameDll();
        RunMain();
    }

    private System.Reflection.Assembly gameAss;

    public static AssetBundle AssemblyAssetBundle { get; private set; }

    private void LoadGameDll()
    {
        AssetBundle dllAB = AssemblyAssetBundle = AssetBundle.LoadFromMemory(GetAbBytes("common"));
#if !UNITY_EDITOR
        TextAsset dllBytes1 = dllAB.LoadAsset<TextAsset>("HotFix.dll.bytes");
        System.Reflection.Assembly.Load(dllBytes1.bytes);
        TextAsset dllBytes2 = dllAB.LoadAsset<TextAsset>("HotFix2.dll.bytes");
        gameAss = System.Reflection.Assembly.Load(dllBytes2.bytes);
#else
        gameAss = AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "HotFix2");
#endif

        GameObject testPrefab = GameObject.Instantiate(dllAB.LoadAsset<UnityEngine.GameObject>("HotUpdatePrefab.prefab"));
    }

    public void RunMain()
    {
        if (gameAss == null)
        {
            UnityEngine.Debug.LogError("dll未加载");
            return;
        }
        var appType = gameAss.GetType("App");
        var mainMethod = appType.GetMethod("Main");
        mainMethod.Invoke(null, null);
    }
}
