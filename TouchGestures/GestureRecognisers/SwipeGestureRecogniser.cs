using UnityEngine;
using System.Collections;


namespace BaseFramework.Gestures
{
	/// <summary>
	/// A SwipeGesture looks for multiple(?) touches in the same
	/// direction, and returns their direction and speed.
	/// </summary>
	public class SwipeGestureRecogniser : GestureRecogniser
	{
		public Vector2 Velocity {
			get { return m_pxVelocityVector; }
		}
		
		public float SwipeDuration {
			get { return m_fMaximumGestureTime;  }
			set { m_fMaximumGestureTime = value; }
		}

		protected override IEnumerator ProcessTouchGesture()
		{
			m_fStartTime = Time.time;
			m_pxInitialTouchPosition = Focus;
			Vector2 pxCurrentPosition = m_pxInitialTouchPosition;
			
			bool bHasTouches = Input.touchCount > 0;
			while ( bHasTouches )
			{
				pxCurrentPosition = Focus;
				
				bool bTimedOut = SwipeHasTimedOut();
				bool bNotASwipe = false;
				
				bool bHasTargetSwipeAngle = m_pxTargetSwipe != Vector2.zero;
				if ( bHasTargetSwipeAngle )
				{
					bNotASwipe = SwipeIsNotValid();
				}
				else
				{
					m_iNumberOfFramesPassed++;
					bool bShouldDetermineAngle = m_iNumberOfFramesPassed >= m_iNumberOfFramesToDetermineAngle;
					if ( bShouldDetermineAngle )
					{
						Vector2 pxCurrentSwipe = pxCurrentPosition - m_pxInitialTouchPosition;
						m_pxTargetSwipe = pxCurrentSwipe.normalized;
					}
				}
				
				if ( bTimedOut || bNotASwipe )
				{
					State = GestureState.GestureStateFailed;
					break;
				}
				
				bHasTouches = Input.touchCount > 0;
				m_pxLastTouchPosition = pxCurrentPosition;
				
				yield return null;
			}
			
			if ( State != GestureState.GestureStateFailed )
			{
				Vector2 pxDistanceTravelled = m_pxLastTouchPosition - m_pxInitialTouchPosition;
				float fTimeElapsed = Time.time - m_fStartTime;
				
				m_pxVelocityVector = pxDistanceTravelled / fTimeElapsed;
				State = GestureState.GestureStateRecognised;
			}
		}
		
		protected override IEnumerator ProcessMouseGesture()
		{
			m_fStartTime = Time.time;
			m_pxInitialTouchPosition = Focus;
			Vector2 pxCurrentPosition = m_pxInitialTouchPosition;

			bool bHasMouseDown = Input.GetMouseButton( 0 );
			while ( bHasMouseDown )
			{
				pxCurrentPosition = Focus;

				bool bTimedOut = SwipeHasTimedOut();
				bool bNotASwipe = false;

				bool bHasTargetSwipeAngle = m_pxTargetSwipe != Vector2.zero;
				if ( bHasTargetSwipeAngle )
				{
					bNotASwipe = SwipeIsNotValid();
				}
				else
				{
					m_iNumberOfFramesPassed++;
					bool bShouldDetermineAngle = m_iNumberOfFramesPassed >= m_iNumberOfFramesToDetermineAngle;
					if ( bShouldDetermineAngle )
					{
						Vector2 pxCurrentSwipe = pxCurrentPosition - m_pxInitialTouchPosition;
						m_pxTargetSwipe = pxCurrentSwipe.normalized;
					}
				}

				if ( bTimedOut || bNotASwipe )
				{
					State = GestureState.GestureStateFailed;
					break;
				}

				bHasMouseDown = Input.GetMouseButton( 0 );
				m_pxLastTouchPosition = pxCurrentPosition;

				yield return null;
			}

			if ( State != GestureState.GestureStateFailed )
			{
				Vector2 pxDistanceTravelled = m_pxLastTouchPosition - m_pxInitialTouchPosition;
				float fTimeElapsed = Time.time - m_fStartTime;

				m_pxVelocityVector = pxDistanceTravelled / fTimeElapsed;
				State = GestureState.GestureStateRecognised;
			}
		}

		protected override void ResetGesture()
		{
			m_pxTargetSwipe = Vector2.zero;
			m_iNumberOfFramesPassed = 0;
			m_iNumberOfFramesToDetermineAngle = 3;
			m_fMaximumGestureTime = 0.25f;
			m_fSwipeAngleAccuracy = 20.0f; // Swipe can vary by 5 degrees.
		}

		#region Helper Methods

		private bool SwipeHasTimedOut()
		{
			float fTimeElapsed = Time.time - m_fStartTime;
			
			bool bTimedOut  = fTimeElapsed >= m_fMaximumGestureTime;
			if ( DebugEnabled && bTimedOut )
			{
				Debug.Log( "SwipeGestureRecogniser (" + this + ") timed out!" );
			}

			return bTimedOut;
		}

		private bool SwipeIsNotValid()
		{
			Vector2 pxLastSwipeVector = m_pxLastTouchPosition - m_pxInitialTouchPosition;
			Vector2 pxTargetSwipeVector = m_pxTargetSwipe;

			float fSwipeAngleDifference = Vector2.Angle( pxTargetSwipeVector, pxLastSwipeVector );
			float fSwipeAngleAccuracy = m_fSwipeAngleAccuracy;

			bool bNotASwipe = !(fSwipeAngleDifference <= fSwipeAngleAccuracy && fSwipeAngleAccuracy >= -fSwipeAngleAccuracy);

			if ( DebugEnabled && bNotASwipe )
			{
				Debug.Log( "SwipeGestureRecogniser (" + this + ") is not a swipe!" );
			}
			
			return bNotASwipe;
		}

		#endregion

		public float m_fMaximumGestureTime;

		private int m_iNumberOfFramesToDetermineAngle;
		private int m_iNumberOfFramesPassed;

		private float m_fStartTime;
		private Vector2 m_pxVelocityVector;

		private float m_fSwipeAngleAccuracy;
		private Vector2 m_pxTargetSwipe;

		private Vector2 m_pxLastTouchPosition;
		private Vector2 m_pxInitialTouchPosition;
	}
}