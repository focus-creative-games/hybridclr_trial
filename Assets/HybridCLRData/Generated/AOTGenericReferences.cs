public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	// UnityEngine.CoreModule.dll
	// mscorlib.dll
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// System.Func<object>
	// }}

	public void RefMethods()
	{
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,HotUpdateMain.<>c.<<Start>b__1_0>d>(System.Runtime.CompilerServices.TaskAwaiter&,HotUpdateMain.<>c.<<Start>b__1_0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<HotUpdateMain.<>c.<<Start>b__1_0>d>(HotUpdateMain.<>c.<<Start>b__1_0>d&)
		// object UnityEngine.GameObject.AddComponent<object>()
	}
}