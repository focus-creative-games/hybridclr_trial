using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadDll : MonoBehaviour
{
    void Start()
    {
        BetterStreamingAssets.Initialize();
        LoadGameDll();
        RunMain();
    }

    public static System.Reflection.Assembly gameAss;

    private void LoadGameDll()
    {
#if UNITY_EDITOR
        string gameDll = Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll";
        byte[] dllBytes = File.ReadAllBytes(gameDll);
        // 使用File.ReadAllBytes是为了避免Editor下gameDll文件被占用导致后续编译后无法覆盖
#else
        string gameDll = "HotFix.dll";
        byte[] dllBytes = BetterStreamingAssets.ReadAllBytes(gameDll);
#endif
        gameAss = System.Reflection.Assembly.Load(dllBytes);
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
}
