using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;

[assembly: Preserve]
enum IntEnum : int
{
    A,
    B,
}

public class RefTypes : MonoBehaviour
{
    List<Type> GetTypes()
    {
        return new List<Type>
        {
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GetTypes());
        GameObject.Instantiate<GameObject>(null);
        Instantiate<GameObject>(null, null);
        Instantiate<GameObject>(null, null, false);
        Instantiate<GameObject>(null, new Vector3(), new Quaternion());
        Instantiate<GameObject>(null, new Vector3(), new Quaternion(), null);
    }
    void RefNullable()
    {
        // nullable
        object b = null;
        int? a = 5;
        b = a;
        int d = (int?)b ?? 7;
        int e = (int)b;
        a = d;
        b = a;
        b = Enumerable.Range(0, 1).Reverse().Take(1).TakeWhile(x => true).Skip(1).All(x => true);
        b = new WaitForSeconds(1f);
        b = new WaitForSecondsRealtime(1f);
        b = new WaitForFixedUpdate();
        b = new WaitForEndOfFrame();
        b = new WaitWhile(() => true);
        b = new WaitUntil(() => true);
    }
    void RefContainer()
    {
        //int, long,float,double, IntEnum,object
        List<object> b = new List<object>()
        {
            new Dictionary<int, long>(),
            new Dictionary<int, float>(),
            new Dictionary<int, double>(),
            new Dictionary<int, object>(),
            new Dictionary<int, IntEnum>(),
            new Dictionary<long, int>(),
            new Dictionary<long, float>(),
            new Dictionary<long, double>(),
            new Dictionary<long, object>(),
            new Dictionary<long, IntEnum>(),
            new Dictionary<float, int>(),
            new Dictionary<float, long>(),
            new Dictionary<float, double>(),
            new Dictionary<float, object>(),
            new Dictionary<float, IntEnum>(),
            new Dictionary<double, int>(),
            new Dictionary<double, long>(),
            new Dictionary<double, float>(),
            new Dictionary<double, object>(),
            new Dictionary<double, IntEnum>(),
            new Dictionary<object, int>(),
            new Dictionary<object, long>(),
            new Dictionary<object, float>(),
            new Dictionary<object, double>(),
            new Dictionary<object, IntEnum>(),
            new Dictionary<IntEnum, int>(),
            new Dictionary<IntEnum, long>(),
            new Dictionary<IntEnum, float>(),
            new Dictionary<IntEnum, double>(),
            new Dictionary<IntEnum, object>(),
            new Dictionary<(int, long), object>(),
            new Dictionary<(int, float), object>(),
            new Dictionary<(int, double), object>(),
            new Dictionary<(int, object), object>(),
            new Dictionary<(int, IntEnum), object>(),
            new Dictionary<(long, int), object>(),
            new Dictionary<(long, float), object>(),
            new Dictionary<(long, double), object>(),
            new Dictionary<(long, object), object>(),
            new Dictionary<(long, IntEnum), object>(),
            new Dictionary<(float, int), object>(),
            new Dictionary<(float, long), object>(),
            new Dictionary<(float, double), object>(),
            new Dictionary<(float, object), object>(),
            new Dictionary<(float, IntEnum), object>(),
            new Dictionary<(double, int), object>(),
            new Dictionary<(double, long), object>(),
            new Dictionary<(double, float), object>(),
            new Dictionary<(double, object), object>(),
            new Dictionary<(double, IntEnum), object>(),
            new Dictionary<(object, int), object>(),
            new Dictionary<(object, long), object>(),
            new Dictionary<(object, float), object>(),
            new Dictionary<(object, double), object>(),
            new Dictionary<(object, IntEnum), object>(),
            new Dictionary<(IntEnum, int), object>(),
            new Dictionary<(IntEnum, long), object>(),
            new Dictionary<(IntEnum, float), object>(),
            new Dictionary<(IntEnum, double), object>(),
            new Dictionary<(IntEnum, object), object>(),
            new Dictionary<(int, long, float), object>(),
            new Dictionary<(int, long, double), object>(),
            new Dictionary<(int, long, object), object>(),
            new Dictionary<(int, long, IntEnum), object>(),
            new Dictionary<(int, float, long), object>(),
            new Dictionary<(int, float, double), object>(),
            new Dictionary<(int, float, object), object>(),
            new Dictionary<(int, float, IntEnum), object>(),
            new Dictionary<(int, double, long), object>(),
            new Dictionary<(int, double, float), object>(),
            new Dictionary<(int, double, object), object>(),
            new Dictionary<(int, double, IntEnum), object>(),
            new Dictionary<(int, object, long), object>(),
            new Dictionary<(int, object, float), object>(),
            new Dictionary<(int, object, double), object>(),
            new Dictionary<(int, object, IntEnum), object>(),
            new Dictionary<(int, IntEnum, long), object>(),
            new Dictionary<(int, IntEnum, float), object>(),
            new Dictionary<(int, IntEnum, double), object>(),
            new Dictionary<(int, IntEnum, object), object>(),
            new Dictionary<(long, int, float), object>(),
            new Dictionary<(long, int, double), object>(),
            new Dictionary<(long, int, object), object>(),
            new Dictionary<(long, int, IntEnum), object>(),
            new Dictionary<(long, float, int), object>(),
            new Dictionary<(long, float, double), object>(),
            new Dictionary<(long, float, object), object>(),
            new Dictionary<(long, float, IntEnum), object>(),
            new Dictionary<(long, double, int), object>(),
            new Dictionary<(long, double, float), object>(),
            new Dictionary<(long, double, object), object>(),
            new Dictionary<(long, double, IntEnum), object>(),
            new Dictionary<(long, object, int), object>(),
            new Dictionary<(long, object, float), object>(),
            new Dictionary<(long, object, double), object>(),
            new Dictionary<(long, object, IntEnum), object>(),
            new Dictionary<(long, IntEnum, int), object>(),
            new Dictionary<(long, IntEnum, float), object>(),
            new Dictionary<(long, IntEnum, double), object>(),
            new Dictionary<(long, IntEnum, object), object>(),
            new Dictionary<(float, int, long), object>(),
            new Dictionary<(float, int, double), object>(),
            new Dictionary<(float, int, object), object>(),
            new Dictionary<(float, int, IntEnum), object>(),
            new Dictionary<(float, long, int), object>(),
            new Dictionary<(float, long, double), object>(),
            new Dictionary<(float, long, object), object>(),
            new Dictionary<(float, long, IntEnum), object>(),
            new Dictionary<(float, double, int), object>(),
            new Dictionary<(float, double, long), object>(),
            new Dictionary<(float, double, object), object>(),
            new Dictionary<(float, double, IntEnum), object>(),
            new Dictionary<(float, object, int), object>(),
            new Dictionary<(float, object, long), object>(),
            new Dictionary<(float, object, double), object>(),
            new Dictionary<(float, object, IntEnum), object>(),
            new Dictionary<(float, IntEnum, int), object>(),
            new Dictionary<(float, IntEnum, long), object>(),
            new Dictionary<(float, IntEnum, double), object>(),
            new Dictionary<(float, IntEnum, object), object>(),
            new Dictionary<(double, int, long), object>(),
            new Dictionary<(double, int, float), object>(),
            new Dictionary<(double, int, object), object>(),
            new Dictionary<(double, int, IntEnum), object>(),
            new Dictionary<(double, long, int), object>(),
            new Dictionary<(double, long, float), object>(),
            new Dictionary<(double, long, object), object>(),
            new Dictionary<(double, long, IntEnum), object>(),
            new Dictionary<(double, float, int), object>(),
            new Dictionary<(double, float, long), object>(),
            new Dictionary<(double, float, object), object>(),
            new Dictionary<(double, float, IntEnum), object>(),
            new Dictionary<(double, object, int), object>(),
            new Dictionary<(double, object, long), object>(),
            new Dictionary<(double, object, float), object>(),
            new Dictionary<(double, object, IntEnum), object>(),
            new Dictionary<(double, IntEnum, int), object>(),
            new Dictionary<(double, IntEnum, long), object>(),
            new Dictionary<(double, IntEnum, float), object>(),
            new Dictionary<(double, IntEnum, object), object>(),
            new Dictionary<(object, int, long), object>(),
            new Dictionary<(object, int, float), object>(),
            new Dictionary<(object, int, double), object>(),
            new Dictionary<(object, int, IntEnum), object>(),
            new Dictionary<(object, long, int), object>(),
            new Dictionary<(object, long, float), object>(),
            new Dictionary<(object, long, double), object>(),
            new Dictionary<(object, long, IntEnum), object>(),
            new Dictionary<(object, float, int), object>(),
            new Dictionary<(object, float, long), object>(),
            new Dictionary<(object, float, double), object>(),
            new Dictionary<(object, float, IntEnum), object>(),
            new Dictionary<(object, double, int), object>(),
            new Dictionary<(object, double, long), object>(),
            new Dictionary<(object, double, float), object>(),
            new Dictionary<(object, double, IntEnum), object>(),
            new Dictionary<(object, IntEnum, int), object>(),
            new Dictionary<(object, IntEnum, long), object>(),
            new Dictionary<(object, IntEnum, float), object>(),
            new Dictionary<(object, IntEnum, double), object>(),
            new Dictionary<(IntEnum, int, long), object>(),
            new Dictionary<(IntEnum, int, float), object>(),
            new Dictionary<(IntEnum, int, double), object>(),
            new Dictionary<(IntEnum, int, object), object>(),
            new Dictionary<(IntEnum, long, int), object>(),
            new Dictionary<(IntEnum, long, float), object>(),
            new Dictionary<(IntEnum, long, double), object>(),
            new Dictionary<(IntEnum, long, object), object>(),
            new Dictionary<(IntEnum, float, int), object>(),
            new Dictionary<(IntEnum, float, long), object>(),
            new Dictionary<(IntEnum, float, double), object>(),
            new Dictionary<(IntEnum, float, object), object>(),
            new Dictionary<(IntEnum, double, int), object>(),
            new Dictionary<(IntEnum, double, long), object>(),
            new Dictionary<(IntEnum, double, float), object>(),
            new Dictionary<(IntEnum, double, object), object>(),
            new Dictionary<(IntEnum, object, int), object>(),
            new Dictionary<(IntEnum, object, long), object>(),
            new Dictionary<(IntEnum, object, float), object>(),
            new Dictionary<(IntEnum, object, double), object>(),
            new SortedDictionary<int, long>(),
            new SortedDictionary<int, float>(),
            new SortedDictionary<int, double>(),
            new SortedDictionary<int, object>(),
            new SortedDictionary<int, IntEnum>(),
            new SortedDictionary<long, int>(),
            new SortedDictionary<long, float>(),
            new SortedDictionary<long, double>(),
            new SortedDictionary<long, object>(),
            new SortedDictionary<long, IntEnum>(),
            new SortedDictionary<float, int>(),
            new SortedDictionary<float, long>(),
            new SortedDictionary<float, double>(),
            new SortedDictionary<float, object>(),
            new SortedDictionary<float, IntEnum>(),
            new SortedDictionary<double, int>(),
            new SortedDictionary<double, long>(),
            new SortedDictionary<double, float>(),
            new SortedDictionary<double, object>(),
            new SortedDictionary<double, IntEnum>(),
            new SortedDictionary<object, int>(),
            new SortedDictionary<object, long>(),
            new SortedDictionary<object, float>(),
            new SortedDictionary<object, double>(),
            new SortedDictionary<object, IntEnum>(),
            new SortedDictionary<IntEnum, int>(),
            new SortedDictionary<IntEnum, long>(),
            new SortedDictionary<IntEnum, float>(),
            new SortedDictionary<IntEnum, double>(),
            new SortedDictionary<IntEnum, object>(),
            new HashSet<int>(),
            new HashSet<long>(),
            new HashSet<float>(),
            new HashSet<double>(),
            new HashSet<object>(),
            new HashSet<IntEnum>(),
            new List<int>(),
            new List<long>(),
            new List<float>(),
            new List<double>(),
            new List<object>(),
            new List<IntEnum>(),
            new ValueTuple<int>(1),
            new ValueTuple<long>(1),
            new ValueTuple<float>(1f),
            new ValueTuple<double>(1),
            new ValueTuple<object>(null),
            new ValueTuple<IntEnum>(IntEnum.A),
            new ValueTuple<int, long>(1, 1),
            new ValueTuple<int, float>(1, 1f),
            new ValueTuple<int, double>(1, 1),
            new ValueTuple<int, object>(1, null),
            new ValueTuple<int, IntEnum>(1, IntEnum.A),
            new ValueTuple<long, int>(1, 1),
            new ValueTuple<long, float>(1, 1f),
            new ValueTuple<long, double>(1, 1),
            new ValueTuple<long, object>(1, null),
            new ValueTuple<long, IntEnum>(1, IntEnum.A),
            new ValueTuple<float, int>(1f, 1),
            new ValueTuple<float, long>(1f, 1),
            new ValueTuple<float, double>(1f, 1),
            new ValueTuple<float, object>(1f, null),
            new ValueTuple<float, IntEnum>(1f, IntEnum.A),
            new ValueTuple<double, int>(1, 1),
            new ValueTuple<double, long>(1, 1),
            new ValueTuple<double, float>(1, 1f),
            new ValueTuple<double, object>(1, null),
            new ValueTuple<double, IntEnum>(1, IntEnum.A),
            new ValueTuple<object, int>(null, 1),
            new ValueTuple<object, long>(null, 1),
            new ValueTuple<object, float>(null, 1f),
            new ValueTuple<object, double>(null, 1),
            new ValueTuple<object, IntEnum>(null, IntEnum.A),
            new ValueTuple<IntEnum, int>(IntEnum.A, 1),
            new ValueTuple<IntEnum, long>(IntEnum.A, 1),
            new ValueTuple<IntEnum, float>(IntEnum.A, 1f),
            new ValueTuple<IntEnum, double>(IntEnum.A, 1),
            new ValueTuple<IntEnum, object>(IntEnum.A, null),
            new ValueTuple<int, long, float>(1, 1, 1f),
            new ValueTuple<int, long, double>(1, 1, 1),
            new ValueTuple<int, long, object>(1, 1, null),
            new ValueTuple<int, long, IntEnum>(1, 1, IntEnum.A),
            new ValueTuple<int, float, long>(1, 1f, 1),
            new ValueTuple<int, float, double>(1, 1f, 1),
            new ValueTuple<int, float, object>(1, 1f, null),
            new ValueTuple<int, float, IntEnum>(1, 1f, IntEnum.A),
            new ValueTuple<int, double, long>(1, 1, 1),
            new ValueTuple<int, double, float>(1, 1, 1f),
            new ValueTuple<int, double, object>(1, 1, null),
            new ValueTuple<int, double, IntEnum>(1, 1, IntEnum.A),
            new ValueTuple<int, object, long>(1, null, 1),
            new ValueTuple<int, object, float>(1, null, 1f),
            new ValueTuple<int, object, double>(1, null, 1),
            new ValueTuple<int, object, IntEnum>(1, null, IntEnum.A),
            new ValueTuple<int, IntEnum, long>(1, IntEnum.A, 1),
            new ValueTuple<int, IntEnum, float>(1, IntEnum.A, 1f),
            new ValueTuple<int, IntEnum, double>(1, IntEnum.A, 1),
            new ValueTuple<int, IntEnum, object>(1, IntEnum.A, null),
            new ValueTuple<long, int, float>(1, 1, 1f),
            new ValueTuple<long, int, double>(1, 1, 1),
            new ValueTuple<long, int, object>(1, 1, null),
            new ValueTuple<long, int, IntEnum>(1, 1, IntEnum.A),
            new ValueTuple<long, float, int>(1, 1f, 1),
            new ValueTuple<long, float, double>(1, 1f, 1),
            new ValueTuple<long, float, object>(1, 1f, null),
            new ValueTuple<long, float, IntEnum>(1, 1f, IntEnum.A),
            new ValueTuple<long, double, int>(1, 1, 1),
            new ValueTuple<long, double, float>(1, 1, 1f),
            new ValueTuple<long, double, object>(1, 1, null),
            new ValueTuple<long, double, IntEnum>(1, 1, IntEnum.A),
            new ValueTuple<long, object, int>(1, null, 1),
            new ValueTuple<long, object, float>(1, null, 1f),
            new ValueTuple<long, object, double>(1, null, 1),
            new ValueTuple<long, object, IntEnum>(1, null, IntEnum.A),
            new ValueTuple<long, IntEnum, int>(1, IntEnum.A, 1),
            new ValueTuple<long, IntEnum, float>(1, IntEnum.A, 1f),
            new ValueTuple<long, IntEnum, double>(1, IntEnum.A, 1),
            new ValueTuple<long, IntEnum, object>(1, IntEnum.A, null),
            new ValueTuple<float, int, long>(1f, 1, 1),
            new ValueTuple<float, int, double>(1f, 1, 1),
            new ValueTuple<float, int, object>(1f, 1, null),
            new ValueTuple<float, int, IntEnum>(1f, 1, IntEnum.A),
            new ValueTuple<float, long, int>(1f, 1, 1),
            new ValueTuple<float, long, double>(1f, 1, 1),
            new ValueTuple<float, long, object>(1f, 1, null),
            new ValueTuple<float, long, IntEnum>(1f, 1, IntEnum.A),
            new ValueTuple<float, double, int>(1f, 1, 1),
            new ValueTuple<float, double, long>(1f, 1, 1),
            new ValueTuple<float, double, object>(1f, 1, null),
            new ValueTuple<float, double, IntEnum>(1f, 1, IntEnum.A),
            new ValueTuple<float, object, int>(1f, null, 1),
            new ValueTuple<float, object, long>(1f, null, 1),
            new ValueTuple<float, object, double>(1f, null, 1),
            new ValueTuple<float, object, IntEnum>(1f, null, IntEnum.A),
            new ValueTuple<float, IntEnum, int>(1f, IntEnum.A, 1),
            new ValueTuple<float, IntEnum, long>(1f, IntEnum.A, 1),
            new ValueTuple<float, IntEnum, double>(1f, IntEnum.A, 1),
            new ValueTuple<float, IntEnum, object>(1f, IntEnum.A, null),
            new ValueTuple<double, int, long>(1, 1, 1),
            new ValueTuple<double, int, float>(1, 1, 1f),
            new ValueTuple<double, int, object>(1, 1, null),
            new ValueTuple<double, int, IntEnum>(1, 1, IntEnum.A),
            new ValueTuple<double, long, int>(1, 1, 1),
            new ValueTuple<double, long, float>(1, 1, 1f),
            new ValueTuple<double, long, object>(1, 1, null),
            new ValueTuple<double, long, IntEnum>(1, 1, IntEnum.A),
            new ValueTuple<double, float, int>(1, 1f, 1),
            new ValueTuple<double, float, long>(1, 1f, 1),
            new ValueTuple<double, float, object>(1, 1f, null),
            new ValueTuple<double, float, IntEnum>(1, 1f, IntEnum.A),
            new ValueTuple<double, object, int>(1, null, 1),
            new ValueTuple<double, object, long>(1, null, 1),
            new ValueTuple<double, object, float>(1, null, 1f),
            new ValueTuple<double, object, IntEnum>(1, null, IntEnum.A),
            new ValueTuple<double, IntEnum, int>(1, IntEnum.A, 1),
            new ValueTuple<double, IntEnum, long>(1, IntEnum.A, 1),
            new ValueTuple<double, IntEnum, float>(1, IntEnum.A, 1f),
            new ValueTuple<double, IntEnum, object>(1, IntEnum.A, null),
            new ValueTuple<object, int, long>(null, 1, 1),
            new ValueTuple<object, int, float>(null, 1, 1f),
            new ValueTuple<object, int, double>(null, 1, 1),
            new ValueTuple<object, int, IntEnum>(null, 1, IntEnum.A),
            new ValueTuple<object, long, int>(null, 1, 1),
            new ValueTuple<object, long, float>(null, 1, 1f),
            new ValueTuple<object, long, double>(null, 1, 1),
            new ValueTuple<object, long, IntEnum>(null, 1, IntEnum.A),
            new ValueTuple<object, float, int>(null, 1f, 1),
            new ValueTuple<object, float, long>(null, 1f, 1),
            new ValueTuple<object, float, double>(null, 1f, 1),
            new ValueTuple<object, float, IntEnum>(null, 1f, IntEnum.A),
            new ValueTuple<object, double, int>(null, 1, 1),
            new ValueTuple<object, double, long>(null, 1, 1),
            new ValueTuple<object, double, float>(null, 1, 1f),
            new ValueTuple<object, double, IntEnum>(null, 1, IntEnum.A),
            new ValueTuple<object, IntEnum, int>(null, IntEnum.A, 1),
            new ValueTuple<object, IntEnum, long>(null, IntEnum.A, 1),
            new ValueTuple<object, IntEnum, float>(null, IntEnum.A, 1f),
            new ValueTuple<object, IntEnum, double>(null, IntEnum.A, 1),
            new ValueTuple<IntEnum, int, long>(IntEnum.A, 1, 1),
            new ValueTuple<IntEnum, int, float>(IntEnum.A, 1, 1f),
            new ValueTuple<IntEnum, int, double>(IntEnum.A, 1, 1),
            new ValueTuple<IntEnum, int, object>(IntEnum.A, 1, null),
            new ValueTuple<IntEnum, long, int>(IntEnum.A, 1, 1),
            new ValueTuple<IntEnum, long, float>(IntEnum.A, 1, 1f),
            new ValueTuple<IntEnum, long, double>(IntEnum.A, 1, 1),
            new ValueTuple<IntEnum, long, object>(IntEnum.A, 1, null),
            new ValueTuple<IntEnum, float, int>(IntEnum.A, 1f, 1),
            new ValueTuple<IntEnum, float, long>(IntEnum.A, 1f, 1),
            new ValueTuple<IntEnum, float, double>(IntEnum.A, 1f, 1),
            new ValueTuple<IntEnum, float, object>(IntEnum.A, 1f, null),
            new ValueTuple<IntEnum, double, int>(IntEnum.A, 1, 1),
            new ValueTuple<IntEnum, double, long>(IntEnum.A, 1, 1),
            new ValueTuple<IntEnum, double, float>(IntEnum.A, 1, 1f),
            new ValueTuple<IntEnum, double, object>(IntEnum.A, 1, null),
            new ValueTuple<IntEnum, object, int>(IntEnum.A, null, 1),
            new ValueTuple<IntEnum, object, long>(IntEnum.A, null, 1),
            new ValueTuple<IntEnum, object, float>(IntEnum.A, null, 1f),
            new ValueTuple<IntEnum, object, double>(IntEnum.A, null, 1)
        };
    }

    class RefStateMachine : IAsyncStateMachine
    {
        public void MoveNext()
        {
            throw new NotImplementedException();
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            throw new NotImplementedException();
        }
    }

    void RefAsyncMethod()
    {
        var stateMachine = new RefStateMachine();

        TaskAwaiter aw = default;
        var c0 = new AsyncTaskMethodBuilder();
        c0.Start(ref stateMachine);
        c0.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c0.SetException(null);
        c0.SetResult();

        var c1 = new AsyncTaskMethodBuilder();
        c1.Start(ref stateMachine);
        c1.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c1.SetException(null);
        c1.SetResult();

        var c2 = new AsyncTaskMethodBuilder<bool>();
        c2.Start(ref stateMachine);
        c2.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c2.SetException(null);
        c2.SetResult(default);

        var c3 = new AsyncTaskMethodBuilder<int>();
        c3.Start(ref stateMachine);
        c3.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c3.SetException(null);
        c3.SetResult(default);

        var c4 = new AsyncTaskMethodBuilder<long>();
        c4.Start(ref stateMachine);
        c4.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c4.SetException(null);

        var c5 = new AsyncTaskMethodBuilder<float>();
        c5.Start(ref stateMachine);
        c5.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c5.SetException(null);
        c5.SetResult(default);

        var c6 = new AsyncTaskMethodBuilder<double>();
        c6.Start(ref stateMachine);
        c6.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c6.SetException(null);
        c6.SetResult(default);

        var c7 = new AsyncTaskMethodBuilder<object>();
        c7.Start(ref stateMachine);
        c7.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c7.SetException(null);
        c7.SetResult(default);

        var c8 = new AsyncTaskMethodBuilder<IntEnum>();
        c8.Start(ref stateMachine);
        c8.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c8.SetException(null);
        c8.SetResult(default);

        var c9 = new AsyncVoidMethodBuilder();
        var b = AsyncVoidMethodBuilder.Create();
        c9.Start(ref stateMachine);
        c9.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c9.SetException(null);
        c9.SetResult();
        Debug.Log(b);
    }

    // Update is called once per frame
    void Update()
    {
        TestAsync();
    }

    public static int TestAsync()
    {
        var t0 = Task.Run(async () =>
        {
            await Task.Delay(10);
        });
        t0.Wait();
        var task = Task.Run(async () =>
        {
            await Task.Delay(10);
            return 100;
        });
        Debug.Log(task.Result);
        return 0;
    }
}
