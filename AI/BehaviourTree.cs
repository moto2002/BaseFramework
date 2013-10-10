using UnityEngine;
using System.Collections.Generic;

namespace BaseFramework.AI
{
	public class BehaviourTree
	{
		public Node RootNode
		{
			get { return m_pxRootNode; }
		}
		
		public BehaviourTree()
		{
			m_pxRootNode = new Node();
			m_pxRootNode.SetIsLeaf( false );
			m_pxRootNode.SetTaskType<SequenceTask>();
//			m_pxRootNode.AddChild();
		}
		
		public BehaviourTree( Node pxRootNode )
		{
			m_pxRootNode = pxRootNode;
			m_pxRootTask = pxRootNode.CreateTask();
		}
		
		public BehaviourNodeState Tick( Dictionary<string, object> pxActorView )
		{
			m_eStatus = m_pxRootTask.GetCurrentState();
			m_pxRootTask.UpdateTask( pxActorView );
			return m_eStatus;
		}

		private Node m_pxRootNode;
		private Task m_pxRootTask;
		private BehaviourNodeState m_eStatus;
	}
}