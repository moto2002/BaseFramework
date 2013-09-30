using System.Collections.Generic;

namespace BaseFramework.AI
{
	public abstract class SelectorNode : Node
	{
		public SelectorNode() : base()
		{
			m_childNodes = new List<Node>();
		}
		
		/// <summary>
		/// Gets the child nodes.
		/// </summary>
		/// <value>
		/// The child nodes.
		/// </value>
		public Node[] ChildNodes
		{
			get
			{
				return m_childNodes.ToArray();
			}
		}
		
		/// <summary>
		/// Adds a new child node after the last child
		/// in the SelectorNode.
		/// </summary>
		/// <param name='xChildNode'>
		/// The child node.
		/// </param>
		public void AddChild( Node xChildNode )
		{
			m_childNodes.Add( xChildNode );
		}
		
		/// <summary>
		/// Inserts a new child node at a specified index within 
		/// the current children of the SelectorNode.
		/// </summary>
		/// <param name='xChildNode'>
		/// The child node.
		/// </param>
		/// <param name='iNewIndex'>
		/// The index (position) for the new node.
		/// </param>
		public void AddChild( Node xChildNode, int iNewIndex )
		{
			m_childNodes[ iNewIndex ] = xChildNode;
		}
		
		/// <summary>
		/// Removes an existing child from the SelectorNode.
		/// </summary>
		/// <param name='xChildNode'>
		/// The child node.
		/// </param>
		public void RemoveChild( Node xChildNode )
		{
			m_childNodes.Remove( xChildNode );
		}
		
		private List<Node> m_childNodes;
	}
}