using UnityEngine;
using System.Collections.Generic;

namespace BaseFramework.AI
{
	public enum TaskState
	{
		eTaskReady,
		eTaskRunning,
		eTaskSuccess,
		eTaskFailed
	};
	
	public class BehaviourTree
	{
		public Node RootNode
		{
			get { return m_pxRootNode; }
			set
			{
				m_pxRootNode = value;
				
				m_pxRootTask = null;
				if ( m_pxRootNode != null )
				{
					m_pxRootTask = m_pxRootNode.CreateTask();
				}
			}
		}
		
		public BehaviourTree()
		{
		}
		
		public BehaviourTree( Node pxRootNode )
		{
			RootNode = pxRootNode;
		}
		
		public TaskState Tick( Dictionary<string, object> pxActorView )
		{
			bool bHasTask = m_pxRootTask != null;
			if ( bHasTask )
			{
				m_pxRootTask.TickTask( pxActorView );
				m_eStatus = m_pxRootTask.GetCurrentState();
				
				return m_eStatus;
			}
			return TaskState.eTaskFailed;
		}

		private Node m_pxRootNode;
		private Task m_pxRootTask;
		private TaskState m_eStatus;
	}
}