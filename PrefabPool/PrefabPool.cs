using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BaseFramework
{
	[ExecuteInEditMode ()]
	public class PrefabPool : MonoBehaviour
	{
		public string m_name; //TODO : Specify Prefab Pool Name
		public GameObject m_objectPrefab;
		
		private PrefabPoolManager m_manager;
		
		private List<GameObject> m_pooledObjects = new List<GameObject> ();
		private Queue<GameObject> m_inactiveObjects = new Queue<GameObject> ();
		
		#region Public Properties / Methods
		
		public int Cached
		{
			get { return m_pooledObjects.Count; }
			set { Recache (value); }
		}
		
		public GameObject GetNextActive ()
		{
			// TODO : Pool new object if no object could be found in queue (increase pool size)
			if (m_inactiveObjects.Count == 0)
			{
				Debug.LogError ("[Prefab Pool] No more GameObjects pooled!!");
				return null;
			}
			
			GameObject go = m_inactiveObjects.Dequeue ();
			go.SetActive (true);
			
			return go;
		}
		
		public void ReturnToPool (GameObject go)
		{
			m_inactiveObjects.Enqueue (go);
			go.SetActive (false);
		}
		
		#endregion
		
		#region Monobehaviour Overrides
		
		void Start ()
		{
			m_manager = PrefabPoolManager.Instance;
			m_manager.RegisterPool (m_objectPrefab.name, this);
		}
		
		void OnEnable ()
		{
			for (int i=0; i<transform.childCount; i++)
			{
				GameObject go = transform.GetChild (i).gameObject;
				if (!m_pooledObjects.Contains (go))
				{
					m_pooledObjects.Add (go);
				}
				
				if (!go.activeSelf && !m_inactiveObjects.Contains (go))
				{
					m_inactiveObjects.Enqueue (go);
				}
			}
		}
		
		#endregion
		
		#region Internal Functions
		
		void Recache (int newSize) // TODO : Recache should be able to lower the number of prefabs in memory too!!
		{
			// How many objects to we need to Instantiate / Destroy
			int diff = newSize - m_pooledObjects.Count;
			
			bool addMore = diff > 0;
			diff = Mathf.Abs (diff);
			
			// Do the Instantions / Destroys
			for (int i=0; i<diff; i++)
			{
				if (addMore)
				{
					PoolObject ();
				}
				else
				{
					UnpoolObject ();
				}
			}
			
			// Rename pooled objects
			RenameObjects ();
		}
		
		void PoolObject ()
		{
			GameObject go = Instantiate (m_objectPrefab) as GameObject;
			go.SetActive (false);
			go.transform.parent = transform;
			
			m_pooledObjects.Add (go);
			m_inactiveObjects.Enqueue (go);
		}
		
		void UnpoolObject ()
		{
			GameObject toRemove = null;
			foreach (GameObject go in m_pooledObjects)
			{
				if (m_inactiveObjects.Contains (go))
				{
					toRemove = go;
					break;
				}
			}
			
			if (toRemove != null)
			{
				m_pooledObjects.Remove (toRemove);
				// todo : remove from m_inactiveObjects too
				DestroyImmediate (toRemove);
			}
		}
		
		void RenameObjects ()
		{
			for (int i=0; i<transform.childCount; i++)
			{
				Transform t = transform.GetChild (i);
				
				int nameIndex = t.name.IndexOf ('.');
				
				if (nameIndex > 0)
				{
					string trimmedName = t.name.Substring (0, nameIndex);
					t.name = trimmedName + "." + i;
				}
				else
				{
					t.name = t.name + "." + i;
				}
			}
		}
		
		#endregion
	}
}