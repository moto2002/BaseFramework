using UnityEngine;
using UnityEditor;

using BaseFramework.EditorUtils;

namespace BaseFramework.AI
{
	public class NodeEditorView : BaseEditorView
	{
		public NodeEditorView( BaseEditorWindow xSourceWindow, float fYOriginOffset ) : base( xSourceWindow )
		{
			m_yOriginOffset = fYOriginOffset;
		}
		
		public void SetTree( BehaviourTree xTree )
		{
			m_tree = xTree;
		}
		
		public override void Draw()
		{
			base.Draw();
			
			Event xCurrentEvent = Event.current;
			EventType xCurrentEventType = xCurrentEvent.type;
			
			if ( xCurrentEventType == EventType.Repaint )
			{
				float fSourceWidth  = SourceWindow.maxSize.x;
				float fSourceHeight = SourceWindow.maxSize.y;
				ViewBounds = new Rect( 0, m_yOriginOffset, fSourceWidth, fSourceHeight );
			}
			
			GUI.Box( ViewBounds, "" );
		}
		
		private float m_yOriginOffset;
		private BehaviourTree m_tree;
	}
}