using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintHello : MonoBehaviour
{

    public string text;

    // Start is called before the first frame update
    void Start()
    {
        Debug.LogFormat("hello, huatuo. {0}", text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
