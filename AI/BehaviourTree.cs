using UnityEngine;

namespace BaseFramework.AI
{
	public class BehaviourTree
	{
		public BehaviourTree()
		{
			m_rootNode = new PrioritySelectorNode();
		}
		
		public Node[] GenerateBehaviourStream()
		{
			return null;
		}
		
		private PrioritySelectorNode m_rootNode;
	}
}