using UnityEngine;

namespace BaseFramework.Gestures
{
	public class TapGestureRecogniser : GestureRecogniser
	{
		public float gestureTapTime;
		
		//TODO: Multi-tap support
		//TODO: Movement validation
		
		public TapGestureRecogniser( Collider xCollider, GestureRecogniserDelegate xDelegate ) : base( xCollider, xDelegate )
		{
			gestureTapTime = 0.3f;
		}
		
		protected override void InputBegan( Touch pxTouch )
		{
			m_fGestureStartTime = Time.time;
		}
		
		protected override void InputEnded( Touch pxTouch )
		{
			float fCurrentTime = Time.time;
			float fTimeElapsed = fCurrentTime - m_fGestureStartTime;
			
			if ( fTimeElapsed <= gestureTapTime )
			{
				gestureState = GestureState.GestureStateRecognised;
			}
		}
		
		private float m_fGestureStartTime;
	}
}