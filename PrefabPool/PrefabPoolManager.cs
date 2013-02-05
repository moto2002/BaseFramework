using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseFramework
{
	[ExecuteInEditMode ()]
	public class PrefabPoolManager : MonoSingleton<PrefabPoolManager>
	{
		// Choosing a data type to store references to the prefab pools:
		// - NO dupilcates
		// - Don't need fast inserts / removals as pools are not set up at run time.
		// - We could index or iterate. indexed would be quicker at runtime though?
		private Dictionary<String, PrefabPool> m_prefabPools;
		
		[SerializeField]
		private List<String> m_prefabKeys = new List<String>();
		
		[SerializeField]
		private List<PrefabPool> m_prefabValues = new List<PrefabPool>();
		
		public void CreatePool (GameObject prefabType)
		{
			UpdateDictionary ();
			
			string id = GetPrefabID (prefabType);
			if (!m_prefabPools.ContainsKey (id))
			{
				m_prefabKeys.Add (id);
				m_prefabValues.Add (NewPool (prefabType));
				
				UpdateDictionary ();
			}
		}
		
		public PrefabPool[] GetAllPools ()
		{
			UpdateDictionary ();
			
			PrefabPool[] pools = new PrefabPool[m_prefabPools.Count];
			m_prefabPools.Values.CopyTo (pools, 0);
			return pools;
		}
		
		public PrefabPool GetPool (GameObject prefabType)
		{
			UpdateDictionary ();
			
			string id = GetPrefabID (prefabType);
			if (m_prefabPools.ContainsKey (id))
			{
				return m_prefabPools[id];
			}
			
			return null;
		}
		
		public void RemovePool (GameObject prefabType)
		{
			UpdateDictionary ();
			
			string id = GetPrefabID (prefabType);
			if (m_prefabPools.ContainsKey (id))
			{
				DestroyImmediate (m_prefabPools[id].gameObject);
				
				m_prefabKeys.Remove (id);
				m_prefabValues.Remove (NewPool (prefabType));
				
				UpdateDictionary ();
			}
		}
		
		#region Private Memebers
		
		private void Awake ()
		{
			UpdateDictionary();
		}
		
		private void UpdateDictionary ()
		{
			m_prefabPools = new Dictionary<string, PrefabPool>();
			
			if (m_prefabKeys.Count != m_prefabValues.Count)
 			{
				Debug.LogError ("Keys and Values are out of sync!! (keys: "+m_prefabKeys.Count+", values: "+m_prefabValues.Count+")");
				m_prefabKeys = new List<string>();
				m_prefabValues = new List<PrefabPool>();
				return;
			}
			
			for (int i=0; i<m_prefabKeys.Count; i++)
			{
				m_prefabPools.Add (m_prefabKeys[i], m_prefabValues[i]);
			}
		}
		
		// TODO : Better Generation of PrefabId's ??
		private String GetPrefabID (GameObject prefabType)
		{
			if (prefabType == null)
			{
				//Debug.LogError ("No prefabType specified!");
				return null;
			}
			
			int i = prefabType.name.IndexOf ('(');
			
			if (i < 0)
			{
				//Debug.Log ("PrefabID : "+prefabType.name);
				return prefabType.name;
			}
			else
			{
				//Debug.Log ("PrefabID : "+prefabType.name.Substring (0, i));
				return prefabType.name.Substring (0, i);
			}
		}
		
		private PrefabPool NewPool (GameObject prefabType)
		{
			GameObject go = new GameObject();
			go.transform.parent = transform;
			
			PrefabPool pool = go.AddComponent<PrefabPool>();
			pool.ObjectPrefab = prefabType;
			pool.Cached = 5;
			
			return pool;
		}
		
		void Update () // todo : optimize and do not execute in play mode
		{
			UpdateDictionary ();
			
			foreach (KeyValuePair<String, PrefabPool> prefabEntry in m_prefabPools)
			{
				if (prefabEntry.Value == null)
				{
					m_prefabPools.Remove (prefabEntry.Key);
				}
				else if (prefabEntry.Value.ObjectPrefab == null)
				{
					DestroyImmediate (prefabEntry.Value.gameObject);
					m_prefabPools.Remove (prefabEntry.Key);
				}
			}
			
			foreach (Transform t in transform)
			{
				PrefabPool pool = t.GetComponent<PrefabPool>();
				
				if (pool != null)
				{
					if (m_prefabPools.ContainsValue (pool))
					{
						continue;
					}
					
					if (pool.ObjectPrefab != null)
					{
						string id = GetPrefabID (pool.ObjectPrefab);
						m_prefabPools.Add (id, pool);
					}
					else
					{
						DestroyImmediate (t.gameObject);
					}	
				}
				else
				{
					DestroyImmediate (t.gameObject);
				}
			}
		}
		
		#endregion
	}
}