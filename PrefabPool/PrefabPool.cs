using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BaseFramework
{
	[ExecuteInEditMode ()]
	public class PrefabPool : MonoBehaviour
	{
		[SerializeField]
		private GameObject m_objectPrefab; // bug: this keeps getting set to nothing when play mode is entered.
		
		[SerializeField]
		private List<GameObject> m_pooledObjects = new List<GameObject> ();
		
		[SerializeField]
		private Queue<GameObject> m_inactiveObjects = new Queue<GameObject> ();
		
		#region Public Interface
		
		public GameObject ObjectPrefab
		{
			get { return m_objectPrefab; }
			set { m_objectPrefab = value; }
		}
		
		public int Cached
		{
			get { return m_pooledObjects.Count; }
			set { Recache (value); }
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
		
		void Update ()
		{
			if (m_pooledObjects.Count.Equals (transform.childCount))
			{
				return;
			}
			
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
		
		void Recache (int newSize)
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
			PoolObject (go);
		}
		
		void PoolObject (GameObject go)
		{
			if (!m_pooledObjects.Contains (go))
			{
				Debug.LogWarning ("[PrefabPool] GameObject is not contained in pooled objects!", gameObject);
				return;
			}
			
			m_inactiveObjects.Enqueue (go);
			go.SetActive (false);
		}
		
		void UnpoolObject ()
		{
			GameObject toRemove = GetNextActive ();
			
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