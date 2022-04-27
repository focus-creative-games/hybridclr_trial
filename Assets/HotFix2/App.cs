using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class App
{
    public static int Main()
    {
        Debug.Log("hello, huatuo");

        var go = new GameObject("HotFix2");
        go.AddComponent<CreateByHotFix2>();

        return 0;
    }
}
