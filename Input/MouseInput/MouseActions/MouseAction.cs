using UnityEngine;

namespace BaseFramework.MouseInput
{
	public delegate void MouseActionDelegate( MouseAction xMouseAction );
	
	public enum MouseActionState
	{
		MouseActionPossible,
		MouseActionRecognised,
		MouseActionFailed,
		MouseActionBegan,
		MouseActionChanged,
		MouseActionEnded
	};
	
	public abstract class MouseAction
	{
		public Vector3 focus;
		public Collider collider;
		
		public MouseActionDelegate actionDelegate;
		public MouseActionState state
		{
			get
			{
				return m_state;
			}
			
			set
			{
				m_state = value;
				
				if ( m_state != MouseActionState.MouseActionPossible )
				{
					if ( actionDelegate != null )
					{
						actionDelegate( this );
					}
				}
			}
		}
		
		public MouseAction( Collider xCollider, MouseActionDelegate xDelegate )
		{
			collider = xCollider;
			actionDelegate = xDelegate;
		}
		
		public virtual void OnButtonPressed( int iButtonID )
		{
			focus = Input.mousePosition;
		}
		
		public virtual void OnButtonReleased( int iButtonID )
		{
			focus = Input.mousePosition;
		}
		
		public virtual void OnHover( Vector3 xCursorPosition )
		{
			focus = Input.mousePosition;
		}
		
		public virtual void ResetAction()
		{
			state = MouseActionState.MouseActionPossible;
		}
		
		private MouseActionState m_state;
	}
}