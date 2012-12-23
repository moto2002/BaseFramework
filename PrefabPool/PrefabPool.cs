using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BaseFramework
{
	//TODO: Implement PrefabPool
	public class PrefabPool : MonoBehaviour
	{
		public string m_name; //TODO : Specify Prefab Pool Name
		public GameObject m_objectPrefab;
		
		private int m_cacheLimit = 0;
		
		private PrefabPoolManager m_manager;
		private Queue<GameObject> m_pooledPrefabs;
		private List<GameObject> m_activePrefabs;
		
		public int Cached
		{
			get { return m_cacheLimit; }
			set { Recache (value); }
		}
		
		bool Initialised
		{
			get { return m_manager == null || m_pooledPrefabs == null || m_activePrefabs == null || m_cacheLimit == 0; }
		}
		
		void Awake ()
		{
		}
		
		void Initialise ()
		{
			m_manager = PrefabPoolManager.Instance;
			m_manager.RegisterPool (m_objectPrefab.name, this);
			
			m_pooledPrefabs = new Queue<GameObject> ();
			m_activePrefabs = new List<GameObject> ();
			
			Recache (4);
		}
		
		void PoolObject ()
		{
			if (!Initialised)
			{
				Initialise ();
			}
			
			GameObject go = Instantiate (m_objectPrefab) as GameObject;
			go.SetActive (false);
			go.name = go.name+"."+m_pooledPrefabs.Count;
			go.transform.parent = transform;
			m_pooledPrefabs.Enqueue (go);
		}
		
		void Recache (int newSize) // TODO : Recache should be able to lower the number of prefabs in memory too!!
		{
			// How many objects to we need to Instantiate / Destroy
			int diff = newSize - m_cacheLimit;
			
			// Do the Instantions / Destroys
			if (diff < 0)
			{
				Debug.LogWarning ("PrefabPools cannot recache to less objects");
				return;
			}
			
			for (int i = 0; i<diff; i++)
			{
				PoolObject ();
			}
			
			// Set m_cacheLimit to newSize
			m_cacheLimit = newSize;
		}
		
		public GameObject GetNextActive ()
		{
			if (!Initialised)
			{
				Initialise ();
			}
			
			// TODO : Pool new object if no object could be found in queue (increase pool size)
			if (m_pooledPrefabs.Count == 0)
			{
				Debug.LogError ("[Prefab Pool] No more GameObjects pooled!!");
				return null;
			}
			
			GameObject go = m_pooledPrefabs.Dequeue ();
			go.SetActive (true);
			m_activePrefabs.Add (go);
			return go;
		}
		
		public void ReturnToPool (GameObject go)
		{
			if (!Initialised)
			{
				Initialise ();
			}
			
			m_activePrefabs.Remove (go);
			m_pooledPrefabs.Enqueue (go);
			
			go.SetActive (false);
		}
		
	}
}