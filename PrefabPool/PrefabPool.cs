using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BaseFramework.PrefabPool
{
    public class PrefabPool : MonoBehaviour
    {
        public GameObject ObjectPrefab {
            get { return m_pxObjectPrefab;  }
            set {
                m_pxObjectPrefab = value;
                this.gameObject.name = m_pxObjectPrefab.name + " - PrefabPool";
            }
        }
		
        public int Cached {
            get { return m_pxAllPooledObjects.Count; }
            set { Recache( value ); }
		}
		
		public int InactiveObjects
		{
			get { return m_pxInactiveObjects.Count; }
		}
		
        public GameObject GetNextActive()
        {
            return GetNextActive( Vector3.zero, Quaternion.identity );
        }
        
        public GameObject GetNextActive( Vector3 pxInitialPosition )
        {
            return GetNextActive( pxInitialPosition, Quaternion.identity );
        }
        
        public GameObject GetNextActive( Vector3 pxInitialPosition, Quaternion pxRotation )
        {
            if ( m_pxInactiveObjects.Count == 0 )
            {
                Cached += 5;
            }
            PrefabPoolObject pxPoolObject = m_pxInactiveObjects.Dequeue();
            pxPoolObject.Spawn( pxInitialPosition, pxRotation );
            
            return pxPoolObject.gameObject;
        }
        
        private void Awake()
        {
            m_pxAllPooledObjects = new List<PrefabPoolObject>();
            m_pxInactiveObjects = new Queue<PrefabPoolObject>();
        }
        
        private void ReturnObjectToPool( PrefabPoolObject pxPoolComponent )
        {
            m_pxInactiveObjects.Enqueue( pxPoolComponent );
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
			RenameObjects();
		}
		
		private void PoolNewObjects( int iAmountToAdd )
		{
			for ( int iAddIndex = 0; iAddIndex < iAmountToAdd; iAddIndex++ )
			{
				GameObject pxNewGameObjectToPool = Instantiate( m_pxObjectPrefab ) as GameObject;
				pxNewGameObjectToPool.SetActive( false );
				pxNewGameObjectToPool.transform.parent = transform;
				
                PrefabPoolObject pxPoolComponent = pxNewGameObjectToPool.GetComponent<PrefabPoolObject>();
                if ( pxPoolComponent == null )
                {
                    Debug.LogError( "No PrefabPoolObject Component could be found on " + m_pxObjectPrefab + ". Cannot create prefab pool." );
                    Destroy( pxNewGameObjectToPool );
                    break;
                }
                else
                {
    				Rename( pxNewGameObjectToPool, m_pxAllPooledObjects.Count );
    				m_pxAllPooledObjects.Add( pxPoolComponent );
    				
                    pxPoolComponent.OwnerPool = this;
                }
			}
		}
		
		private void RemoveExistingObjects( int iAmountToRemove )
		{
			int iNumberOfInactiveObjects = m_pxInactiveObjects.Count;
			
			int iTotalAvailableForRemoval = Mathf.Min( iAmountToRemove, iNumberOfInactiveObjects );
			for ( int iRemoveIndex = 0; iRemoveIndex < iTotalAvailableForRemoval; iRemoveIndex++ )
			{
				// We only want to remove inactive objects. Active objects may be busy.
                PrefabPoolObject pxObjectToRemoveFromPool = m_pxInactiveObjects.Dequeue();
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
		private List<PrefabPoolObject> m_pxAllPooledObjects;
        private Queue<PrefabPoolObject> m_pxInactiveObjects;
	}
}