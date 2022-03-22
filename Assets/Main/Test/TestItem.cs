using System.Collections;
using UnityEngine;

public abstract class TestItem
{
    private static int _id;
    protected PTest m_ptest;
    protected int m_index;

    public TestItem(PTest ptest, int index)
    {
        m_ptest = ptest;
        m_index = index;
    }

    public int GenBatchId()
    {
        var ret = _id + 1;
        _id++;
        return ret;
    }
    public abstract IEnumerator Test();
}

public class TestJustForCall
{

}

public struct LogData
{
    public int batchId;
    public string name;
    public int count;
    public int times;
    public double total_cost_ms;
    public int every_count_cost_ms;


    private const int baseTimes = 1_000;

    public static string GetHeader()
    {
        //eturn $"ID\tName\t接口调用次数\t总耗时(ms)\t每{baseTimes}次耗时(ms)";
        return $"ID\tName\t接口调用次数\t总耗时(ms)\t";
    }
    public string ToString()
    {
        return $"{batchId}\t{name}\t{count * times}\t{(int)total_cost_ms}";
    }
}


public class TestDllOneParam : TestItem
{
    string m_hotfixFuncName;
    Transform m_trans;

    public TestDllOneParam(PTest ptest, int index, Transform trans)
        : base(ptest, index)
    {
        m_hotfixFuncName = "Test" + index;
        m_trans = trans;
    }


    public override IEnumerator Test()
    {
        m_ptest.logText += "Test" + m_index + " Begin:\n";
        System.Type type = m_ptest.assembly.GetType("PTest");
        var appType = m_ptest.assembly.GetType("TestFunc");
        var method = appType.GetMethod(m_hotfixFuncName);
        object[] param = new object[1] { m_trans };
        int count = m_ptest.runCount;
        double totalMS = 0;
        int times = 1;
        int batchId = GenBatchId();
        for (int i = 1; i <= count; ++i)
        {
            m_ptest.GC();
            yield return m_ptest.ws;
            double t = (double)method.Invoke(null, param);
            totalMS += t;
            m_ptest.logText += string.Format("{0}: ms: {1}\n", i, t);
        }
        var logdata = new LogData();
        logdata.name = m_hotfixFuncName;
        logdata.batchId = batchId;
        logdata.times = times;
        logdata.count = count;
        logdata.total_cost_ms = (double)totalMS;
        logdata.every_count_cost_ms = (int)totalMS / count;
        m_ptest.logdata.Add(logdata);

        m_ptest.logText += string.Format("Test{0} complete average ms: {1}\n", m_index, totalMS / count);
        yield return m_ptest.ws;
        m_ptest.saveLog();
    }
}
public class TestDll : TestItem
{
    string m_hotfixFuncName;

    public TestDll(PTest ptest, int index)
        : base(ptest, index)
    {
        m_hotfixFuncName = "Test" + index;
    }


    public override IEnumerator Test()
    {
        m_ptest.logText += "Test" + m_index + " Begin:\n";
        System.Type type = m_ptest.assembly.GetType("PTest");
        var appType = m_ptest.assembly.GetType("TestFunc");
        var method = appType.GetMethod(m_hotfixFuncName);
        int count = m_ptest.runCount;
        double totalMS = 0;
        int times = 1;// PTest.times;
        int batchId = GenBatchId();
        for (int i = 1; i <= count; ++i)
        {
            m_ptest.GC();
            yield return m_ptest.ws;
            double t = (double)method.Invoke(null, null);
            totalMS += t;
            m_ptest.logText += string.Format("{0}: ms: {1}\n", i, t);
        }
        var logdata = new LogData();
        logdata.name = m_hotfixFuncName;
        logdata.batchId = batchId;
        logdata.times = times;
        logdata.count = count;
        logdata.total_cost_ms = (double)totalMS;
        logdata.every_count_cost_ms = (int)totalMS / count;
        m_ptest.logdata.Add(logdata);

        m_ptest.logText += string.Format("Test{0} complete average ms: {1}\n", m_index, totalMS / count);

        yield return m_ptest.ws;
        m_ptest.saveLog();
    }
}


public class TestEmptyFunc : TestItem
{
    public TestEmptyFunc(PTest ptest, int index)
        : base(ptest, index)
    {
    }

    public override IEnumerator Test()
    {
        m_ptest.logText += "Test" + m_index + " Begin:\n";
        System.Type type = m_ptest.assembly.GetType("PTest");
        var appType = m_ptest.assembly.GetType("TestFunc");
        var method = appType.GetMethod("EmptyFunc");
        int count = m_ptest.runCount;
        double totalMS = 0;
        int times = PTest.times;
        int batchId = GenBatchId();
        for (int i = 1; i <= count; ++i)
        {
            m_ptest.GC();
            yield return m_ptest.ws;

            long ts = System.DateTime.Now.Ticks;
            for (int j = 0; j < times; ++j)
            {
                method.Invoke(null, null);
            }
            double t = (double)((System.DateTime.Now.Ticks - ts) / 10000.0);

            totalMS += t;
            m_ptest.logText += string.Format("{0}: ms: {1}\n", i, t);
        }
        var logdata = new LogData();
        logdata.name = nameof(TestEmptyFunc);
        logdata.batchId = batchId;
        logdata.times = times;
        logdata.count = count;
        logdata.total_cost_ms = (double)totalMS;
        logdata.every_count_cost_ms = (int)totalMS / count;
        m_ptest.logdata.Add(logdata);

        m_ptest.logText += string.Format("Test{0} complete average ms: {1}\n", m_index, totalMS / count);
        yield return m_ptest.ws;
        m_ptest.saveLog();
    }
}


public class TestGetValue : TestItem
{
    string m_valueName;
    string m_methodName;
    object m_value;

    public TestGetValue(PTest ptest, int index, string valueName)
        : base(ptest, index)
    {
        m_valueName = valueName;
        m_methodName = $"Get{valueName}";
    }

    public override IEnumerator Test()
    {
        m_ptest.logText += "Test" + m_index + " Begin:\n";


        int count = m_ptest.runCount;
        double totalMS = 0;
        int times = PTest.times;
        int batchId = GenBatchId();
        for (int i = 1; i <= count; ++i)
        {
            m_ptest.GC();
            yield return m_ptest.ws;

            var appType = m_ptest.assembly.GetType("TestFunc");
            long ts = System.DateTime.Now.Ticks;

            for (int j = 0; j < times; ++j)
            {
                var method = appType.GetMethod(m_methodName);
                m_value = method.Invoke(null, null);
            }
            double t = (double)((System.DateTime.Now.Ticks - ts) / 10000.0);
            totalMS += t;
            m_ptest.logText += string.Format("{0}: ms: {1}\n", i, t);
        }
        var logdata = new LogData();
        logdata.name = m_methodName;
        logdata.batchId = batchId;
        logdata.times = times;
        logdata.count = count;
        logdata.total_cost_ms = (double)totalMS;
        m_ptest.logdata.Add(logdata);

        m_ptest.logText += string.Format("Test{0} complete average ms: {1}\n", m_index, totalMS / count);
        yield return m_ptest.ws;
        m_ptest.saveLog();
    }
}
