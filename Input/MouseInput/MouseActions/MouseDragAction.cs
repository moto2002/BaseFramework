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
				state = MouseActionState.MouseActionBegan;
			}
		}
		
		public override void OnHover( Vector3 xCursorPosition )
		{
			if ( state == MouseActionState.MouseActionBegan )
			{
				base.OnHover( xCursorPosition );
				
				float fCurrentTime = Time.time;
				float fTimeElapsed = fCurrentTime - m_startTime;
				
				if ( fTimeElapsed > beginDelay )
				{
					state = MouseActionState.MouseActionChanged;
				}
			}
			else if ( state == MouseActionState.MouseActionChanged )
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
