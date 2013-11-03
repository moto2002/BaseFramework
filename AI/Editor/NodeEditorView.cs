using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

using BaseFramework.EditorUtils;

namespace BaseFramework.AI
{
// TODO:
//
// Q: How do we do Tree reordering?
// A: ?
//
// Q: How do we find the Source and Target:
//    - Parent Node?
//    - Sibling Index?
// A: - Publicly Store & Update them in the NodeView
//
// Q: How do we find the appropriate Grid Position, given a Parent Node / Sibling Index?
// A: ?
//
// Q: How do we clip a drag-drop to a grid?
// A: - Expose a DragEnded event in the BaseDraggableView.
//    - Handle the event here, setting the Centre property of the View to the nearest empty Grid Space
//
// Q: How do we link Grid Positions to Nodes?
// A: It'll be the centre point fool!!
//
// Q: How do we find the nearest empty grid space?
// A: - Find nearest by Dividing Centre Point Components by NodeGridSpacingX/Y
//    - Test empty by:
//      1. Find the Parent Node / Sibling Index that corresponds to the grid space
//      2. Iterating through all NodeViews, searching for matching Parent Node / Sibling Index values.
//
	public class NodeEditorView : BaseEditorView
	{
		public NodeEditorView( BaseEditorWindow xSourceWindow ) : base( xSourceWindow )
		{
		}
		
		public void SetTree( BehaviourTree xTree )
		{
			m_tree = xTree;
			this.RemoveAllSubviews();
			
			Node xRootNode = m_tree.RootNode;
			m_rootNodeView = this.CreateNodeViews( xRootNode );
		}
		
		public override void Draw()
		{
			if ( m_rootNodeView != null )
			{
				DrawNodeViews( m_rootNodeView );
			}
		}
		
		private NodeView CreateNodeViews( Node xNode )
		{
//			List<Node> xChildNodes = xNode.ChildNodes;
//			List<NodeView> xChildNodeViews = new List<NodeView>();
//			
//			int iNumberOfChildren = xChildNodes.Length;
//			for ( int iChildIndex = 0; iChildIndex < iNumberOfChildren; iChildIndex++ )
//			{
//				Node xChild = xChildNodes[ iChildIndex ];
//				
//				NodeView xView = CreateNodeViews( xChild );
//				xView.ParentNode   = xNode;
//				xView.SiblingIndex = iChildIndex;
//				
//				xChildNodeViews.Add( xView );
//			}
//			
//			NodeView xNodeView = null;
//			if ( xNode != null )
//			{
//				BaseEditorWindow xSourceWindow = this.SourceWindow;
//				
//				xNodeView = new NodeView( xNode, xSourceWindow );
//				xNodeView.ViewBounds = new Rect( 0, 0, NodeGridSpacingX, NodeGridSpacingY );
//				xNodeView.DragEnded += HandleDrop;
//				xNodeView.ChildNodeViews = xChildNodeViews.ToArray();
//			}
//			
//			return xNodeView;
			return null;
		}
		
		private void DrawNodeViews( NodeView xNodeView )
		{
			List<Vector2> xEndPoints = new List<Vector2>();
			
			NodeView[] xChildNodeViews = xNodeView.ChildNodeViews;
			for ( int iChildViewIndex = 0; iChildViewIndex < xChildNodeViews.Length; iChildViewIndex++ )
			{
				NodeView xChildNodeView = xChildNodeViews[ iChildViewIndex ];
				
				Vector2 xEndPoint = xChildNodeView.Centre;
				xEndPoint.y += 0.5f * NodeGridSpacingY;
				
				xEndPoints.Add( xEndPoint );
			}
			
			float fStartPointX = xNodeView.Centre.x;
			float fStartPointY = xNodeView.Centre.y + 0.5f * NodeGridSpacingY;
			
			xNodeView.Draw();
			foreach ( Vector2 xEndPoint in xEndPoints )
			{
				float fEndPointX = xEndPoint.x;
				float fEndPointY = xEndPoint.y;
				
				GL.Begin( GL.LINES );
				
				GL.Vertex3( fStartPointX, fStartPointY, 0 );
				GL.Vertex3( fEndPointX,   fEndPointY,   0 );
				
				GL.End();
			}
		}
		
		private void HandleDrop( BaseDraggableView xView )
		{
			Vector2 xGridPosition = xView.Centre;
			xGridPosition.x = Mathf.FloorToInt( xGridPosition.x / NodeGridSpacingX );
			xGridPosition.y = Mathf.FloorToInt( xGridPosition.y / NodeGridSpacingY );
			
			xGridPosition.x = Mathf.Max( xGridPosition.x, 1 );
			xGridPosition.y = Mathf.Max( xGridPosition.y, 1 );
			
			
			Node xOtherNode = GetNodeAtGridPosition( xGridPosition );
			if ( xOtherNode == null )
			{
				Vector2 xNewCentre = xGridPosition;
				xNewCentre.x *= NodeGridSpacingX;
				xNewCentre.y *= NodeGridSpacingY;
				
				xView.Centre = xNewCentre;
				SourceWindow.Repaint();
			}
		}
		
		private Node GetNodeAtGridPosition( Vector2 xGridPosition )
		{
			// TODO
			return null;
		}
		
		private const int NodeGridSpacingX = 128;
		private const int NodeGridSpacingY = 64;
		
		private BehaviourTree m_tree;
		private NodeView m_rootNodeView;
	}
}