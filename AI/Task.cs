using UnityEngine;
using System.Collections.Generic;

namespace BaseFramework.AI
{
	public abstract class Task
	{
		public Task()
		{
		}
		
		public Task( Node pxNode )
		{
			m_pxNode = pxNode;
		}
		
		public BehaviourNodeState GetCurrentState()
		{
			return m_eStatus;
		}
		
		public abstract void InitialiseTask();
		public abstract void DestroyTask();
		
		public abstract void UpdateTask( Dictionary<string, object> pxActorView );
		
		protected Node m_pxNode;
		
		protected BehaviourNodeState m_eStatus;
	}
}