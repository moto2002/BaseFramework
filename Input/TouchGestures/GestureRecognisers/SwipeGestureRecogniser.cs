using UnityEngine;


namespace BaseFramework.Gestures
{
	/// <summary>
	/// A SwipeGesture looks for multiple(?) touches in the same
	/// direction, and returns their direction and speed.
	/// </summary>
//	public class SwipeGestureRecogniser : GestureRecogniser
//	{
//		public Vector2 Velocity
//		{
//			get { return m_pxVelocityVector; }
//		}
//		
////		public float SwipeDuration
////		{
////			get { return m_fMaximumGestureTime;  }
////			set { m_fMaximumGestureTime = value; }
////		}
//		
//		public SwipeGestureRecogniser( Collider pxCollider, GestureRecogniserDelegate pxDelegate ) : base( pxCollider, pxDelegate )
//		{
//			m_fMaximumTimeStationary = 0.25f;
//			m_fMinimumMoveDistance = 62.0f; // Pixels
//		}
//		
//		protected override void InputBegan( Touch pxTouch )
//		{
//			base.InputBegan( pxTouch );
//			
//			m_fStartTime = Time.time;
//			m_pxInitialTouchPosition = Focus;
//		}
//		
////		protected override void InputChanged( Touch pxTouch )
////		{
////			base.InputChanged( pxTouch );
////			
////			bool bValidInput = ValidGestureInput( pxTouch );
////			if ( !bValidInput )
////			{
////				gestureState = GestureState.GestureStateFailed;
////			}
////		}
////		
////		protected override void InputCancelled( Touch pxTouch )
////		{
////			base.InputCancelled( pxTouch );
////			gestureState = GestureState.GestureStateFailed;
////		}
//		
//		protected override void InputStationary( Touch pxTouch )
//		{
//			base.InputStationary( pxTouch );
//			
//			float fTimePassed = Time.time - m_fStartTime;
//			bool bTooMuchTimePassed = fTimePassed >= m_fMaximumTimeStationary;
//			
//			if ( bTooMuchTimePassed )
//			{
//				State = GestureState.GestureStateFailed;
//			}
//		}
//		
//		protected override void InputEnded( Touch pxTouch )
//		{
//			base.InputEnded( pxTouch );
//			
//			Vector2 pxDistanceTravelled = pxTouch.position - m_pxInitialTouchPosition;
//						
//			float fMagnitudeOfDistance = pxDistanceTravelled.magnitude;
//			if ( fMagnitudeOfDistance > m_fMinimumMoveDistance )
//			{
//				float fTimeElapsed = Time.time - m_fStartTime;
//				m_pxVelocityVector = pxDistanceTravelled / fTimeElapsed;
//				State = GestureState.GestureStateRecognised;
//			}
//			else
//			{
//				State = GestureState.GestureStateFailed;
//			}
//		}
//		
////		private bool ValidGestureInput( Touch pxTouch )
////		{
////			float fDistanceMoved = pxTouch.deltaPosition.magnitude;
////			float fTimeElapsed = Time.time - m_fStartTime;
////			
////			bool bTimedOut  = fTimeElapsed >= m_fMaximumGestureTime;
////			bool bNotASwipe = fDistanceMoved > m_fMinimumMoveDistance;
////			
////			Debug.Log( bTimedOut + "/" + bNotASwipe );
////			Debug.Log( fDistanceMoved + ">" + m_fMinimumMoveDistance );
////			
////			return !bNotASwipe && !bTimedOut;
////		}
////		
//		
//		private float m_fMinimumMoveDistance;
//		
//		private float m_fMaximumTimeStationary;
//		private float m_fStartTime;
//		
//		private Vector2 m_pxInitialTouchPosition;
//		private Vector2 m_pxVelocityVector;
//	}
}