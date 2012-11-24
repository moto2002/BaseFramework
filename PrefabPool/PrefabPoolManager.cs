using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GenericLib
{
	public class PrefabPoolManager : MonoSingleton<PrefabPoolManager>
	{
		private Dictionary<GameObject, PrefabPool> m_prefabPools = new Dictionary<GameObject, PrefabPool>();
		
		public void RegisterPool(PrefabPool pool, GameObject prefab)
		{
			m_prefabPools.Add (prefab, pool);
		}
		
		public GameObject Spawn (GameObject prefab, Vector3 pos, Quaternion rot)
		{
			PrefabPool pool = GetPool (prefab);
			
			// TODO : When spawning a prefab, create a new pool if pool == null
			if (pool == null) return null;
			
			GameObject go = pool.GetNextActive();
			go.transform.position = pos;
			go.transform.rotation = rot;
			
			return go;
		}
		
		// TODO : Prefab Pool Despawn Function should have just one parameter.
		public void Despawn (GameObject prefabType, GameObject prefabInstance)
		{
			PrefabPool pool = GetPool (prefabType);
			if (pool == null) return;
			
			pool.ReturnToPool (prefabInstance);
		}
		
		PrefabPool GetPool(GameObject prefabType)
		{
			try
			{
				PrefabPool pool = m_prefabPools[prefabType];
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
	}
}