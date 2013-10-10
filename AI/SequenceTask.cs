using System.Collections.Generic;

namespace BaseFramework.AI
{
	public class SequenceTask : Task
	{
		public override void InitialiseTask()
		{
			bool bIsLeafNode = m_pxNode.IsLeaf();
			if ( !bIsLeafNode )
			{
				m_eStatus = BehaviourNodeState.READY;
				m_pxNodeEnumerator = m_pxNode.GetChildNodeEnumerator();
				
				m_pxCurrentNode = m_pxNodeEnumerator.Current;
				m_pxCurrentTask = m_pxCurrentNode.CreateTask();
			}
		}
		
		public override void DestroyTask()
		{
			m_pxCurrentNode = null;
			m_pxCurrentTask = null;

			m_pxNodeEnumerator.Dispose();
			m_pxNodeEnumerator = null;
		}
		
		public override void UpdateTask( Dictionary<string, object> pxActorView )
		{
			BehaviourNodeState eCurrentTaskState = m_pxCurrentTask.GetCurrentState();
			
			switch ( eCurrentTaskState )
			{
				
			case BehaviourNodeState.READY:
			{
				m_pxCurrentTask.InitialiseTask();
				m_pxCurrentTask.UpdateTask( pxActorView );
				
				break;
			}
				
			case BehaviourNodeState.RUNNING:
			{
				m_eStatus = BehaviourNodeState.RUNNING;
				m_pxCurrentTask.UpdateTask( pxActorView );
				
				break;
			}
				
			case BehaviourNodeState.FAILED:
			{
				m_pxCurrentTask.DestroyTask();
				m_pxCurrentTask = null;
				m_pxCurrentNode = null;
				
				m_eStatus = BehaviourNodeState.FAILED;
				
				break;
			}
				
			case BehaviourNodeState.SUCCESS:
			{
				m_pxCurrentTask.DestroyTask();
				m_pxCurrentTask = null;
				m_pxCurrentNode = null;
				
				bool bNextNode = m_pxNodeEnumerator.MoveNext();
				if ( bNextNode )
				{
					m_pxCurrentNode = m_pxNodeEnumerator.Current;
					m_eStatus = BehaviourNodeState.RUNNING;
				}
				else
				{
					m_eStatus = BehaviourNodeState.SUCCESS;
				}
				
				break;
			}
				
			}
		}
		
		private Task m_pxCurrentTask;
		private Node m_pxCurrentNode;
		private IEnumerator<Node> m_pxNodeEnumerator;
	}
}

