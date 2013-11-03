using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using BaseFramework.Core;

namespace BaseFramework.PrefabPool
{
	[ExecuteInEditMode ()]
	public class PrefabPoolManager : MonoSingleton<PrefabPoolManager>
	{
		// Choosing a data type to store references to the prefab pools:
		// - NO dupilcates
		// - Don't need fast inserts / removals as pools are not set up at run time.
		// - We could index or iterate. indexed would be quicker at runtime though?
		private Dictionary<String, PrefabPool> m_pxPrefabPools;
		
		[SerializeField]
		private List<String> m_pxPrefabKeys = new List<String>();
		
		[SerializeField]
		private List<PrefabPool> m_pxPrefabValues = new List<PrefabPool>();
		
		public void CreatePool( GameObject pxPrefab )
		{
			UpdateDictionary();
			
			string pxPoolID = GetPrefabID( pxPrefab );
			if ( !m_pxPrefabPools.ContainsKey( pxPoolID ) )
			{
				m_pxPrefabKeys.Add( pxPoolID );
				
				PrefabPool pxPrefabPool = NewPool( pxPrefab );
				m_pxPrefabValues.Add( pxPrefabPool );
				
				UpdateDictionary();
			}
		}
		
		public PrefabPool[] GetAllPools()
		{
			UpdateDictionary();
			
			PrefabPool[] pxAllPrefabPools = new PrefabPool[m_pxPrefabPools.Count];
			m_pxPrefabPools.Values.CopyTo( pxAllPrefabPools, 0 );
			
			return pxAllPrefabPools;
		}
		
		public PrefabPool GetPool (GameObject prefabType)
		{
			UpdateDictionary ();
			
			string id = GetPrefabID (prefabType);
			if (m_pxPrefabPools.ContainsKey (id))
			{
				return m_pxPrefabPools[id];
			}
			
			return null;
		}
		
		public void RemovePool (GameObject prefabType)
		{
			UpdateDictionary ();
			
			string id = GetPrefabID (prefabType);
			if (m_pxPrefabPools.ContainsKey (id))
			{
				DestroyImmediate (m_pxPrefabPools[id].gameObject);
				
				m_pxPrefabKeys.Remove (id);
				m_pxPrefabValues.Remove (NewPool (prefabType));
				
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
			m_pxPrefabPools = new Dictionary<string, PrefabPool>();
			
			if (m_pxPrefabKeys.Count != m_pxPrefabValues.Count)
 			{
				Debug.LogError ("Keys and Values are out of sync!! (keys: "+m_pxPrefabKeys.Count+", values: "+m_pxPrefabValues.Count+")");
				m_pxPrefabKeys = new List<string>();
				m_pxPrefabValues = new List<PrefabPool>();
				return;
			}
			
			for (int i=0; i<m_pxPrefabKeys.Count; i++)
			{
				m_pxPrefabPools.Add (m_pxPrefabKeys[i], m_pxPrefabValues[i]);
			}
		}
		
		private String GetPrefabID( GameObject pxPrefab )
		{
			if ( pxPrefab != null )
			{
				// Handle's the removal of the (copy) string Unity appends to a GameObject.
				int iIndexOfBracket = pxPrefab.name.IndexOf ('(');
				if ( iIndexOfBracket < 0 )
				{
					return pxPrefab.name;
				}
				else
				{
					return pxPrefab.name.Substring (0, iIndexOfBracket);
				}
			}
			return null;
		}
		
		private PrefabPool NewPool( GameObject pxPrefab )
		{
			GameObject pxPrefabPoolInstance = new GameObject();
			pxPrefabPoolInstance.transform.parent = transform;
			
			PrefabPool pxPrefabPool = pxPrefabPoolInstance.AddComponent<PrefabPool>();
			pxPrefabPool.ObjectPrefab = pxPrefab;
			pxPrefabPool.Cached = 5;
			
			return pxPrefabPool;
		}
		
		private void Update()
		{
			UpdateDictionary();
			
			foreach ( KeyValuePair<String, PrefabPool> pxPrefabPoolKVPair in m_pxPrefabPools )
			{
				string pxPrefabPoolID = pxPrefabPoolKVPair.Key;
				PrefabPool pxPrefabPool = pxPrefabPoolKVPair.Value;
				
				if ( pxPrefabPool == null )
				{
					m_pxPrefabPools.Remove( pxPrefabPoolID );
				}
				else if ( pxPrefabPool.ObjectPrefab == null )
				{
					DestroyImmediate( pxPrefabPool.gameObject );
					m_pxPrefabPools.Remove( pxPrefabPoolKVPair.Key );
				}
			}
			
			foreach ( Transform pxPrefabPoolTransform in transform )
			{
				PrefabPool pxPrefabPool = pxPrefabPoolTransform.GetComponent<PrefabPool>();
				
				if ( pxPrefabPool != null )
				{
					if ( m_pxPrefabPools.ContainsValue( pxPrefabPool ) )
					{
						continue;
					}
					
					GameObject pxPrefab = pxPrefabPool.ObjectPrefab;
					if ( pxPrefab != null )
					{
						string pxPrefabPoolID = GetPrefabID( pxPrefab );
						m_pxPrefabPools.Add( pxPrefabPoolID, pxPrefabPool );
					}
					else
					{
						DestroyImmediate( pxPrefabPoolTransform );
					}	
				}
				else
				{
					DestroyImmediate( pxPrefabPoolTransform );
				}
			}
		}
		
		#endregion
	}
}