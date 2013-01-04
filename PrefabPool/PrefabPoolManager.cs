using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseFramework
{
	[ExecuteInEditMode ()]
	public class PrefabPoolManager : MonoSingleton<PrefabPoolManager>
	{
		private Dictionary<string, PrefabPool> m_prefabPools = new Dictionary<string, PrefabPool>(); // todo: index by type, not string.
		
		public void RegisterPool (string objectType, PrefabPool pool)
		{
			if (!m_prefabPools.ContainsKey (objectType))
			{
				m_prefabPools.Add (objectType, pool);
			}
			else
			{
				Debug.LogWarning ("PoolManager already contains a reference to this type!", this);
			}
		}
		
		public GameObject Spawn (GameObject prefab)
		{
			return Spawn (prefab, Vector3.zero, Quaternion.identity);
		}
		
		public GameObject Spawn (GameObject prefab, Vector3 pos, Quaternion rot)
		{
			
			PrefabPool pool = GetPool (prefab);
			
			// TODO : When spawning a prefab, create a new pool if pool == null
			if (pool == null)
			{
				GameObject poolObject = new GameObject ("PrefabPool."+prefab.ToString());
				pool = poolObject.AddComponent <PrefabPool> ();
				pool.Cached = 4;
				
				return null;
			}
			
			GameObject go = pool.GetNextActive();
			if (go == null)
			{
				Debug.LogError ("[PrefabPoolManager] Could not Spawn - PrefabPool did not give us a GameObject!");
				return null;
			}
			
			go.transform.position = pos;
			go.transform.rotation = rot;
			
			return go;
		}
		
		public void Queue (GameObject prefab, int amount)
		{
			PrefabPool pool = GetPool (prefab);
			
			if (pool == null)
			{
				GameObject poolObject = new GameObject ("PrefabPool."+prefab.ToString());
				pool = poolObject.AddComponent <PrefabPool> ();
			}
			
			pool.Cached = amount;
		}
		
		public void Despawn (GameObject prefabInstance)
		{
			PrefabPool pool = GetPool (prefabInstance);
			if (pool == null) return;
			
			pool.ReturnToPool (prefabInstance);
		}
		
		PrefabPool GetPool (GameObject prefabType)
		{
			try
			{
				string id = GetPrefabId (prefabType);
				PrefabPool pool = m_prefabPools[id];
				return pool;
			}
			catch (MissingReferenceException e)
			{
				Debug.LogError ("[PrefabPoolManager] No PrefabPool found for "+prefabType.ToString());
				Debug.LogError(e.Message);
				
				return null;
			}
			catch (KeyNotFoundException e)
			{
				Debug.LogError ("[PrefabPoolManager] No Prefab found for "+prefabType.ToString());
				Debug.LogError(e.Message);
				
				return null;
			}
		}
		
		// TODO : Better Generation of PrefabId's ??
		string GetPrefabId (GameObject prefabType)
		{
			int i = prefabType.name.IndexOf('(');
			
			if (i < 0)
			{
				return prefabType.name;
			}
			else
			{
				return prefabType.name.Substring (0, i);
			}
		}
	}
}