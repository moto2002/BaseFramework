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
//			m_rootNode = new PrioritySelectorNode();
//			SequenceSelectorNode xSequenceSelector = new SequenceSelectorNode();
//			ConditionLeafNode xCondition = new ConditionLeafNode();
//			ActionLeafNode xAction = new ActionLeafNode();
//			
//			m_rootNode.AddChild( xSequenceSelector );
//			xSequenceSelector.AddChild( xCondition );
//			xSequenceSelector.AddChild( xAction );
		}
		
		public BehaviourTree( Node pxRootNode )
		{
			m_pxRootNode = pxRootNode;
			m_pxRootTask = pxRootNode.CreateTask();
		}
		
		public BehaviourNodeState Tick( Dictionary<string, object> pxActorView )
		{
			m_eStatus = m_pxRootTask.UpdateTask( pxActorView );
			
			return m_eStatus;
		}

		private Node m_pxRootNode;
		private Task m_pxRootTask;
		private BehaviourNodeState m_eStatus;
	}
}