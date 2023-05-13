using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class InstantiateByAsset : MonoBehaviour
{
    public string text;

    void Start()
    {
        Debug.Log($"[InstantiateByAsset] text:{text}, 这个脚本通过挂载到资源的方式实例化");
    }
}
