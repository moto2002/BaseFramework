using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BaseFramework.Gestures
{
    public class TapGestureRecogniser : GestureRecogniser
    {
        public float gestureTapTime = 0.3f;
		
        //TODO: Movement validation
		
        protected override IEnumerator ProcessTouchGesture()
        {
            m_fGestureStartTime = Time.time;
			
            foreach( Touch pxTouch in Input.touches )
            {
                Vector2 pxFocus = pxTouch.position;
                bool bValidTouch = InputIntersectsCollider( pxFocus );
                
                if ( bValidTouch )
                {
                    // Wait until this touch ends.
                    int iFingerID = pxTouch.fingerId;
                    m_pxValidFingerIDs.Add( iFingerID );
                }
            }
            
            bool bTimedOut = false;
            bool bValidFingersRemain = ValidFingersRemain();
            while( bValidFingersRemain )
            {
                bTimedOut = HasTimedOut();
                if( bTimedOut )
                {
                    State = GestureState.GestureStateFailed;
                }
                yield return null;
                
                bValidFingersRemain = ValidFingersRemain();
            }
			
            if( !bTimedOut )
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
				
				if ( bTimedOut )
				{
					State = GestureState.GestureStateFailed;
				}
				yield return null;
                
                bMouseButtonDown = !Input.GetMouseButton( 0 );
			}
			
			if ( !bTimedOut )
			{
				State = GestureState.GestureStateRecognised;
			}
		}

		protected override void ResetGesture()
		{
            m_pxValidFingerIDs = new List<int>();
            m_fGestureStartTime = 0.0f;
		}
		
		private bool HasTimedOut()
		{
			float fCurrentTime = Time.time;
			float fTimeElapsed = fCurrentTime - m_fGestureStartTime;
			
			return fTimeElapsed >= gestureTapTime;
		}
        
        private bool ValidFingersRemain()
        {
            bool bResult = false;
            foreach ( Touch pxTouch in Input.touches )
            {
                int iFingerID = pxTouch.fingerId;
                bool bValidFinger = m_pxValidFingerIDs.Contains( iFingerID );
                
                bResult |= bValidFinger;
            }
            return bResult;
        }
        
        private List<int> m_pxValidFingerIDs;
		private float m_fGestureStartTime;
	}
}