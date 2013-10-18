using UnityEngine;

namespace BaseFramework.MouseInput
{
	public class MouseDragAction : MouseAction
	{
		public float beginDelay;
		public int clickButton;
		
		public Vector3 moveDelta
		{
			get { return m_pxMoveDelta; }
		}
		
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
				m_fStartTime = Time.time;
				state = MouseActionState.MouseActionBegan;
				m_pxMoveDelta = Vector3.zero;
			}
		}
		
		public override void OnHover( Vector3 xCursorPosition )
		{
			if ( state == MouseActionState.MouseActionBegan )
			{
				base.OnHover( xCursorPosition );
				
				float fCurrentTime = Time.time;
				float fTimeElapsed = fCurrentTime - m_fStartTime;
				
				if ( fTimeElapsed > beginDelay )
				{
					state = MouseActionState.MouseActionChanged;
				}
			}
			else if ( state == MouseActionState.MouseActionChanged )
			{
				Vector3 pxDifference = Input.mousePosition - focus;
				m_pxMoveDelta = pxDifference;
				m_pxMoveDelta.z = 0;
				
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
		
		public override void ResetAction()
		{
			base.ResetAction();
			m_pxMoveDelta = Vector3.zero;
		}
		
		private float m_fStartTime;
		private Vector3 m_pxMoveDelta;
	}
}
