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

    private void LoadGameDll()
    {
#if !UNITY_EDITOR
        // 此代码在Android等平台下并不能工作，请酌情调整
        string gameDll = Application.streamingAssetsPath + "/HotFix.dll";
        gameAss = System.Reflection.Assembly.Load(File.ReadAllBytes(gameDll));
#else
        gameAss = AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "HotFix");
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
}
