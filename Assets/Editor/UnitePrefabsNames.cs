using UnityEngine;
using UnityEditor;

public class UnitePrefabsNames : MonoBehaviour
{
	[MenuItem("Tools/Unite Prefabs Names")] //Place the Set Pivot menu item in the GameObject menu
	static void UnitePrefabNames()
	{
		GameObject[] objectArray = Selection.gameObjects;

		// Loop through every GameObject in the array above
		foreach (GameObject gameObject in objectArray)
		{
			Object prefabAsset = PrefabUtility.GetPrefabParent(gameObject);
			if(prefabAsset) gameObject.name = prefabAsset.name;
		}
	}
	[MenuItem("Tools/Replace With First Prefab")] //Place the Set Pivot menu item in the GameObject menu
	static void ReplaceWithFirstPrefab()
	{
		GameObject[] objectArray = Selection.gameObjects;
		int index = 0;
		Object _prefab = null;
		for (int i = 0; i < objectArray.Length; i++)
		{
			_prefab = PrefabUtility.GetPrefabParent(objectArray[i]);
			if(_prefab != null)
            {
				index = i;
				break;
            }
		}
        for (int i = 0; i < objectArray.Length; i++)
        {
			if (index == i) continue;
			Transform t = (PrefabUtility.InstantiatePrefab(_prefab) as GameObject).transform;
			t.position = objectArray[i].transform.position;
			t.rotation = objectArray[i].transform.rotation;
			t.parent = objectArray[i].transform.parent;
			DestroyImmediate(objectArray[i]);
        }
	}
}
