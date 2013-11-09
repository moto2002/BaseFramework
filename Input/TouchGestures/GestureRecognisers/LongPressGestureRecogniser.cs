using UnityEngine;

namespace BaseFramework.Gestures
{
	public class LongPressGestureRecogniser : GestureRecogniser
	{
		public float beginDelay;
		
		public Vector2 moveDelta
		{
			get { return m_pxMoveDelta; }
		}
		
		public LongPressGestureRecogniser( Collider xCollider, GestureRecogniserDelegate xDelegate ) : base( xCollider, xDelegate )
		{
			beginDelay = 0.5f;
			m_fStartTime = -1.0f;
		}
		
		public override void InputBegan( Touch[] xTouches )
		{
			base.InputBegan( xTouches );
			m_fStartTime = Time.time;
		}
			
		public override void InputStationary( Touch[] xTouches )
		{
			if ( m_fStartTime >= 0.0f )
			{
				float fCurrentTime = Time.time;
				float fTimeElapsed = fCurrentTime - m_fStartTime;
				
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
				
				Vector2 pxDifference = xTouches[0].deltaPosition;
				m_pxMoveDelta = pxDifference;
				
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
			
			m_fStartTime  = -1.0f;
			m_pxMoveDelta = Vector3.zero;
		}
		
		private float m_fStartTime;
		private Vector2 m_pxMoveDelta;
	}
}