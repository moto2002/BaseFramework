using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BaseFramework.PrefabPool
{
	[ExecuteInEditMode()]
	public class PrefabPool : MonoBehaviour
	{
		[SerializeField]
		private bool m_bInitialised;
		
		[SerializeField]
		private GameObject m_pxObjectPrefab;
		
		[SerializeField]
		private List<GameObject> m_pxPooledObjects;
		
		[SerializeField]
		private Queue<GameObject> m_pxInactiveObjects; // TODO : bug: this keeps getting set to nothing when play mode is entered.
		
		#region Public Interface
		
		public GameObject ObjectPrefab
		{
			get { return m_pxObjectPrefab; }
			set { m_pxObjectPrefab = value; } // todo : check if a pool of this type exists already?
		}
		
		public int Cached
		{
			get { return m_pxPooledObjects.Count; }
			set { Recache (value); }
		}
		
		public int InactiveObjects
		{
			get { return m_pxInactiveObjects.Count; }
		}
		
		public GameObject GetNextActive()
		{
			if (m_pxInactiveObjects.Count == 0)
			{
				Cached += 5;
			}
			
			GameObject pxGameObject = m_pxInactiveObjects.Dequeue();
			pxGameObject.SetActive( true );
			
			return pxGameObject;
		}
		
		#endregion
		
		#region Internal Functions
		
		private void Awake()
		{
			if ( !m_bInitialised )
			{
				Initialise();
			}
		}
		
		private void Initialise()
		{
			if ( !m_bInitialised )
			{
				m_bInitialised = true;
				
				if ( m_pxPooledObjects == null )
				{
					m_pxPooledObjects = new List<GameObject>();
				}
				
				if ( m_pxInactiveObjects == null )
				{
					m_pxInactiveObjects = new Queue<GameObject>();
				}
			}
		}
		
		private void Update()
		{
			// Don't do anything if in sync with all children
			int iNumberOfChildObjects = transform.childCount;
			int iNumberOfObjectsRegisteredToPool = m_pxPooledObjects.Count;
			
			bool bAllObjectsRegistered = iNumberOfObjectsRegisteredToPool.Equals( iNumberOfChildObjects );
			if ( !bAllObjectsRegistered )
			{
				// If not in sync, sync up!
				for (int i=0; i<transform.childCount; i++)
				{
					GameObject go = transform.GetChild (i).gameObject;
					if ( !m_pxPooledObjects.Contains( go ) )
					{
						m_pxPooledObjects.Add( go );
					}
					
					if ( !go.activeSelf && !m_pxInactiveObjects.Contains( go ) )
					{
						m_pxInactiveObjects.Enqueue( go );
					}
				}
				RenameObjects();
			}
		}
		
		// TODO : Recaching is bugged.
		private void Recache( int iNewPoolSize )
		{
			// How many objects we need to Instantiate / Destroy
			int iCurrentSize = m_pxPooledObjects.Count;
			int iChangeInSize = iNewPoolSize - iCurrentSize;
			
			bool bMustAddMoreObjects = iChangeInSize > 0;
			iChangeInSize = Mathf.Abs( iChangeInSize );
			
			// Do the Instantions / Destroys
			for ( int iObjectToModifyIndex = 0; iObjectToModifyIndex < iChangeInSize; iObjectToModifyIndex++ )
			{
				if ( bMustAddMoreObjects )
				{
					PoolNewObject();
				}
				else
				{
					UnpoolObject();
				}
			}
			
			// Rename pooled objects
			RenameObjects ();
		}
		
		private void PoolNewObject()
		{
			GameObject pxNewGameObjectToPool = Instantiate( m_pxObjectPrefab ) as GameObject;
			pxNewGameObjectToPool.SetActive( false );
			pxNewGameObjectToPool.transform.parent = transform;
			
			Rename( pxNewGameObjectToPool, m_pxPooledObjects.Count );
			m_pxPooledObjects.Add( pxNewGameObjectToPool );
			
			PoolObject( pxNewGameObjectToPool );
		}
		
		private void PoolObject( GameObject pxObjectToAddToPool )
		{
			bool bObjectIsRegisteredToPool = m_pxPooledObjects.Contains( pxObjectToAddToPool );
			if ( bObjectIsRegisteredToPool )
			{
				m_pxInactiveObjects.Enqueue( pxObjectToAddToPool );
				pxObjectToAddToPool.SetActive( false );
			}
			else
			{
				Debug.LogError ("[PrefabPool] GameObject is not contained in pooled objects!", gameObject);
			}
		}
		
		private void UnpoolObject()
		{
			GameObject pxObjectToRemoveFromPool = GetNextActive();
			//GameObject toRemove = m_pooledObjects[m_pooledObjects.Count-1];
			
			if ( pxObjectToRemoveFromPool != null )
			{
				m_pxPooledObjects.Remove( pxObjectToRemoveFromPool );
				DestroyImmediate( pxObjectToRemoveFromPool );
			}
		}
		
		private void RenameObjects()
		{
			int iNumberOfObjects = transform.childCount;
			for ( int iObjectIndex = 0; iObjectIndex < iNumberOfObjects; iObjectIndex++ )
			{
				Transform pxObjectTransform = transform.GetChild( iObjectIndex );
				GameObject pxObject = pxObjectTransform.gameObject;
				
				Rename( pxObject, iObjectIndex );
			}
		}
		
		private void Rename( GameObject pxObjectToRename, int iIndexOfObjectInPool )
		{
			int iIndexOfStop = pxObjectToRename.name.IndexOf ('.');
				
			if ( iIndexOfStop > 0 )
			{
				string pxTrimmedName  = pxObjectToRename.name.Substring (0, iIndexOfStop);
				pxObjectToRename.name = pxTrimmedName + "." + iIndexOfObjectInPool;
			}
			else
			{
				pxObjectToRename.name = pxObjectToRename.name + "." + iIndexOfObjectInPool;
			}
		}
		
		#endregion
	}
}