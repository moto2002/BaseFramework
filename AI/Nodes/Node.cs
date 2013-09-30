
namespace BaseFramework.AI
{
	public enum BehaviourState
	{
		Ready,
		Visiting,
		Running,
		Failed
	};
	
	public abstract class Node
	{
		public BehaviourState State
		{
			get
			{
				return m_state;
			}
			
			protected set
			{
				m_state = value;
			}
		}
		
		public Node()
		{
			
		}
		
		private BehaviourState m_state;
	}
}