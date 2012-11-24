using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GenericLib
{
	//TODO: Implement PrefabPool
	public class PrefabPool : MonoBehaviour
	{
		public string m_name; //TODO : Specify Prefab Pool Name
		public GameObject m_objectPrefab;
		public int m_preLoadAmount = 5;
		
		private PrefabPoolManager m_manager;
		private Queue<GameObject> m_pooledPrefabs;
		private List<GameObject> m_activePrefabs;
		
		void Awake ()
		{
		}
		
		void Start ()
		{
			m_manager = PrefabPoolManager.Instance;
			m_manager.RegisterPool (this, m_objectPrefab);
			
			m_pooledPrefabs = new Queue<GameObject> ();
			m_activePrefabs = new List<GameObject> ();
			
			for (int i=0; i<m_preLoadAmount; i++)
			{
				PoolObject ();
			}
		}
		
		void PoolObject ()
		{
			GameObject go = Instantiate (m_objectPrefab) as GameObject;
			go.SetActive (false);
			go.name = go.name+"."+m_pooledPrefabs.Count;
			go.transform.parent = transform;
			m_pooledPrefabs.Enqueue (go);
		}
		
		public GameObject GetNextActive ()
		{
			// TODO : Pool new object if no object could be found in queue
			GameObject go = m_pooledPrefabs.Dequeue ();
			go.SetActive (true);
			m_activePrefabs.Add (go);
			return go;
		}
		
		public void ReturnToPool (GameObject go)
		{
			m_activePrefabs.Remove (go);
			m_pooledPrefabs.Enqueue (go);
			
			go.SetActive (false);
		}
	}
}