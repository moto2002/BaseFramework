using UnityEngine;

namespace BaseFramework.MouseInput
{
	public class MouseClickAction : MouseAction
	{
		public int clickButton;
		public float clickTime;
		
		public MouseClickAction( Collider xCollider, MouseActionDelegate xDelegate ) : base( xCollider, xDelegate )
		{
			clickButton = 0;
			clickTime = 0.3f;
		}
		
		public override void OnButtonPressed( int iButtonID )
		{
			if ( iButtonID == clickButton )
			{
				base.OnButtonPressed( iButtonID );
				m_startTime = Time.time;
			}
		}
		
		public override void OnButtonReleased( int iButtonID )
		{
			if ( iButtonID == clickButton )
			{
				base.OnButtonReleased( iButtonID );
				
				float fCurrTime = Time.time;
				float fTimeElapsed = fCurrTime - m_startTime;
				
				if ( fTimeElapsed < clickTime )
				{
					state = MouseActionState.MouseActionRecognised;
				}
			}
		}
		
		private float m_startTime;
	}
}