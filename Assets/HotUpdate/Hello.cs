using System.Collections;
using UnityEngine;

public class Hello
{
    public static void Run()
    {
        Debug.Log("Hello, HybridCLR");
        Benchmark();
    }


    public static void Benchmark()
    {
        Debug.Log("Benchmark");
        int round = 10;

        Debug.Log("========= 测试 热更新后未改变的函数 Test1");
        for (int i = 0; i < round; i++)
        {
            BenchmarkTest1(i);
        }

        Debug.Log("========= 测试 热更新后改变的函数 Test2");
        for (int i = 0; i < round; i++)
        {
            BenchmarkTest2(i);
        }
    }

    /// <summary>
    /// 此函数热更新前后未发生变化，走AOT编译
    /// </summary>
    /// <param name="round"></param>
    /// <returns></returns>
    public static int BenchmarkTest1(int round)
    {
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        int n = 0;
        for (int i = 0; i < 10000000; i++)
        {
            n = n * round + 1;
        }
        sw.Stop();
        Debug.Log($"Test1 [{round}]: cost {sw.ElapsedMilliseconds}ms");
        return n;
    }

    /// <summary>
    /// 此函数热更新前后发生变化，走解释执行
    /// </summary>
    /// <param name="round"></param>
    /// <returns></returns>
    public static int BenchmarkTest2(int round)
    {
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        int n = 0;
        // 热更新后，此处代码改成从1开始，函数发生变化，走解释执行
        for (int i = 1; i < 10000000; i++)
        //for (int i = 0; i < 10000000; i++)
        {
            n = n * round + 1;
        }
        sw.Stop();
        Debug.Log($"Test1 [{round}]: cost {sw.ElapsedMilliseconds}ms");
        return n;
    }
}