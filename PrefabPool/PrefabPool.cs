using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BaseFramework.PrefabPool
{
	[ExecuteInEditMode ()]
	public class PrefabPool : MonoBehaviour
	{
		[SerializeField]
		private bool m_initialised;
		
		[SerializeField]
		private GameObject m_objectPrefab;
		
		[SerializeField]
		private List<GameObject> m_pooledObjects;
		
		[SerializeField]
		private Queue<GameObject> m_inactiveObjects; // TODO : bug: this keeps getting set to nothing when play mode is entered.
		
		#region Public Interface
		
		public GameObject ObjectPrefab
		{
			get { return m_objectPrefab; }
			set { m_objectPrefab = value; } // todo : check if a pool of this type exists already?
		}
		
		public int Cached
		{
			get { return m_pooledObjects.Count; }
			set { Recache (value); }
		}
		
		public int InactiveObjects
		{
			get { return m_inactiveObjects.Count; }
		}
		
		public GameObject GetNextActive ()
		{
			if (m_inactiveObjects.Count == 0)
			{
				Cached += 5;
			}
			
			GameObject go = m_inactiveObjects.Dequeue ();
			go.SetActive (true);
			
			return go;
		}
		
		#endregion
		
		#region Internal Functions
		
		void Awake ()
		{
			if (!m_initialised)
			{
				Initialise ();
			}
		}
		
		void Initialise ()
		{
			if (!m_initialised)
			{
				m_initialised = true;
				
				if (m_pooledObjects == null)
				{
					m_pooledObjects = new List<GameObject>();
				}
				
				if (m_inactiveObjects == null)
				{
					m_inactiveObjects = new Queue<GameObject>();
				}
			}
		}
		
		void Update ()
		{
			// Don't do anything if in sync with all children
			if (m_pooledObjects.Count.Equals (transform.childCount))
			{
				return;
			}
			
			// If not in sync, sync up!
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
			RenameObjects ();
		}
		
		// TODO : Recaching is bugged.
		void Recache (int newSize)
		{
			// How many objects we need to Instantiate / Destroy
			int diff = newSize - m_pooledObjects.Count;
			
			bool addMore = diff > 0;
			diff = Mathf.Abs (diff);
			
			// Do the Instantions / Destroys
			for (int i=0; i<diff; i++)
			{
				if (addMore)
				{
					PoolNewObject ();
				}
				else
				{
					UnpoolObject ();
				}
			}
			
			// Rename pooled objects
			RenameObjects ();
		}
		
		void PoolNewObject ()
		{
			GameObject go = Instantiate (m_objectPrefab) as GameObject;
			go.SetActive (false);
			go.transform.parent = transform;
			
			Rename (go, m_pooledObjects.Count);
			m_pooledObjects.Add (go);
			PoolObject (go);
		}
		
		void PoolObject (GameObject go)
		{
			if (!m_pooledObjects.Contains (go))
			{
				Debug.LogError ("[PrefabPool] GameObject is not contained in pooled objects!", gameObject);
				return;
			}
			
			m_inactiveObjects.Enqueue (go);
			go.SetActive (false);
		}
		
		void UnpoolObject ()
		{
			GameObject toRemove = GetNextActive();
			//GameObject toRemove = m_pooledObjects[m_pooledObjects.Count-1];
			
			if (toRemove != null)
			{
				m_pooledObjects.Remove (toRemove);
				DestroyImmediate (toRemove);
			}
		}
		
		void RenameObjects ()
		{
			for (int i=0; i<transform.childCount; i++)
			{
				Transform t = transform.GetChild (i);
				Rename (t.gameObject, i);
			}
		}
		
		void Rename (GameObject go, int index)
		{
			int nameIndex = go.name.IndexOf ('.');
				
			if (nameIndex > 0)
			{
				string trimmedName = go.name.Substring (0, nameIndex);
				go.name = trimmedName + "." + index;
			}
			else
			{
				go.name = go.name + "." + index;
			}
		}
		
		#endregion
	}
}