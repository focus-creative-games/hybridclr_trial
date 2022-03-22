using System.Collections;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class PTest : MonoBehaviour
{

    public static int _V0;

    //AppDomain是ILRuntime的入口，最好是在一个单例类中保存，整个游戏全局就一个，这里为了示例方便，每个例子里面都单独做了一个
    //大家在正式项目中请全局只创建一个AppDomain
    //public AppDomain appdomain;
    bool inited = false;
    TestItem[] testItems;

    public WaitForSeconds ws = new WaitForSeconds(2);
    public string logText = "";
    public int runCount;
    static public int times = 200_000;
    public List<LogData> logdata = new List<LogData>();
    public System.Reflection.Assembly assembly { get => LoadDll.gameAss; }

    void Test1(UnityEngine.Transform t)
    {
        var up = Vector3.up;
        t.Rotate(up, 1);
    }

    void Start()
    {
        runCount = 2;
        Application.logMessageReceived += this.log;
        testItems = new TestItem[17];
        testItems[0] = new TestDllOneParam(this, 0, transform);
        testItems[1] = new TestDllOneParam(this, 1, transform);
        for (int i = 2; i < 10; ++i)
        {
            testItems[i] = new TestDll(this, i);
        }
        testItems[10] = new TestDllOneParam(this, 1, transform);
        testItems[11] = new TestEmptyFunc(this, 11);
        testItems[12] = new TestGetValue(this, 12, "_V0");
        testItems[13] = new TestGetValue(this, 13, "_V1");
        testItems[14] = new TestGetValue(this, 14, "_V2");
        testItems[15] = new TestGetValue(this, 15, "_V3");
        testItems[16] = new TestGetValue(this, 16, "_V4");
    }

    void log(string cond, string trace, LogType lt)
    {
        logText += cond;
        logText += "\n";
    }

    public void GC()
    {
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        print("GC Done!");
    }

    public void saveLog()
    {
        List<string> logStr = new List<string>();
        logStr.Add(LogData.GetHeader());
        foreach (var log in logdata)
        {
            logStr.Add(log.ToString());
        }
        string path = Application.dataPath + "/huatuo_test.log";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        var fs = new FileStream(path, FileMode.CreateNew);
        byte[] byteArray = System.Text.Encoding.Default.GetBytes(string.Join("\n", logStr));
        fs.Write(byteArray, 0, byteArray.Length);
        fs.Close();
    }
    void RunAll()
    {
        for (int i = 0; i < testItems.Length; ++i)
        {
            //testItems[i].Test();
            StartCoroutine(testItems[i].Test());
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(400, 0, 500, 1000), logText);

        if (GUI.Button(new Rect(10, 10, 100, 50), "GC"))
        {
            GC();
        }

        if (GUI.Button(new Rect(130, 10, 100, 50), "Clear Screen"))
        {
            logText = "";
        }
        if (GUI.Button(new Rect(250, 10, 100, 50), "Save log"))
        {
            saveLog();
        }
        if (GUI.Button(new Rect(370, 80, 100, 50), "Run All"))
        {
            RunAll();
        }


        int[] rows = { 10, 110, 210 };
        int[] cols = { 0, 0, 70 };
        int col = 80;
        for (int i = 0; i < testItems.Length; ++i)
        {
            if (GUI.Button(new Rect(rows[i % 3], col, 100, 50), "Test" + i))
            {
                StartCoroutine(testItems[i].Test());
            }

            col += cols[i % 3];
        }
    }
}


