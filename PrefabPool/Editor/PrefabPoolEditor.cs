using UnityEngine;
using UnityEditor;
using System.Collections;

namespace BaseFramework
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
			base.OnInspectorGUI ();
			
			PrefabPool pool = target as PrefabPool;
			
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Size of Pool", GUILayout.MaxWidth (146)); 
			pool.Cached = EditorGUILayout.IntField (pool.Cached);
			EditorGUILayout.EndHorizontal ();
	
			if (GUI.changed)
			{
				EditorUtility.SetDirty (target);
			}
		}
	}
}