using UnityEngine;
using System.Collections.Generic;

namespace BaseFramework.AI
{
	public class BehaviourTreeController : MonoBehaviour
	{
		public void UpdateViewInfo( Dictionary<string, object> xActorViewInfo )
		{
			m_pxActorBlackboard = xActorViewInfo;
		}
		
		protected virtual Node CreateBehaviourTree()
		{
			Node pxNode = new Node();
			pxNode.SetIsLeaf( false );
			pxNode.SetTaskType<SequenceTask>();
			
			Node pxChild_1 = new Node();
			pxChild_1.SetIsLeaf( true );
			pxNode.SetTaskType<DebugLogTask>();
			pxNode.AddChild( pxChild_1 );
			
			Node pxChild_2 = new Node();
			pxChild_2.SetIsLeaf( true );
			pxNode.SetTaskType<DebugLogTask>();
			pxNode.AddChild( pxChild_2 );
			
			return pxNode;
		}
		
		private void Start()
		{
			Node pxRootNode = CreateBehaviourTree();
			m_pxBehaviourTree = new BehaviourTree( pxRootNode );
		}
		
		private void Update()
		{
			m_pxBehaviourTree.Tick( m_pxActorBlackboard );
		}
		
		private BehaviourTree m_pxBehaviourTree;
		private Dictionary<string, object> m_pxActorBlackboard;
	}
}