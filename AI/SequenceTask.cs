using System.Collections.Generic;

namespace BaseFramework.AI
{
	public class SequenceTask : Task
	{
		public SequenceTask( Node pxNode ) : base( pxNode ) {  }
		
		protected override void InitialiseTask()
		{
			bool bIsLeafNode = m_pxNode.IsLeaf();
			if ( !bIsLeafNode )
			{
				m_eStatus = TaskState.eTaskReady;
				m_pxNodeEnumerator = m_pxNode.GetChildNodeEnumerator();
				
				m_pxCurrentNode = m_pxNodeEnumerator.Current;
				m_pxCurrentTask = m_pxCurrentNode.CreateTask();
			}
		}
		
		protected override void DestroyTask()
		{
			m_pxCurrentNode = null;
			m_pxCurrentTask = null;

			m_pxNodeEnumerator.Dispose();
			m_pxNodeEnumerator = null;
		}
		
		protected override void UpdateTask( Dictionary<string, object> pxActorView )
		{
			TaskState eCurrentTaskState = m_pxCurrentTask.TickTask( pxActorView );
			
			switch ( eCurrentTaskState )
			{
				
			case TaskState.eTaskReady:
			{
				break;
			}
				
			case TaskState.eTaskRunning:
			{
				m_eStatus = TaskState.eTaskRunning;
				
				break;
			}
				
			case TaskState.eTaskFailed:
			{
				m_pxCurrentTask = null;
				m_pxCurrentNode = null;
				
				m_eStatus = TaskState.eTaskFailed;
				
				break;
			}
				
			case TaskState.eTaskSuccess:
			{
				m_pxCurrentTask = null;
				m_pxCurrentNode = null;
				
				bool bNextNode = m_pxNodeEnumerator.MoveNext();
				if ( bNextNode )
				{
					m_pxCurrentNode = m_pxNodeEnumerator.Current;
					m_eStatus = TaskState.eTaskRunning;
				}
				else
				{
					m_eStatus = TaskState.eTaskSuccess;
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

