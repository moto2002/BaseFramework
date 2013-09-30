using UnityEngine;
using UnityEditor;

using BaseFramework.EditorUtils;

namespace BaseFramework.AI
{
	public class NodeView : BaseDraggableView
	{
		const int NodeSizeX = 128;
		const int NodeSizeY = 64;
		
		public NodeView( BaseEditorWindow xSourceWindow, Vector2 xCentre ) : base( xSourceWindow )
		{
			float fNodeOriginX = xCentre.x - 0.5f * NodeSizeX;
			float fNodeOriginY = xCentre.y - 0.5f * NodeSizeY;
			Rect xNodeBounds = new Rect( fNodeOriginX, fNodeOriginY, NodeSizeX, NodeSizeY );
			ViewBounds = xNodeBounds;
		}
		
		public override void Draw()
		{
			base.Draw();
			GUI.Box( ViewBounds, "" );
		}
	}
}