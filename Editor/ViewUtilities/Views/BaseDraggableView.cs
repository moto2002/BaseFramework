using UnityEngine;
using System.Collections;

namespace BaseFramework.EditorUtils
{
	public delegate void DragInputEvent( BaseDraggableView xView );
	
	public abstract class BaseDraggableView : BaseEditorView
	{
		public event DragInputEvent DragBegan;
		public event DragInputEvent DragChanged;
		public event DragInputEvent DragEnded;
		
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
							m_bIsBeingDragged = true;
							
							float fXOffset = ViewBounds.x - xMousePosition.x;
							float fYOffset = ViewBounds.y - xMousePosition.y;
							m_pxCursorOffsetFromCentre = new Vector2( fXOffset, fYOffset );
						
							if ( DragBegan != null )
							{
								DragBegan( this );
							}
						}
						break;
					}
					
					case EventType.MouseDrag:
					{
						if ( m_bIsBeingDragged )
						{
							float fBoundOriginX = xMousePosition.x + m_pxCursorOffsetFromCentre.x;
							float fBoundOriginY = xMousePosition.y + m_pxCursorOffsetFromCentre.y;
							Vector2 xNewBoundCentre = new Vector2( fBoundOriginX, fBoundOriginY );
							
							Rect NewBounds = new Rect();
							NewBounds.x = xNewBoundCentre.x;
							NewBounds.y = xNewBoundCentre.y;
							NewBounds.width = ViewBounds.width;
							NewBounds.height = ViewBounds.height;
							
							ViewBounds = NewBounds;
							
							SourceWindow.Repaint();
							
							if ( DragChanged != null )
							{
								DragChanged( this );
							}
						}
						break;
					}
					
					case EventType.MouseUp:
					{
						if ( DragEnded != null && m_bIsBeingDragged )
						{
							DragEnded( this );
						}
						
						m_bIsBeingDragged = false;
						m_pxCursorOffsetFromCentre = Vector2.zero;
						
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
		
		private bool m_bIsBeingDragged;
		private Vector2 m_pxCursorOffsetFromCentre;
	}
}