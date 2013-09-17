using UnityEngine;

namespace BaseFramework.MouseInput
{
	public class MouseDragAction : MouseAction
	{
		public float beginDelay;
		public int clickButton;
		
		public MouseDragAction( Collider xCollider, MouseActionDelegate xDelegate ) : base( xCollider, xDelegate )
		{
			clickButton = 0;
			beginDelay = 0.0f;
		}
		
		public override void OnButtonPressed( int iButtonID )
		{
			if ( clickButton == iButtonID )
			{
				base.OnButtonPressed( iButtonID );
				m_startTime = Time.time;
			}
		}
		
		public override void OnHover( Vector3 xCursorPosition )
		{
			if ( state == MouseActionState.MouseActionPossible )
			{
				base.OnHover( xCursorPosition );
				
				float fCurrentTime = Time.time;
				float fTimeElapsed = m_startTime - fCurrentTime;
				
				if ( fTimeElapsed > m_startTime )
				{
					state = MouseActionState.MouseActionBegan;
				}
			}
			else if ( state == MouseActionState.MouseActionBegan ||
						state == MouseActionState.MouseActionChanged )
			{
				base.OnHover( xCursorPosition );
				state = MouseActionState.MouseActionChanged;
			}
		}
		
		public override void OnButtonReleased( int iButtonID )
		{
			if ( clickButton == iButtonID )
			{
				base.OnButtonReleased( iButtonID );
				state = MouseActionState.MouseActionEnded;
			}
		}
		
		private float m_startTime;
	}
}
