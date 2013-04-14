using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace BaseFramework
{
	[CustomEditor (typeof (PrefabPoolManager))]
	public class PrefabPoolManagerEditor : Editor
	{
		private PrefabPoolManager m_manager;
		private GameObject m_newPoolObject;
		
		public override void OnInspectorGUI ()
		{	
			// List all pools
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Pool Name");
			EditorGUILayout.LabelField ("Object Type");
			EditorGUILayout.LabelField ("Size");
			EditorGUILayout.EndHorizontal ();
			
			m_manager = ((PrefabPoolManager)target);
			PrefabPool[] pools = m_manager.GetAllPools ();
			
			int poolToRemove = -1;
			for (int i=0; i<pools.Length; i++)
			{
				PrefabPool p = pools[i];
				
				EditorGUILayout.BeginHorizontal ();
				p.name = EditorGUILayout.TextField (p.name);
				p.ObjectPrefab = EditorGUILayout.ObjectField (p.ObjectPrefab, typeof (GameObject), false) as GameObject;
				p.Cached = EditorGUILayout.IntField (p.Cached);
				if (GUILayout.Button ("-"))
				{
					poolToRemove = i;
				}
				
				EditorGUILayout.EndHorizontal ();
			}
			
			if (poolToRemove >= 0 && poolToRemove < pools.Length)
			{
				m_manager.RemovePool (pools[poolToRemove].ObjectPrefab);
			}
			
			EditorGUILayout.BeginHorizontal ();
			m_newPoolObject = EditorGUILayout.ObjectField ("Create new pool", m_newPoolObject, typeof (GameObject), false) as GameObject;
			if (GUILayout.Button ("+", GUILayout.Width (32)) && m_newPoolObject != null)
			{
				m_manager.CreatePool (m_newPoolObject);
				m_newPoolObject = null;
				
				EditorUtility.SetDirty (m_manager);
				foreach (PrefabPool pool in m_manager.GetAllPools())
				{
					EditorUtility.SetDirty (pool);
				}
			}
			EditorGUILayout.EndHorizontal ();
		}
	}
}