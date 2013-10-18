using UnityEngine;
using System.Collections.Generic;

namespace BaseFramework.AI
{
	public abstract class Task
	{
		public Task( BehaviourTreeController pxTreeController )
		{
			m_eStatus = TaskState.eTaskReady;
			m_pxTreeController = pxTreeController;
		}
		
		public Task( BehaviourTreeController pxTreeController, Node pxNode )
		{
			m_pxNode = pxNode;
			m_eStatus = TaskState.eTaskReady;
			m_pxTreeController = pxTreeController;
		}
		
		public TaskState GetCurrentState()
		{
			return m_eStatus;
		}
		
		public TaskState TickTask( Dictionary<string, object> pxActorView )
		{
			if ( m_eStatus == TaskState.eTaskReady )
			{
				InitialiseTask();
				m_eStatus = TaskState.eTaskRunning;
			}
			
			if ( m_eStatus == TaskState.eTaskRunning && pxActorView != null )
			{
				UpdateTask( pxActorView );
			}
			
			if ( m_eStatus == TaskState.eTaskSuccess || m_eStatus == TaskState.eTaskFailed )
			{
				DestroyTask();
				m_eStatus = TaskState.eTaskReady;
			}
			
			return m_eStatus;
		}
		
		protected abstract void InitialiseTask();
		protected abstract void DestroyTask();
		
		protected abstract void UpdateTask( Dictionary<string, object> pxActorView );
		
		protected Node m_pxNode;
		protected TaskState m_eStatus;
		protected BehaviourTreeController m_pxTreeController;
	}
}