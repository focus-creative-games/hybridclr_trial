using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class LoadDll : MonoBehaviour
{


    void Start()
    {
        StartGame();
    }

    public static byte[] ReadBytesFromStreamingAssets(string file)
    {
        // Android平台不支持直接读取StreamingAssets下文件，请自行修改实现
        return File.ReadAllBytes($"{Application.streamingAssetsPath}/{file}");
    }

    Assembly _ass;

    void StartGame()
    {
        LoadMetadataForAOTAssemblies();
#if !UNITY_EDITOR
        Assembly.Load(ReadBytesFromStreamingAssets("HotUpdate.dll.bytes"));
#endif
        var demos = new Demos();
        demos.Run();

#if UNITY_STANDALONE_WIN
        // 以下代码只为了方便自动化测试，与演示无关
        File.WriteAllText("run.log", "ok", System.Text.Encoding.UTF8);
        if (File.Exists("autoexit"))
        {
            Debug.Log("==== 本程序将于3秒后自动退出 ====");
            Task.Run(async () =>
            {
                await Task.Delay(3000);
                Application.Quit(0);
            });
        }
#endif
    }

    /// <summary>
    /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
    /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
    /// </summary>
    private static void LoadMetadataForAOTAssemblies()
    {
        List<string> aotMetaAssemblyFiles = new List<string>()
        {
            "mscorlib.dll",
            "System.dll",
            "System.Core.dll",
        };
        /// 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
        /// 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
        /// 
        HomologousImageMode mode = HomologousImageMode.SuperSet;
        foreach (var aotDllName in aotMetaAssemblyFiles)
        {
            byte[] dllBytes = ReadBytesFromStreamingAssets(aotDllName + ".bytes");
            // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
        }
    }
}
