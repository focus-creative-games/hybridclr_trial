using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


public class Demos
{
    private Assembly _hotUpdateAss;

    public Demos()
    {
        _hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
    }

    public void Run()
    {
        Run_ReflectionInvoke();
        Run_CreateComponentByCode();
        Run_CreateComponentByInstantiatePrefab();
        Run_AOTGeneric();
    }

    private void Run_ReflectionInvoke()
    {
        Type type = _hotUpdateAss.GetType("ReflectionInvoke");
        type.GetMethod("Run").Invoke(null, null);
    }

    private void Run_CreateComponentByCode()
    {
        GameObject cube = GameObject.Find("Cube");
        Type rotateType = _hotUpdateAss.GetType("Rotate");
        cube.AddComponent(rotateType);
    }

    private void Run_CreateComponentByInstantiatePrefab()
    {
        AssetBundle ab = AssetBundle.LoadFromMemory(LoadDll.ReadBytesFromStreamingAssets("prefabs"));
        GameObject cube = ab.LoadAsset<GameObject>("Cube");
        GameObject.Instantiate(cube);
    }


    struct MyVec3
    {
        public int x;
        public int y;
        public int z;
    }

    private void Run_AOTGeneric()
    {
        var arr = new List<MyVec3>();
        Debug.Log("[Demos.Run_AOTGeneric] 成功运行泛型代码");
    }
}