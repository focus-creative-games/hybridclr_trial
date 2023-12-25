using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;


public static class Entry
{
    public static void Start()
    {
        Debug.Log("[Entry::Start] 看到这个日志表示你成功运行了热更新代码");

#if UNITY_EDITOR
        Assembly unloadAss = System.AppDomain.CurrentDomain.GetAssemblies().First(ass => ass.GetName().Name == "Unload");
#else
        byte[] unloadBytes = File.ReadAllBytes($"{Application.streamingAssetsPath}/Unload.dll.bytes");
        Assembly unloadAss = Assembly.Load(unloadBytes);
#endif
        unloadAss.GetType("UnloadEntry").GetMethod("Start").Invoke(null, null);
        UnloadAssembly(unloadAss);
    }

    static async void UnloadAssembly(Assembly unloadAss)
    {
        UnityEngine.Debug.Log("UnloadAssembly 3 seconds later...");
        await Task.Delay(3000);
        UnityEngine.Debug.Log("UnloadAssembly now...");
        RuntimeApi.UnloadAssembly(unloadAss);
        UnityEngine.Debug.Log("UnloadAssembly done...");
    }

}