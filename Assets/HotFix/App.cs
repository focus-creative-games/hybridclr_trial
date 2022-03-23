using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class App
{
    public static int Main()
    {
        Debug.Log("hello,huatuo");

        TestAsync();

        Debug.Log("hello,huatuo 2");
        return 0;
    }

    public static async void TestAsync()
    {
        Debug.Log("async task 1");
        await Task.Delay(10);
        Debug.Log("async task 2");
    }

    public static int Main_1()
    {
        Debug.Log("hello,huatuo");

        var task = Task.Run(async () =>
        {
            await TestAsync2();
        });

        task.Wait();

        Debug.Log("async task end");
        Debug.Log("async task end2");

        return 0;
    }

    public static async Task TestAsync2()
    {
        Debug.Log("async task 1");
        await Task.Delay(3000);
        Debug.Log("async task 2");
    }
}
