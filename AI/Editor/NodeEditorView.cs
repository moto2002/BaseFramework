using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

using BaseFramework.EditorUtils;

namespace BaseFramework.AI
{
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
			base.Draw();
		}
		
		public void GenerateGrid()
		{
		}
		
		public NodeView CreateNodeViews( Node xNode )
		{
			Node[] xChildNodes = xNode.ChildNodes;
			
			int iNumberOfChildren = xChildNodes.Length;
			for ( int iChildIndex = 0; iChildIndex < iNumberOfChildren; iChildIndex++ )
			{
				Node xChild = xChildNodes[ iChildIndex ];
				
				NodeView xView = CreateNodeViews( xChild );
				xView.ParentNode   = xNode;
				xView.SiblingIndex = iChildIndex;
				
				// TODO:
				//
				// Q: How do we do Tree reordering?
				// A: ?
				//
				// Q: How do we find the Source and Target:
				//    - Parent Node?
				//    - Sibling Index?
				// A: - Publicly Store & Update them in the NodeView, as above.
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
				
				if ( xView == null )
				{
					// ?
				}
			}
			
			NodeView xNodeView = null;
			if ( xNode != null )
			{
				BaseEditorWindow xSourceWindow = this.SourceWindow;
				
				xNodeView = new NodeView( xNode, xSourceWindow );
				xNodeView.ViewBounds = new Rect( 0, 0, NodeGridSpacingX, NodeGridSpacingY );
				xNodeView.DragEnded += HandleDrop;
			}
			
			this.AddSubview( xNodeView );
			return xNodeView;
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