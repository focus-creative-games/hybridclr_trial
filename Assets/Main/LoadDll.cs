using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LoadDll : MonoBehaviour
{
    /// <summary>
    /// 编辑器下是否启用AB加载流程
    /// </summary>
    public bool m_loadAB = false;

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
        if (m_loadAB)
        {
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
        }
        else
        {
            Debug.Log("Load dll");

            string gameDll = Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll";
            // 使用File.ReadAllBytes是为了避免Editor下gameDll文件被占用导致后续编译后无法覆盖
            gameAss = System.Reflection.Assembly.Load(File.ReadAllBytes(gameDll));
        }

#else

        string gameDll = Application.streamingAssetsPath + "/HotFix.dll";
        Debug.LogError(gameDll);
        Debug.LogError(Application.persistentDataPath + "/HotFix.dll");

        gameAss = System.Reflection.Assembly.LoadFile(gameDll);

        //string gameDll = Application.persistentDataPath + "/HotFix.dll";
        //Debug.LogError(gameDll);

        //gameAss = System.Reflection.Assembly.LoadFile(gameDll);

        // 这里的加载只是用来做最简单的加载测试, 具体请根据自己的需求订制

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