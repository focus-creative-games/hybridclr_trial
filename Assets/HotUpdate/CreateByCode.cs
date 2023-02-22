using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateByCode : MonoBehaviour
{
    void Start()
    {
        Debug.Log($"[{GetType().FullName}] 这个脚本是通过代码AddComponent直接创建的");
    }
}
