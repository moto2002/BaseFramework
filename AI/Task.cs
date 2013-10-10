using UnityEngine;
using System.Collections.Generic;

namespace BaseFramework.AI
{
	public abstract class Task
	{
		public Task( Node pxNode )
		{
			m_pxNode = pxNode;
		}
		
		public abstract void InitiliseTask();
		public abstract void DestroyTask();
		
		public abstract BehaviourNodeState UpdateTask( Dictionary<string, object> pxActorView );
		
		protected Node m_pxNode;
	}
}