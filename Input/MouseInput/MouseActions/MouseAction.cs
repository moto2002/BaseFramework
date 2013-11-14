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
				return m_kActionState;
			}
			
			set
			{
				if ( m_kActionState != value || value == MouseActionState.MouseActionChanged )
				{
					m_kActionState = value;
				
					switch ( m_kActionState )
					{
						default:
						{
							break;
						}
						
						case MouseActionState.MouseActionBegan:
						case MouseActionState.MouseActionChanged:
						{
							actionDelegate( this );
							break;
						}
							
						case MouseActionState.MouseActionEnded:
						case MouseActionState.MouseActionRecognised:
						{
							//Return to 'Possible' state.
							actionDelegate( this );
							
							ResetAction();
							state = MouseActionState.MouseActionPossible;
							break;
						}
						
						case MouseActionState.MouseActionFailed:
						{
							// TODO: Return to 'Possible' state when no mouse presses remain.
							actionDelegate( this );
							
							ResetAction();
							state = MouseActionState.MouseActionPossible;
							break;
						}
					}
				}
			}
		}
		
		public MouseAction( Collider xCollider, MouseActionDelegate xDelegate )
		{
			collider = xCollider;
			actionDelegate = xDelegate;
		}
		
		public void ReceivedCursorPosition( Vector2 pxCursorPosition, bool bIntersectsWithCollider )
		{
			MouseActionState kActionState = m_kActionState;
			if ( bIntersectsWithCollider )
			{
				switch ( kActionState )
				{
					case MouseActionState.MouseActionPossible:
					case MouseActionState.MouseActionBegan:
					case MouseActionState.MouseActionChanged:
					{
						OnHover( pxCursorPosition );
						break;
					}
					
					case MouseActionState.MouseActionEnded:
					case MouseActionState.MouseActionRecognised:
					{
						ResetAction();
						state = MouseActionState.MouseActionPossible;
						break;
					}
					
					case MouseActionState.MouseActionFailed:
					{
						//TODO: Only Reset after all mouse buttons have been released.
						ResetAction();
						state = MouseActionState.MouseActionPossible;
						break;
					}
				}
			}
			else if ( kActionState > MouseActionState.MouseActionPossible )
			{
				state = MouseActionState.MouseActionFailed;
			}
		}
		
		public void ReceivedButtonPressed( int iButtonIndex, bool bIntersectsWithCollider )
		{
			MouseActionState kActionState = m_kActionState;
			switch ( kActionState )
			{
				case MouseActionState.MouseActionPossible:
				{
					if ( bIntersectsWithCollider )
					{
						OnButtonPressed( iButtonIndex );
					}
					break;
				}

				case MouseActionState.MouseActionBegan:
				case MouseActionState.MouseActionChanged:
				{
					OnButtonPressed( iButtonIndex );
					break;
				}
			}
		}
		
		public void ReceivedButtonReleased( int iButtonIndex, bool bIntersectsWithCollider )
		{
			MouseActionState kActionState = m_kActionState;
			switch ( kActionState )
			{
				case MouseActionState.MouseActionPossible:
				{
//					if ( bIntersectsWithCollider )
					{
						OnButtonReleased( iButtonIndex );
					}
					break;
				}

				case MouseActionState.MouseActionBegan:
				case MouseActionState.MouseActionChanged:
				{
					OnButtonReleased( iButtonIndex );
					break;
				}
			}
		}
		
		#region Protected Virtual Methods
		
		protected virtual void OnButtonPressed( int iButtonID )
		{
			focus = Input.mousePosition;
		}
		
		protected virtual void OnButtonReleased( int iButtonID )
		{
			focus = Input.mousePosition;
		}
		
		protected virtual void OnHover( Vector3 pxCursorPosition )
		{
			focus = Input.mousePosition;
		}
		
		protected virtual void ResetAction()
		{
			state = MouseActionState.MouseActionPossible;
		}
		
		#endregion
		
		private MouseActionState m_kActionState;
	}
}