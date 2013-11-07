using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using BaseFramework.Core;

namespace BaseFramework.PrefabPool
{
	public class PrefabPoolManager : MonoSingleton<PrefabPoolManager>
	{
		public PrefabPool CreatePool( GameObject pxPrefab )
		{
			string pxPrefabID = GetPrefabID( pxPrefab );
			
			PrefabPool pxPrefabPool;
			bool bPoolExists = m_pxPrefabPools.ContainsKey( pxPrefabID );
			if ( !bPoolExists )
			{
				pxPrefabPool = NewPool( pxPrefab );
				m_pxPrefabPools.Add( pxPrefabID, pxPrefabPool );
			}
			else
			{
				pxPrefabPool = m_pxPrefabPools[ pxPrefabID ];
			}
			return pxPrefabPool;
		}
		
		public PrefabPool[] GetAllPools()
		{
			int iNumberOfPools = m_pxPrefabPools.Count;
			
			PrefabPool[] pxAllPrefabPools = new PrefabPool[ iNumberOfPools ];
			m_pxPrefabPools.Values.CopyTo( pxAllPrefabPools, 0 );
			
			return pxAllPrefabPools;
		}
		
		public PrefabPool GetPool( GameObject pxPrefab )
		{
			string pxPrefabID = GetPrefabID( pxPrefab );
			
			bool bPoolExists = m_pxPrefabPools.ContainsKey( pxPrefabID );
			if ( bPoolExists )
			{
				return m_pxPrefabPools[ pxPrefabID ];
			}
			return null;
		}
		
		public void RemovePool( GameObject prefabType )
		{
			string pxPrefabID = GetPrefabID( prefabType );
			
			bool bPoolExists = m_pxPrefabPools.ContainsKey( pxPrefabID );
			if ( bPoolExists )
			{
				GameObject pxPrefabPool = m_pxPrefabPools[pxPrefabID].gameObject;
				m_pxPrefabPools.Remove( pxPrefabID );
				
				Destroy( pxPrefabPool );
			}
		}
		
		private void Awake()
		{
			m_pxPrefabPools = new Dictionary<string, PrefabPool>();
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
		
		private Dictionary<String, PrefabPool> m_pxPrefabPools;
	}
}