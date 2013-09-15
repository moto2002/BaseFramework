using UnityEngine;

namespace BaseFramework.Gestures
{
	public class LongPressGestureRecogniser : GestureRecogniser
	{
		public float beginDelay;
		
		public LongPressGestureRecogniser( Collider xCollider, GestureRecogniserDelegate xDelegate ) : base( xCollider, xDelegate )
		{
			beginDelay = 0.5f;
			m_gestureStartTime = -1.0f;
		}
		
		public override void InputBegan( Touch[] xTouches )
		{
			base.InputBegan( xTouches );
			m_gestureStartTime = Time.time;
		}
			
		public override void InputStationary( Touch[] xTouches )
		{
			if ( m_gestureStartTime >= 0.0f )
			{
				float fCurrentTime = Time.time;
				float fTimeElapsed = fCurrentTime - m_gestureStartTime;
				
				if ( fTimeElapsed >= beginDelay )
				{
					gestureState = GestureState.GestureStateBegan;
				}
			}
		}
		
		public override void InputChanged( Touch[] xTouches )
		{
			if ( gestureState == GestureState.GestureStatePossible )
			{
				gestureState = GestureState.GestureStateFailed;
			}
			else if ( gestureState == GestureState.GestureStateBegan ||
				gestureState == GestureState.GestureStateChanged )
			{
				base.InputChanged( xTouches );
				gestureState = GestureState.GestureStateChanged;
			}
		}
		
		public override void InputEnded( Touch[] xTouches )
		{
			base.InputEnded( xTouches );
			
			if ( gestureState == GestureState.GestureStateBegan ||
				gestureState == GestureState.GestureStateChanged )
			{
				gestureState = GestureState.GestureStateEnded;
			}
		}
		
		public override void ResetGesture()
		{
			base.ResetGesture ();
			m_gestureStartTime = -1.0f;
		}
		
		private float m_gestureStartTime;
	}
}