using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LoadDll : MonoBehaviour
{
    void Start()
    {
        BetterStreamingAssets.Initialize();
        LoadGameDll();
        RunMain();
    }

    private System.Reflection.Assembly gameAss;

    public static AssetBundle AssemblyAssetBundle { get; private set; }

    private void LoadGameDll()
    {
        AssetBundle dllAB = AssemblyAssetBundle = BetterStreamingAssets.LoadAssetBundle("common");
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

        // 如果是Update之类的函数，推荐先转成Delegate再调用，如
        //var updateMethod = appType.GetMethod("Update");
        //var updateDel = System.Delegate.CreateDelegate(typeof(Action<float>), null, updateMethod);
        //updateDel(deltaTime);
    }
}
