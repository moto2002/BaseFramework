using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BaseFramework.PrefabPool
{
	public class PrefabPool : MonoBehaviour
	{
		public GameObject ObjectPrefab
		{
			get { return m_pxObjectPrefab;  }
			set
			{
				m_pxObjectPrefab = value;
				this.gameObject.name = m_pxObjectPrefab.name + " - PrefabPool";
			}
		}
		
		public int Cached
		{
			get { return m_pxAllPooledObjects.Count; }
			set { Recache( value ); }
		}
		
		public int InactiveObjects
		{
			get { return m_pxInactiveObjects.Count; }
		}
		
		public GameObject GetNextActive()
		{
			if ( m_pxInactiveObjects.Count == 0 )
			{
				Cached += 5;
			}
			GameObject pxGameObject = m_pxInactiveObjects.Dequeue();
			pxGameObject.SetActive( true );
			
			Debug.Log ( "Unpool- " + pxGameObject );
			
			return pxGameObject;
		}
		
		public void ReturnObjectToPool( GameObject pxObjectToAddToPool )
		{
			bool bObjectIsRegisteredToPool = m_pxAllPooledObjects.Contains( pxObjectToAddToPool );
			if ( bObjectIsRegisteredToPool )
			{
				bool bObjectIsAlreadyPooled = m_pxInactiveObjects.Contains( pxObjectToAddToPool );
				if ( !bObjectIsAlreadyPooled )
				{
					Debug.Log ( "Pool- " + pxObjectToAddToPool );
					m_pxInactiveObjects.Enqueue( pxObjectToAddToPool );
					pxObjectToAddToPool.SetActive( false );
				}
			}
			else
			{
				Debug.LogError( "[PrefabPool] GameObject is not contained in pooled objects!", gameObject );
			}
		}
		
		private void Awake()
		{
			m_pxAllPooledObjects   = new List<GameObject>();
			m_pxInactiveObjects = new Queue<GameObject>();
		}
		
		#region Adding / Removing Objects to the Pool
		
		private void Recache( int iNewPoolSize )
		{
			// How many objects we need to Instantiate / Destroy
			int iCurrentSize  = m_pxAllPooledObjects.Count;
			int iChangeInSize = iNewPoolSize - iCurrentSize;
			
			bool bMustAddMoreObjects = iChangeInSize > 0;
			iChangeInSize = Mathf.Abs( iChangeInSize );
			
			// Do the Instantions / Destroys
			if ( bMustAddMoreObjects )
			{
				PoolNewObjects( iChangeInSize );
			}
			else
			{
				RemoveExistingObjects( iChangeInSize );
			}
			
			// Rename pooled objects
			RenameObjects ();
		}
		
		private void PoolNewObjects( int iAmountToAdd )
		{
			for ( int iAddIndex = 0; iAddIndex < iAmountToAdd; iAddIndex++ )
			{
				GameObject pxNewGameObjectToPool = Instantiate( m_pxObjectPrefab ) as GameObject;
				pxNewGameObjectToPool.SetActive( false );
				pxNewGameObjectToPool.transform.parent = transform;
				
				Rename( pxNewGameObjectToPool, m_pxAllPooledObjects.Count );
				m_pxAllPooledObjects.Add( pxNewGameObjectToPool );
				
				ReturnObjectToPool( pxNewGameObjectToPool );
			}
		}
		
		private void RemoveExistingObjects( int iAmountToRemove )
		{
			int iNumberOfInactiveObjects = m_pxInactiveObjects.Count;
			
			int iTotalAvailableForRemoval = Mathf.Min( iAmountToRemove, iNumberOfInactiveObjects );
			for ( int iRemoveIndex = 0; iRemoveIndex < iTotalAvailableForRemoval; iRemoveIndex++ )
			{
				// We only want to remove inactive objects. Active objects may be busy.
				GameObject pxObjectToRemoveFromPool = m_pxInactiveObjects.Dequeue();
				m_pxAllPooledObjects.Remove( pxObjectToRemoveFromPool );
				
				Destroy( pxObjectToRemoveFromPool );
			}
		}
		
		#endregion
		
		#region Helper Methods
		
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
		
		private bool m_bInitialised;
		private GameObject m_pxObjectPrefab;
		private List<GameObject> m_pxAllPooledObjects;
		private Queue<GameObject> m_pxInactiveObjects;
	}
}