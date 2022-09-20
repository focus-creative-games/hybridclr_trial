public class AOTGenericReferences : UnityEngine.MonoBehaviour
{
	//System.Collections.Generic.Dictionary`2
	//GenericType`1
	//GenericType`1
	//System.Collections.Generic.Queue`1
	//System.Collections.Generic.Queue`1
	//System.Collections.Generic.Dictionary`2
	//System.Collections.Generic.Dictionary`2
	public void RefMethods()
	{
		// System.Void GenericType`1::Show()
		// System.Void GenericType`1::.ctor()
		// System.Void GenericType`1::Show()
		// System.Void GenericType`1::.ctor()
		// T UnityEngine.GameObject::AddComponent<T>()
		// T UnityEngine.Resources::Load<T>(System.String)
		// T UnityEngine.AssetBundle::LoadAsset<T>(System.String)
		// T[] System.Array::Empty<T>()
		// System.Void GenericType`1::Foo<U>()
		// System.Void GenericType`1::Foo<U>()
		// System.Void System.Collections.Generic.Queue`1::.ctor()
		// System.Void System.Object::.ctor()
		// System.Void System.Collections.Generic.Queue`1::.ctor()
		// System.Void System.Collections.Generic.Dictionary`2::.ctor()
		// System.Void System.Collections.Generic.Dictionary`2::.ctor()
	}
}