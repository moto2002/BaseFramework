using UnityEngine;
using System.Collections;

namespace BaseFramework.EditorUtils
{
	public abstract class BaseDraggableView : BaseEditorView
	{
		public BaseDraggableView( BaseEditorWindow xSourceWindow ) : base( xSourceWindow )
		{
		}
		
		public override void Draw()
		{
			Event xCurrentEvent = Event.current;
			
			if ( xCurrentEvent != null )
			{
				EventType xCurrentEventType = xCurrentEvent.type;
				
				if ( xCurrentEvent.isMouse )
				{
					Vector3 xMousePosition = xCurrentEvent.mousePosition;
					bool bWithinBounds = CursorIsWithinBounds( xMousePosition );
					switch ( xCurrentEventType )
					{
						case EventType.MouseDown:
						{
							//TODO: Raycast to hit topmost BaseDraggableView...?
							if ( bWithinBounds )
							{
								m_isBeingDragged = true;
								
								float fXOffset = ViewBounds.x - xMousePosition.x;
								float fYOffset = ViewBounds.y - xMousePosition.y;
								m_cursorOffsetFromCentre = new Vector2( fXOffset, fYOffset );
							}
							break;
						}
							
						case EventType.MouseDrag:
						{
							if ( m_isBeingDragged )
							{
								float fBoundOriginX = xMousePosition.x + m_cursorOffsetFromCentre.x;
								float fBoundOriginY = xMousePosition.y + m_cursorOffsetFromCentre.y;
								Vector2 xNewBoundCentre = new Vector2( fBoundOriginX, fBoundOriginY );
								
								Rect NewBounds = new Rect();
								NewBounds.x = xNewBoundCentre.x;
								NewBounds.y = xNewBoundCentre.y;
								NewBounds.width = ViewBounds.width;
								NewBounds.height = ViewBounds.height;
								
								ViewBounds = NewBounds;
								
								SourceWindow.Repaint();
							}
							break;
						}
						
						case EventType.MouseUp:
						{
							m_isBeingDragged = false;
							m_cursorOffsetFromCentre = Vector2.zero;
						
							break;
						}
					}
				}
			}
		}
		
		private bool CursorIsWithinBounds( Vector3 xCursorPosition )
		{
			bool bGreaterThanMin = xCursorPosition.x >= ViewBounds.xMin &&
								   xCursorPosition.y >= ViewBounds.yMin;
			
			bool bLessThanMax    = xCursorPosition.x <= ViewBounds.xMax && 
								   xCursorPosition.y <= ViewBounds.yMax;
			
			return bGreaterThanMin && bLessThanMax;
		}
		
		private bool m_isBeingDragged;
		private Vector2 m_cursorOffsetFromCentre;
	}
}