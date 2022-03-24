using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LoadDllFromAssetbundle : MonoBehaviour
{
    /// <summary>
    /// 这里展示的流程不是最优流程
    /// 仅做最简单展示如何加载AssetBundle中的华佗hotfix.dll
    /// </summary>
    private void Start()
    {
#if !UNITY_EDITOR
        Debug.Log("Load ab");

        StartCoroutine(LoadAssetBundle(Application.streamingAssetsPath + "/huatuo",
            (_assetBundle) =>
        {
            gameAss = System.Reflection.Assembly.Load(_assetBundle.LoadAsset<TextAsset>("HotFix").bytes);

            RunMain();
        }));

#else

        LoadGameDll();
        RunMain();

#endif
    }

    public static System.Reflection.Assembly gameAss;

    private void LoadGameDll()
    {
#if UNITY_EDITOR

        Debug.Log("Load ab");

        AssetBundle _assetBundle = AssetBundle.LoadFromFile(Application.dataPath + "/HuaTuo/Output/huatuo");

        if (_assetBundle == null)
        {
            Debug.LogError("请先使用[HuaTuo/Build/BuildDLLAssetBundle]生成对应平台ab文件.");
        }
        else
        {
            gameAss = System.Reflection.Assembly.Load(_assetBundle.LoadAsset<TextAsset>("HotFix").bytes);
        }

#endif
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

        // 如果是Update之类的函数，推荐先转成Delegate再调用，如
        //var updateMethod = appType.GetMethod("Update");
        //var updateDel = System.Delegate.CreateDelegate(typeof(Action<float>), null, updateMethod);
        //updateMethod(deltaTime);
    }

    private IEnumerator LoadAssetBundle(string _path, Action<AssetBundle> _callback)
    {
        UnityWebRequest _request = UnityWebRequestAssetBundle.GetAssetBundle(_path);
        yield return _request.SendWebRequest();

        if (_request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(_request.error);
        }
        else
        {
            AssetBundle _bundle = DownloadHandlerAssetBundle.GetContent(_request);

            if (_callback != null)
            {
                _callback(_bundle);
            }
        }
    }
}