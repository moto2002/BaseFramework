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
		public void OnEnable()
		{
			m_pxPrefabPool = (PrefabPool)target;
		}
		
		public override void OnInspectorGUI ()
		{	
			GameObject pxPrefab = m_pxPrefabPool.ObjectPrefab;
			string pxPrefabName = "None";
			if ( pxPrefab != null )
			{
				pxPrefabName = m_pxPrefabPool.ObjectPrefab.name;
			}
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel( "Pool Prefab" );
			EditorGUILayout.LabelField( pxPrefabName );
			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel( "Size of Pool" );
			
			int iNumberOfObjectsCached = m_pxPrefabPool.Cached;
			EditorGUILayout.LabelField( iNumberOfObjectsCached.ToString() );
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel( "Inactive Objects" );
			
			int iNumberOfInactiveObjects = m_pxPrefabPool.InactiveObjects;
			EditorGUILayout.LabelField( iNumberOfInactiveObjects.ToString() );
			EditorGUILayout.EndHorizontal();
		}
		
		PrefabPool m_pxPrefabPool;
	}
}