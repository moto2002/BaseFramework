using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GenericLib
{
	public class PrefabPoolManager : MonoSingleton<PrefabPoolManager>
	{
		private Dictionary<string, PrefabPool> m_prefabPools = new Dictionary<string, PrefabPool>();
		
		public void RegisterPool (string prefabId, PrefabPool pool)
		{
			m_prefabPools.Add (prefabId, pool);
		}
		
		public GameObject Spawn (GameObject prefab, Vector3 pos, Quaternion rot)
		{
			PrefabPool pool = GetPool (prefab);
			
			// TODO : When spawning a prefab, create a new pool if pool == null
			if (pool == null) return null;
			
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
		
		public void Despawn (GameObject prefabInstance)
		{
			PrefabPool pool = GetPool (prefabInstance);
			if (pool == null) return;
			
			pool.ReturnToPool (prefabInstance);
		}
		
		PrefabPool GetPool(GameObject prefabType)
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
				string id = prefabType.name.Substring(0, i);
				Debug.Log (id);
				
				return id;
			}
		}
	}
}