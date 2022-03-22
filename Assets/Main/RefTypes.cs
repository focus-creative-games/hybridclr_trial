using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


enum IntEnum : int
{
    A,
    B,
}

public class RefTypes : MonoBehaviour
{
    // Start is called before the first frame update

    List<Type> GetTypes()
    {

        return new List<Type>
        {

        };
    }

    void Start()
    {
        Debug.Log(GetTypes());

        GameObject.Instantiate<GameObject>(null);
        Instantiate<GameObject>(null, null);
        Instantiate<GameObject>(null, null, false);
        Instantiate<GameObject>(null, new Vector3(), new Quaternion());
        Instantiate<GameObject>(null, new Vector3(), new Quaternion(), null);

        //int, long,float,double, IntEnum,object
        object b = new Dictionary<int, int>();
        b = new Dictionary<int, long>();
        b = new Dictionary<int, float>();
        b = new Dictionary<int, double>();
        b = new Dictionary<int, object>();
        b = new Dictionary<int, IntEnum>();
        b = new Dictionary<long, int>();
        b = new Dictionary<long, long>();
        b = new Dictionary<long, object>();
        b = new Dictionary<long, float>();
        b = new Dictionary<long, double>();
        b = new Dictionary<long, IntEnum>();
        b = new Dictionary<float, int>();
        b = new Dictionary<float, long>();
        b = new Dictionary<float, float>();
        b = new Dictionary<float, double>();
        b = new Dictionary<float, object>();
        b = new Dictionary<float, IntEnum>();
        b = new Dictionary<double, int>();
        b = new Dictionary<double, long>();
        b = new Dictionary<double, float>();
        b = new Dictionary<double, double>();
        b = new Dictionary<double, object>();
        b = new Dictionary<double, IntEnum>();
        b = new Dictionary<object, int>();
        b = new Dictionary<object, long>();
        b = new Dictionary<object, float>();
        b = new Dictionary<object, double>();
        b = new Dictionary<object, object>();
        b = new Dictionary<object, IntEnum>();
        b = new Dictionary<IntEnum, int>();
        b = new Dictionary<IntEnum, long>();
        b = new Dictionary<IntEnum, float>();
        b = new Dictionary<IntEnum, double>();
        b = new Dictionary<IntEnum, object>();
        b = new Dictionary<IntEnum, IntEnum>();

        b = new Dictionary<(int, long, string), object>();
        b = new Dictionary<(long, int, string), object>();

        // == sorted 
        b = new SortedDictionary<int, int>();
        b = new SortedDictionary<int, long>();
        b = new SortedDictionary<int, float>();
        b = new SortedDictionary<int, double>();
        b = new SortedDictionary<int, object>();
        b = new SortedDictionary<int, IntEnum>();
        b = new SortedDictionary<long, int>();
        b = new SortedDictionary<long, long>();
        b = new SortedDictionary<long, float>();
        b = new SortedDictionary<long, double>();
        b = new SortedDictionary<long, object>();
        b = new SortedDictionary<long, IntEnum>();
        b = new SortedDictionary<float, int>();
        b = new SortedDictionary<float, long>();
        b = new SortedDictionary<float, float>();
        b = new SortedDictionary<float, double>();
        b = new SortedDictionary<float, object>();
        b = new SortedDictionary<float, IntEnum>();
        b = new SortedDictionary<double, int>();
        b = new SortedDictionary<double, long>();
        b = new SortedDictionary<double, float>();
        b = new SortedDictionary<double, double>();
        b = new SortedDictionary<double, object>();
        b = new SortedDictionary<double, IntEnum>();
        b = new SortedDictionary<object, int>();
        b = new SortedDictionary<object, long>();
        b = new SortedDictionary<object, float>();
        b = new SortedDictionary<object, double>();
        b = new SortedDictionary<object, object>();
        b = new SortedDictionary<object, IntEnum>();
        b = new SortedDictionary<IntEnum, int>();
        b = new SortedDictionary<IntEnum, long>();
        b = new SortedDictionary<IntEnum, float>();
        b = new SortedDictionary<IntEnum, double>();
        b = new SortedDictionary<IntEnum, object>();
        b = new SortedDictionary<IntEnum, IntEnum>();

        b = new HashSet<int>();
        b = new HashSet<long>();
        b = new HashSet<float>();
        b = new HashSet<double>();
        b = new HashSet<object>();
        b = new HashSet<IntEnum>();
        b = new List<int>();
        b = new List<long>();
        b = new List<float>();
        b = new List<double>();
        b = new List<string>();
        b = new List<object>();
        b = new List<IntEnum>();

        {
            b = new System.ValueTuple<int>(1);
            b = new System.ValueTuple<long>(1);
            b = new System.ValueTuple<float>(1f);
            b = new System.ValueTuple<double>(1);
            b = new System.ValueTuple<object>(null);
            b = new System.ValueTuple<IntEnum>(IntEnum.A);

            Debug.Log(b);
            b = new System.ValueTuple<int, int>(1, 1);
            Debug.Log(b);
            b = new System.ValueTuple<int, int>(1, 1);
            b = new System.ValueTuple<int, long>(1, 1);
            b = new System.ValueTuple<int, float>(1, 1);
            b = new System.ValueTuple<int, double>(1, 1);
            b = new System.ValueTuple<int, object>(1, null);
            b = new System.ValueTuple<int, IntEnum>(1, IntEnum.B);

            b = new System.ValueTuple<long, int>(1, 1);
            b = new System.ValueTuple<long, long>(1, 1);
            b = new System.ValueTuple<long, float>(1, 1);
            b = new System.ValueTuple<long, double>(1, 1);
            b = new System.ValueTuple<long, object>(1, null);
            b = new System.ValueTuple<long, IntEnum>(1, IntEnum.B);

            Debug.Log(b);
            b = new System.ValueTuple<int, int, int>(1, 1, 1);
            Debug.Log(b);
            b = new System.ValueTuple<int, long, object>(1, 2, null);
            Debug.Log(b);
            b = new System.ValueTuple<int, string, string>(1, "", "");
        }

        Debug.Log(b);
        // nullable
        int? a = 5;
        b = a;
        int d = (int?)b ?? 7;
        int e = (int)b;
        a = d;
        b = a;

        b = new System.ValueTuple<object, int>(null, 1);
        b = new System.ValueTuple<object, long>(null, 1);
        b = new System.ValueTuple<object, float>(null, 1);
        b = new System.ValueTuple<object, double>(null, 1);
        b = new System.ValueTuple<object, object>(null, null);
        b = new System.ValueTuple<object, IntEnum>(null, IntEnum.B);

        b = new System.ValueTuple<IntEnum, int>(IntEnum.A, 1);
        b = new System.ValueTuple<IntEnum, long>(IntEnum.A, 1);
        b = new System.ValueTuple<IntEnum, float>(IntEnum.A, 1);
        b = new System.ValueTuple<IntEnum, double>(IntEnum.A, 1);
        b = new System.ValueTuple<IntEnum, object>(IntEnum.A, null);
        b = new System.ValueTuple<IntEnum, IntEnum>(IntEnum.A, IntEnum.B);

        b = new System.ValueTuple<int, long, float>(1, 1, 1);
        b = new System.ValueTuple<int, long, double>(1, 1, 1);
        b = new System.ValueTuple<int, long, object>(1, 1, null);
        b = new System.ValueTuple<int, long, IntEnum>(1, 1, IntEnum.A);
        b = new System.ValueTuple<int, float, long>(1, 1, 1);
        b = new System.ValueTuple<int, float, double>(1, 1, 1);
        b = new System.ValueTuple<int, float, object>(1, 1, null);
        b = new System.ValueTuple<int, float, IntEnum>(1, 1, IntEnum.A);
        b = new System.ValueTuple<int, double, long>(1, 1, 1);
        b = new System.ValueTuple<int, double, float>(1, 1, 1);
        b = new System.ValueTuple<int, double, object>(1, 1, null);
        b = new System.ValueTuple<int, double, IntEnum>(1, 1, IntEnum.A);
        b = new System.ValueTuple<int, object, long>(1, null, 1);
        b = new System.ValueTuple<int, object, float>(1, null, 1);
        b = new System.ValueTuple<int, object, double>(1, null, 1);
        b = new System.ValueTuple<int, object, IntEnum>(1, null, IntEnum.A);
        b = new System.ValueTuple<int, IntEnum, long>(1, IntEnum.A, 1);
        b = new System.ValueTuple<int, IntEnum, float>(1, IntEnum.A, 1);
        b = new System.ValueTuple<int, IntEnum, double>(1, IntEnum.A, 1);
        b = new System.ValueTuple<int, IntEnum, object>(1, IntEnum.A, null);
        b = new System.ValueTuple<long, int, float>(1, 1, 1);
        b = new System.ValueTuple<long, int, double>(1, 1, 1);
        b = new System.ValueTuple<long, int, object>(1, 1, null);
        b = new System.ValueTuple<long, int, IntEnum>(1, 1, IntEnum.A);
        b = new System.ValueTuple<long, float, int>(1, 1, 1);
        b = new System.ValueTuple<long, float, double>(1, 1, 1);
        b = new System.ValueTuple<long, float, object>(1, 1, null);
        b = new System.ValueTuple<long, float, IntEnum>(1, 1, IntEnum.A);
        b = new System.ValueTuple<long, double, int>(1, 1, 1);
        b = new System.ValueTuple<long, double, float>(1, 1, 1);
        b = new System.ValueTuple<long, double, object>(1, 1, null);
        b = new System.ValueTuple<long, double, IntEnum>(1, 1, IntEnum.A);
        b = new System.ValueTuple<long, object, int>(1, null, 1);
        b = new System.ValueTuple<long, object, float>(1, null, 1);
        b = new System.ValueTuple<long, object, double>(1, null, 1);
        b = new System.ValueTuple<long, object, IntEnum>(1, null, IntEnum.A);
        b = new System.ValueTuple<long, IntEnum, int>(1, IntEnum.A, 1);
        b = new System.ValueTuple<long, IntEnum, float>(1, IntEnum.A, 1);
        b = new System.ValueTuple<long, IntEnum, double>(1, IntEnum.A, 1);
        b = new System.ValueTuple<long, IntEnum, object>(1, IntEnum.A, null);
        b = new System.ValueTuple<float, int, long>(1, 1, 1);
        b = new System.ValueTuple<float, int, double>(1, 1, 1);
        b = new System.ValueTuple<float, int, object>(1, 1, null);
        b = new System.ValueTuple<float, int, IntEnum>(1, 1, IntEnum.A);
        b = new System.ValueTuple<float, long, int>(1, 1, 1);
        b = new System.ValueTuple<float, long, double>(1, 1, 1);
        b = new System.ValueTuple<float, long, object>(1, 1, null);
        b = new System.ValueTuple<float, long, IntEnum>(1, 1, IntEnum.A);
        b = new System.ValueTuple<float, double, int>(1, 1, 1);
        b = new System.ValueTuple<float, double, long>(1, 1, 1);
        b = new System.ValueTuple<float, double, object>(1, 1, null);
        b = new System.ValueTuple<float, double, IntEnum>(1, 1, IntEnum.A);
        b = new System.ValueTuple<float, object, int>(1, null, 1);
        b = new System.ValueTuple<float, object, long>(1, null, 1);
        b = new System.ValueTuple<float, object, double>(1, null, 1);
        b = new System.ValueTuple<float, object, IntEnum>(1, null, IntEnum.A);
        b = new System.ValueTuple<float, IntEnum, int>(1, IntEnum.A, 1);
        b = new System.ValueTuple<float, IntEnum, long>(1, IntEnum.A, 1);
        b = new System.ValueTuple<float, IntEnum, double>(1, IntEnum.A, 1);
        b = new System.ValueTuple<float, IntEnum, object>(1, IntEnum.A, null);
        b = new System.ValueTuple<double, int, long>(1, 1, 1);
        b = new System.ValueTuple<double, int, float>(1, 1, 1);
        b = new System.ValueTuple<double, int, object>(1, 1, null);
        b = new System.ValueTuple<double, int, IntEnum>(1, 1, IntEnum.A);
        b = new System.ValueTuple<double, long, int>(1, 1, 1);
        b = new System.ValueTuple<double, long, float>(1, 1, 1);
        b = new System.ValueTuple<double, long, object>(1, 1, null);
        b = new System.ValueTuple<double, long, IntEnum>(1, 1, IntEnum.A);
        b = new System.ValueTuple<double, float, int>(1, 1, 1);
        b = new System.ValueTuple<double, float, long>(1, 1, 1);
        b = new System.ValueTuple<double, float, object>(1, 1, null);
        b = new System.ValueTuple<double, float, IntEnum>(1, 1, IntEnum.A);
        b = new System.ValueTuple<double, object, int>(1, null, 1);
        b = new System.ValueTuple<double, object, long>(1, null, 1);
        b = new System.ValueTuple<double, object, float>(1, null, 1);
        b = new System.ValueTuple<double, object, IntEnum>(1, null, IntEnum.A);
        b = new System.ValueTuple<double, IntEnum, int>(1, IntEnum.A, 1);
        b = new System.ValueTuple<double, IntEnum, long>(1, IntEnum.A, 1);
        b = new System.ValueTuple<double, IntEnum, float>(1, IntEnum.A, 1);
        b = new System.ValueTuple<double, IntEnum, object>(1, IntEnum.A, null);
        b = new System.ValueTuple<object, int, long>(null, 1, 1);
        b = new System.ValueTuple<object, int, float>(null, 1, 1);
        b = new System.ValueTuple<object, int, double>(null, 1, 1);
        b = new System.ValueTuple<object, int, IntEnum>(null, 1, IntEnum.A);
        b = new System.ValueTuple<object, long, int>(null, 1, 1);
        b = new System.ValueTuple<object, long, float>(null, 1, 1);
        b = new System.ValueTuple<object, long, double>(null, 1, 1);
        b = new System.ValueTuple<object, long, IntEnum>(null, 1, IntEnum.A);
        b = new System.ValueTuple<object, float, int>(null, 1, 1);
        b = new System.ValueTuple<object, float, long>(null, 1, 1);
        b = new System.ValueTuple<object, float, double>(null, 1, 1);
        b = new System.ValueTuple<object, float, IntEnum>(null, 1, IntEnum.A);
        b = new System.ValueTuple<object, double, int>(null, 1, 1);
        b = new System.ValueTuple<object, double, long>(null, 1, 1);
        b = new System.ValueTuple<object, double, float>(null, 1, 1);
        b = new System.ValueTuple<object, double, IntEnum>(null, 1, IntEnum.A);
        b = new System.ValueTuple<object, IntEnum, int>(null, IntEnum.A, 1);
        b = new System.ValueTuple<object, IntEnum, long>(null, IntEnum.A, 1);
        b = new System.ValueTuple<object, IntEnum, float>(null, IntEnum.A, 1);
        b = new System.ValueTuple<object, IntEnum, double>(null, IntEnum.A, 1);
        b = new System.ValueTuple<IntEnum, int, long>(IntEnum.A, 1, 1);
        b = new System.ValueTuple<IntEnum, int, float>(IntEnum.A, 1, 1);
        b = new System.ValueTuple<IntEnum, int, double>(IntEnum.A, 1, 1);
        b = new System.ValueTuple<IntEnum, int, object>(IntEnum.A, 1, null);
        b = new System.ValueTuple<IntEnum, long, int>(IntEnum.A, 1, 1);
        b = new System.ValueTuple<IntEnum, long, float>(IntEnum.A, 1, 1);
        b = new System.ValueTuple<IntEnum, long, double>(IntEnum.A, 1, 1);
        b = new System.ValueTuple<IntEnum, long, object>(IntEnum.A, 1, null);
        b = new System.ValueTuple<IntEnum, float, int>(IntEnum.A, 1, 1);
        b = new System.ValueTuple<IntEnum, float, long>(IntEnum.A, 1, 1);
        b = new System.ValueTuple<IntEnum, float, double>(IntEnum.A, 1, 1);
        b = new System.ValueTuple<IntEnum, float, object>(IntEnum.A, 1, null);
        b = new System.ValueTuple<IntEnum, double, int>(IntEnum.A, 1, 1);
        b = new System.ValueTuple<IntEnum, double, long>(IntEnum.A, 1, 1);
        b = new System.ValueTuple<IntEnum, double, float>(IntEnum.A, 1, 1);
        b = new System.ValueTuple<IntEnum, double, object>(IntEnum.A, 1, null);
        b = new System.ValueTuple<IntEnum, object, int>(IntEnum.A, null, 1);
        b = new System.ValueTuple<IntEnum, object, long>(IntEnum.A, null, 1);
        b = new System.ValueTuple<IntEnum, object, float>(IntEnum.A, null, 1);
        b = new System.ValueTuple<IntEnum, object, double>(IntEnum.A, null, 1);
        b = Enumerable.Range(0, 1).Reverse().Take(1).TakeWhile(x => true).Skip(1).All(x => true);
        b = new WaitForSeconds(1f);
        b = new WaitForSecondsRealtime(1f);
        b = new WaitForFixedUpdate();
        b = new WaitForEndOfFrame();
        b = new WaitWhile(() => true);
        b = new WaitUntil(() => true);
        b = new SkinnedMeshRenderer();
        b = new Renderer();
        b = Input.mousePosition;
        Debug.Log(b);
        var tp = typeof(SkinnedMeshRenderer);
        var go = new GameObject();
        go.AddComponent(tp);
        var c = go.GetComponent(tp) as SkinnedMeshRenderer;
        c.receiveShadows = true;
        var q1 = Quaternion.Euler(1, 1, 1);
        Quaternion.Slerp(Quaternion.identity, q1, 0.5f);
        Debug.Log(c);
        Debug.Log(q1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
