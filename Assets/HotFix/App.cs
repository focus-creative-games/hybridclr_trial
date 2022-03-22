using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class App
{
    public static int Main()
    {
        Debug.Log("hello,huatuo");

        var task = Task.Run(async () =>
        {
            await TestAsync();
        });

        task.Wait();

        Debug.Log("async task end");
        Debug.Log("async task end2");

        return 0;
    }

    public static async Task TestAsync()
    {
        Debug.Log("async task 1");
        await Task.Delay(3000);
        Debug.Log("async task 2");
    }
}
