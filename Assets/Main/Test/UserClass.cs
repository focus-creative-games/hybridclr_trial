using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UserClass
{
    static int s_num;
    static string s_str;

    public static void TestFunc1(int num, string str, Vector3 pos, Transform trans)
    {
        s_num = num;
        s_str = str;

        trans.position = pos;
    }
}
