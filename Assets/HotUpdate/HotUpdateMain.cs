using HybridCLR;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class HotUpdateMain : MonoBehaviour
{

    public string text;

    void Start()
    {
        Debug.Log($"[{GetType().FullName}] 这个热更新脚本挂载在prefab上，打包成ab。通过从ab中实例化prefab成功还原");
        Debug.Log($"[{GetType().FullName}] hello, {text}.");

        gameObject.AddComponent<CreateByCode>();

        Debug.Log($"[{GetType().FullName}] =======看到此条日志代表你成功运行了示例项目的热更新代码=======");
#if UNITY_STANDALONE_WIN
        File.WriteAllText("run.log", "ok", System.Text.Encoding.UTF8);
        if (File.Exists("autoexit"))
        {
            Debug.Log("==== 本程序将于5秒后自动退出 ====");
            Task.Run(async () =>
            {
                await Task.Delay(5000);
                Application.Quit(0);
            });
        }
#endif
    }
}
