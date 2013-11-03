using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace BaseFramework.PrefabPool
{
	[CustomEditor (typeof (PrefabPoolManager))]
	public class PrefabPoolManagerEditor : Editor
	{
		public void OnEnable()
		{
			m_pxPrefabPoolManager = (PrefabPoolManager)target;
		}
		
		public override void OnInspectorGUI()
		{	
			// List all pools
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField( "Pool Name" );
			EditorGUILayout.LabelField( "Object Type" );
			EditorGUILayout.LabelField( "Size" );
			EditorGUILayout.EndHorizontal();
			
			PrefabPool[] pxPrefabPools = m_pxPrefabPoolManager.GetAllPools();
			int iNumberOfPrefabPools = pxPrefabPools.Length;
			
			int iPoolIndexToRemove = -1;
			for ( int iPrefabPoolIndex = 0; iPrefabPoolIndex < iNumberOfPrefabPools; iPrefabPoolIndex++ )
			{
				PrefabPool pxPrefabPool = pxPrefabPools[ iPrefabPoolIndex ];
				
				EditorGUILayout.BeginHorizontal();
				{
					pxPrefabPool.name         = EditorGUILayout.TextField( pxPrefabPool.name);
					pxPrefabPool.ObjectPrefab = EditorGUILayout.ObjectField( pxPrefabPool.ObjectPrefab, typeof( GameObject ), false ) as GameObject;
					pxPrefabPool.Cached       = EditorGUILayout.IntField( pxPrefabPool.Cached );
					bool bPoolShouldBeRemoved = GUILayout.Button( "-" );
					if ( bPoolShouldBeRemoved )
					{
						iPoolIndexToRemove = iPrefabPoolIndex;
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			
			if ( iPoolIndexToRemove >= 0 && iPoolIndexToRemove < pxPrefabPools.Length )
			{
				m_pxPrefabPoolManager.RemovePool( pxPrefabPools[iPoolIndexToRemove].ObjectPrefab );
			}
			
			EditorGUILayout.BeginHorizontal();
			{
				m_pxNewPrefabPoolObject = EditorGUILayout.ObjectField ("Create new pool", m_pxNewPrefabPoolObject, typeof (GameObject), false) as GameObject;
				bool bShouldAddNewPool  = GUILayout.Button( "+", GUILayout.Width( 32 ) );
				if ( bShouldAddNewPool && m_pxNewPrefabPoolObject != null)
				{
					m_pxPrefabPoolManager.CreatePool( m_pxNewPrefabPoolObject );
					m_pxNewPrefabPoolObject = null;
					
					EditorUtility.SetDirty( m_pxPrefabPoolManager );
					
					foreach ( PrefabPool pxPrefabPool in pxPrefabPools )
					{
						EditorUtility.SetDirty( pxPrefabPool );
					}
				}
			}
			EditorGUILayout.EndHorizontal();
		}
		
		private GameObject m_pxNewPrefabPoolObject;
		private PrefabPoolManager m_pxPrefabPoolManager;
	}
}