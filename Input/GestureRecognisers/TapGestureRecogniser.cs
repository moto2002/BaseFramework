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
		
		public override void InputBegan( Touch[] touches )
		{
			m_fGestureStartTime = Time.time;
		}
		
		public override void InputEnded( Touch[] touches )
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