using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// }}

	public void RefMethods()
	{
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Entry.<UnloadAssembly>d__1>(System.Runtime.CompilerServices.TaskAwaiter&,Entry.<UnloadAssembly>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Entry.<UnloadAssembly>d__1>(Entry.<UnloadAssembly>d__1&)
	}
}