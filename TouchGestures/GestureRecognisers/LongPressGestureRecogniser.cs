using UnityEngine;
using System.Collections;

namespace BaseFramework.Gestures
{
	public class LongPressGestureRecogniser : GestureRecogniser
	{
		public Vector2 MoveDelta {
			get { return m_pxMoveDelta; }
		}

		protected override IEnumerator ProcessMouseGesture()
		{
			m_fStartTime = Time.time;
			m_pxInitialPosition = Focus;
			m_pxLastPosition = m_pxInitialPosition;

			bool bMouseButtonDown = Input.GetMouseButton( 0 );
			while ( bMouseButtonDown )
			{
				float fTimeHeld = TimeHeld();
				Vector2 pxCurrentPosition = Focus;

				bool bGestureBegan = fTimeHeld > m_fStartDelay;
				if ( bGestureBegan )
				{
					if ( State == GestureState.GestureStatePossible )
					{
						State = GestureState.GestureStateBegan;
					}
					else
					{
						Vector2 pxDelta = pxCurrentPosition - m_pxLastPosition;
						m_pxMoveDelta = pxDelta;

						State = GestureState.GestureStateChanged;
					}
				}
				else
				{
					Vector2 pxDifference = m_pxInitialPosition - pxCurrentPosition;
					float fDistanceMoved = pxDifference.magnitude;

					bool bMovedTooFar = fDistanceMoved > m_fInitialMovementToCancel;
					if ( bMovedTooFar )
					{
						State = GestureState.GestureStateFailed;
						break;
					}
				}
				m_pxLastPosition = pxCurrentPosition;
				bMouseButtonDown = Input.GetMouseButton( 0 );

				yield return null;
			}

			if ( State != GestureState.GestureStateFailed )
			{
				State = GestureState.GestureStateEnded;
			}
		}

		protected override IEnumerator ProcessTouchGesture()
		{
			m_fStartTime = Time.time;
			m_pxInitialPosition = Focus;
			m_pxLastPosition = m_pxInitialPosition;
			
			bool bTouchesExist = Input.touchCount > 0;
			while ( bTouchesExist )
			{
				float fTimeHeld = TimeHeld();
				Vector2 pxCurrentPosition = Focus;
				
				bool bGestureBegan = fTimeHeld > m_fStartDelay;
				if ( bGestureBegan )
				{
					if ( State == GestureState.GestureStatePossible )
					{
						State = GestureState.GestureStateBegan;
					}
					else
					{
						Vector2 pxDelta = pxCurrentPosition - m_pxLastPosition;
						m_pxMoveDelta = pxDelta;
						
						State = GestureState.GestureStateChanged;
					}
				}
				else
				{
					Vector2 pxDifference = m_pxInitialPosition - pxCurrentPosition;
					float fDistanceMoved = pxDifference.magnitude;
					
					bool bMovedTooFar = fDistanceMoved > m_fInitialMovementToCancel;
					if ( bMovedTooFar )
					{
						State = GestureState.GestureStateFailed;
						break;
					}
				}
				m_pxLastPosition = pxCurrentPosition;
				bTouchesExist = Input.touchCount > 0;
				
				yield return null;
			}
			
			if ( State != GestureState.GestureStateFailed )
			{
				State = GestureState.GestureStateEnded;
			}
		}

		protected override void ResetGesture()
		{
			m_fStartTime  = 0.0f;
			m_pxMoveDelta = Vector3.zero;
		}

		private float TimeHeld()
		{
			float fCurrentTime = Time.time;
			float fTimeElapsed = fCurrentTime - m_fStartTime;

			return fTimeElapsed;
		}

		public float m_fStartDelay;
		public float m_fInitialMovementToCancel;

		private float m_fStartTime;
		private Vector2 m_pxMoveDelta;
		private Vector2 m_pxInitialPosition;
		private Vector2 m_pxLastPosition;
	}
}