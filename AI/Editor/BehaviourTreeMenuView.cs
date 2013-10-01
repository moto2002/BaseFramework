using UnityEngine;
using System.Collections;

using BaseFramework.EditorUtils;

namespace BaseFramework.AI
{
	public delegate void MenuButtonPressed();
	
	public class BehaviourTreeMenuView : BaseEditorView
	{
		public event MenuButtonPressed CreateNewTree;
		
		public BehaviourTreeMenuView( BaseEditorWindow xSourceWindow ) : base( xSourceWindow )
		{
		}
		
		public override void Draw()
		{
			base.Draw ();
			
			Event xCurrentEvent = Event.current;
			EventType xCurrentEventType = xCurrentEvent.type;
			
			if ( xCurrentEventType == EventType.Repaint )
			{
				float fScreenWidth  = SourceWindow.maxSize.x;
				float fScreenHeight = m_defaultHeight;
				ViewBounds = new Rect( 0, 0, fScreenWidth, fScreenHeight );
			}
			
			GUI.Box( ViewBounds, "" );
			
			Rect xCreateNewTreeButtonRect = new Rect( 0, 0, 256, m_defaultHeight );
			bool bCreateNew = GUI.Button( xCreateNewTreeButtonRect, "Create new Behaviour Tree" );
			
			if ( bCreateNew && CreateNewTree != null )
			{
				CreateNewTree();
			}
		}
		
		private static float m_defaultHeight = 16.0f;
	}
}