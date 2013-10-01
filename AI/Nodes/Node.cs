using System.Collections.Generic;


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
		/// <summary>
		/// Gets or sets the state of the Node.
		/// </summary>
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
		
		/// <summary>
		/// Gets the child nodes.
		/// </summary>
		public Node[] ChildNodes
		{
			get
			{
				return m_childNodes.ToArray();
			}
		}
		
		public Node()
		{
			m_childNodes = new List<Node>();
		}
		
		/// <summary>
		/// Adds a new child node after the last child
		/// in the SelectorNode.
		/// </summary>
		public void AddChild( Node xChildNode )
		{
			m_childNodes.Add( xChildNode );
		}
		
		/// <summary>
		/// Inserts a new child node at a specified index within 
		/// the current children of the SelectorNode.
		/// </summary>
		public void AddChild( Node xChildNode, int iNewIndex )
		{
			m_childNodes[ iNewIndex ] = xChildNode;
		}
		
		/// <summary>
		/// Removes an existing child from the SelectorNode.
		/// </summary>
		public void RemoveChild( Node xChildNode )
		{
			m_childNodes.Remove( xChildNode );
		}
		
		private List<Node> m_childNodes;
		private BehaviourState m_state;
	}
}