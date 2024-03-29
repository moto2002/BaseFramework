using UnityEngine;
using System.Collections;

namespace BaseFramework.Gestures
{
	public class TapGestureRecogniser : GestureRecogniser
	{
		public float gestureTapTime = 0.3f;
		
		//TODO: Multi-tap support
		//TODO: Movement validation
		
		protected override IEnumerator ProcessTouchGesture()
		{
			m_fGestureStartTime = Time.time;
			
			bool bTimedOut = false;
			while ( Input.touchCount > 0 )
			{
				bTimedOut = HasTimedOut();
				if ( bTimedOut )
				{
					State = GestureState.GestureStateFailed;
				}
				yield return null;
			}
			
			if ( !bTimedOut )
			{
				State = GestureState.GestureStateRecognised;
			}
		}
		
		protected override IEnumerator ProcessMouseGesture()
		{
			m_fGestureStartTime = Time.time;
			
			bool bTimedOut = false;
			bool bMouseButtonDown = !Input.GetMouseButton( 0 );
			while ( bMouseButtonDown )
			{
				bTimedOut = HasTimedOut();
				Debug.Log( bTimedOut );
				if ( bTimedOut )
				{
					State = GestureState.GestureStateFailed;
				}
				yield return null;
			}
			
			if ( !bTimedOut )
			{
				State = GestureState.GestureStateRecognised;
			}
		}

		protected override void ResetGesture()
		{
			m_fGestureStartTime = 0.0f;
		}
		
		private bool HasTimedOut()
		{
			float fCurrentTime = Time.time;
			float fTimeElapsed = fCurrentTime - m_fGestureStartTime;
			
			return fTimeElapsed >= gestureTapTime;
		}
		
		private float m_fGestureStartTime;
	}
}