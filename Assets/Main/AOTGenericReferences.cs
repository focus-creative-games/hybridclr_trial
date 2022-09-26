public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ constraint implement type
	// }} 

	// {{ AOT generic type
	//System.Collections.Generic.Dictionary`2<System.Int32,System.Object>
	//System.Collections.Generic.Dictionary`2<UnityEngine.Vector3,System.UInt64>
	//System.Collections.Generic.Dictionary`2<UnityEngine.Vector3,System.UInt16>
	//System.Collections.Generic.Queue`1<System.Single>
	//System.Collections.Generic.Queue`1<System.Double>
	// }}

	public void RefMethods()
	{
		// UnityEngine.Vector2[] System.Array::Empty<UnityEngine.Vector2>()
		// System.Object UnityEngine.AssetBundle::LoadAsset<System.Object>(System.String)
		// System.Object UnityEngine.GameObject::AddComponent<System.Object>()
	}
}