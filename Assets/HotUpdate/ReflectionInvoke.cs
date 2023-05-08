using System.Collections;
using UnityEngine;

public class ReflectionInvoke
{
    public static void Run()
    {
        GameObject cube = GameObject.Find("Cube");
        cube.GetComponent<MeshRenderer>().material.color = Color.red;
        Debug.Log("[ReflectionInvoke.Run] Cube颜色变成红色");
    }
}