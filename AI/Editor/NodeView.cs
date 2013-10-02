using UnityEngine;
using UnityEditor;

using BaseFramework.EditorUtils;

namespace BaseFramework.AI
{
	public class NodeView : BaseDraggableView
	{
		public Node ParentNode;
		public int SiblingIndex;
		
		public NodeView( Node xNode, BaseEditorWindow xSourceWindow ) : base( xSourceWindow )
		{
			m_node = xNode;
		}
		
		public override void Draw()
		{
			base.Draw();
			GUI.Box( ViewBounds, "" );
		}
		
		private Node m_node;
	}
}