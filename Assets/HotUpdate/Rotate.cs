using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 1;

    void Start()
    {
        Debug.Log("[Rotate.Start] 启动热更新Rotate脚本");
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, speed);
    }
}
