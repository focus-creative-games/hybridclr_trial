using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class LoadDll : MonoBehaviour
{

    void Start()
    {
        // Editor环境下，HotUpdate.dll.bytes已经被自动加载，不需要加载，重复加载反而会出问题。
#if !UNITY_EDITOR
        var manifests = LoadManifest($"{Application.streamingAssetsPath}/manifest.txt");
        Assembly hotUpdateAss = LoadDifferentialHybridAssembly(manifests["HotUpdate"], "HotUpdate");
#else
        // Editor下无需加载，直接查找获得HotUpdate程序集
        Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
#endif
        Type helloType = hotUpdateAss.GetType("Hello");
        MethodInfo runMethod = helloType.GetMethod("Run");
        runMethod.Invoke(null, null);
    }

    class Manifest
    {
        public string AssemblyName { get; set; }

        public string OriginalDllMd5 { get; set; }
    }

    private Dictionary<string, Manifest> LoadManifest(string manifestFile)
    {
        var manifest = new Dictionary<string, Manifest>();
        var lines = File.ReadAllLines(manifestFile, Encoding.UTF8);
        foreach (var line in lines)
        {
            string[] args = line.Split(",");
            if (args.Length != 2)
            {
                Debug.LogError($"manifest file format error, line={line}");
                return null;
            }
            manifest.Add(args[0], new Manifest()
            {
                AssemblyName = args[0],
                OriginalDllMd5 = args[1],
            });
        }
        return manifest;
    }


    public static string CreateMD5Hash(byte[] bytes)
    {
        return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(bytes)).Replace("-", "").ToUpperInvariant();
    }

    private Assembly LoadDifferentialHybridAssembly(Manifest manifest, string assName)
    {
        byte[] dllBytes = File.ReadAllBytes($"{Application.streamingAssetsPath}/{assName}.dll.bytes");
        byte[] dhaoBytes = File.ReadAllBytes($"{Application.streamingAssetsPath}/{assName}.dhao.bytes");
        string currentDllMd5 = CreateMD5Hash(dllBytes);
        LoadImageErrorCode err = RuntimeApi.LoadDifferentialHybridAssembly(dllBytes, dhaoBytes, manifest.OriginalDllMd5, currentDllMd5);
        if (err == LoadImageErrorCode.OK)
        {
            Debug.Log($"LoadDifferentialHybridAssembly {assName} OK");
            return System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == assName);
        }
        else
        {
            Debug.LogError($"LoadDifferentialHybridAssembly {assName} failed, err={err}");
            return null;
        }
    }
}
