using System;
using System.Reflection;
using System.Collections.Generic;

namespace BaseFramework.AI
{
	public class Node
	{
		public Task CreateTask( BehaviourTreeController pxTreeController )
		{
			Task pxTask = (Task)Activator.CreateInstance( m_xType, new object[] { pxTreeController, this } );
			return pxTask;
		}
		
		public void SetTaskType<T>() where T : Task
		{
			m_xType = typeof( T );
		}
		
		public IEnumerator<Node> GetChildNodeEnumerator()
		{
			return m_pxChildren.GetEnumerator();
		}
		
		public bool IsLeaf()
		{
			return m_bIsLeaf;
		}
		
		public void SetIsLeaf( bool bIsLeafNode )
		{
			m_bIsLeaf = bIsLeafNode;
			
			if ( m_bIsLeaf )
			{
				m_pxChildren = null;
			}
			else
			{
				m_pxChildren = new List<Node>();
			}
		}
		
		public bool AddChild( Node pxChildNode )
		{
			if ( !m_bIsLeaf )
			{
				m_pxChildren.Add( pxChildNode );
				return true;
			}
			return false;
		}
		
		public bool AddChild( Node pxChildNode, int iAtIndex )
		{
			if ( !m_bIsLeaf )
			{
				m_pxChildren.Insert( iAtIndex, pxChildNode );
				return true;
			}
			return false;
		}
		
		public bool MoveChild( int iChildIndex, int iNewChildIndex )
		{
			if ( !m_bIsLeaf )
			{
				return false;
			}
			return false;
		}
		
		public bool RemoveChild( Node pxChild )
		{
			if ( !m_bIsLeaf )
			{
				m_pxChildren.Remove( pxChild );
				return true;
			}
			return false;
		}
		
		public bool RemoveChild( int iChildIndex )
		{
			if ( !m_bIsLeaf )
			{
				m_pxChildren.RemoveAt( iChildIndex );
				return true;
			}
			return false;
		}
		
		private Type m_xType;
		private bool m_bIsLeaf;
		private List<Node> m_pxChildren;
	}
}
