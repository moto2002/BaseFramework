using UnityEngine;
using UnityEditor;

namespace BaseFramework.PrefabPool
{
	// TODO: Implement PrefabPoolEditor.
	// Functionality:
	// - Add an option to Assets/Create
	// - Bring up a GUI Screen to Setup a pool
	// - Add a GameObject w/ PrefabPool Component to the Scene
	// - (Optional) Create a prefab of the prefab pool in a specified location?
	[CustomEditor (typeof (PrefabPool))]
	public class PrefabPoolEditor : Editor
	{
		public override void OnInspectorGUI ()
		{	
			PrefabPool pool = target as PrefabPool;
			string prefabType = pool.ObjectPrefab == null ? "None" : pool.ObjectPrefab.name;
			
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Pool Prefab");
			EditorGUILayout.LabelField (prefabType);
			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Size of Pool"); 
			EditorGUILayout.LabelField (pool.Cached.ToString ());
			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Inactive Objects"); 
			EditorGUILayout.LabelField (pool.InactiveObjects.ToString ());
			EditorGUILayout.EndHorizontal ();
		}
	}
}