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

    private string GetWebRequestPath(string asset)
    {
        var path = $"{Application.streamingAssetsPath}/{asset}";
        if (!path.Contains("://"))
        {
            path = "file://" + path;
        }
        return path;
    }

    IEnumerator DownLoadDlls(Action onDownloadComplete)
    {
        var abs = new string[]
        {
            "aotdlls",
            "hotupdatedlls",
            "prefabs",
        };
        foreach (var ab in abs)
        {
            string dllPath = GetWebRequestPath(ab);
            Debug.Log($"start download ab:{ab}");
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
                byte[] abBytes = www.downloadHandler.data;
                Debug.Log($"dll:{ab}  size:{abBytes.Length}");
                s_abBytes[ab] = abBytes;
            }
        }

        onDownloadComplete();
    }


    void StartGame()
    {
        AssetBundle hotUpdateDllAb = AssetBundle.LoadFromMemory(GetAbBytes("hotupdatedlls"));
#if !UNITY_EDITOR
        TextAsset dllBytes = hotUpdateDllAb.LoadAsset<TextAsset>("Assembly-CSharp.dll.bytes");
        var gameAss = System.Reflection.Assembly.Load(dllBytes.bytes);
#else
        var gameAss = AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "Assembly-CSharp");
#endif

        AssetBundle prefabAb = AssetBundle.LoadFromMemory(GetAbBytes("prefabs"));
        GameObject testPrefab = Instantiate(prefabAb.LoadAsset<GameObject>("HotUpdatePrefab.prefab"));
    }
}
