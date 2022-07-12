using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintHello : MonoBehaviour
{

    public string text;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("这个热更新脚本挂载在prefab上，打包成ab。通过从ab中实例化prefab成功还原");
        Debug.LogFormat("hello, HybridCLR. {0}", text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
