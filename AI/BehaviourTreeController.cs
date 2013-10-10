using UnityEngine;
using System.Collections.Generic;

namespace BaseFramework.AI
{
	public enum BehaviourNodeState
	{
		Ready,
		Visited,
		Running,
		Failed
	};
	
	public class BehaviourTreeController : MonoBehaviour
	{
		public void UpdateViewInfo( Dictionary<string, object> xActorViewInfo )
		{
			m_pxActorBlackboard = xActorViewInfo;
		}
		
		private void Start()
		{
			m_pxBehaviourTree = new BehaviourTree();
		}
		
		private void Update()
		{
			m_pxBehaviourTree.Tick( m_pxActorBlackboard );
		}
		
		private BehaviourTree m_pxBehaviourTree;
		private Dictionary<string, object> m_pxActorBlackboard;
	}
}