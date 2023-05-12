using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


public static class Entry
{
    public static void Start()
    {
        Run_CreateComponentByCode();
        Run_AOTGeneric();
    }

    private static void Run_CreateComponentByCode()
    {
        // 代码中动态挂载脚本
        GameObject cube = GameObject.Find("Cube");
        cube.AddComponent<Rotate>();
    }


    struct MyVec3
    {
        public int x;
        public int y;
        public int z;
    }

    private static void Run_AOTGeneric()
    {
        // 泛型实例化
        var arr = new List<MyVec3>();
        arr.Add(new MyVec3 { x = 1 });
        Debug.Log("[Demos.Run_AOTGeneric] 成功运行泛型代码");
    }
}