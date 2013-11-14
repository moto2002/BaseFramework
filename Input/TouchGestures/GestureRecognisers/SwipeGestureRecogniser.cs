using UnityEngine;


namespace BaseFramework.Gestures
{
	/// <summary>
	/// A SwipeGesture looks for multiple(?) touches in the same
	/// direction, and returns their direction and speed.
	/// </summary>
	public class SwipeGestureRecogniser : GestureRecogniser
	{
		public Vector2 Velocity
		{
			get { return m_pxVelocityVector; }
		}
		
		public float SwipeDuration
		{
			get { return m_fMaxSwipeDuration;  }
			set { m_fMaxSwipeDuration = value; }
		}
		
		public SwipeGestureRecogniser( Collider pxCollider, GestureRecogniserDelegate pxDelegate ) : base( pxCollider, pxDelegate )
		{
		}
		
		protected override void InputBegan( Touch pxTouch )
		{
			base.InputBegan( pxTouch );
			
			m_fStartTime = Time.time;
			m_pxInitialTouchPosition = focus;
		}
		
		protected override void InputChanged( Touch pxTouch )
		{
			base.InputChanged( pxTouch );
		}
		
		protected override void InputCancelled( Touch pxTouch )
		{
			base.InputCancelled( pxTouch );
		}
		
		protected override void InputStationary( Touch pxTouch )
		{
			base.InputStationary( pxTouch );
		}
		
		protected override void InputEnded( Touch pxTouch )
		{
			base.InputEnded( pxTouch );
			
			float fTimeElapsed = Time.time - m_fStartTime;
			if ( fTimeElapsed < m_fMaxSwipeDuration )
			{
				Vector2 pxDistanceTravelled = focus - m_pxInitialTouchPosition;
				
				float fMagnitudeOfDistance = pxDistanceTravelled.magnitude;
				if ( fMagnitudeOfDistance > 0.0f )
				{
					m_pxVelocityVector = pxDistanceTravelled / fTimeElapsed;
					gestureState = GestureState.GestureStateRecognised;
				}
				else
				{
					gestureState = GestureState.GestureStateFailed;
				}
			}
			else
			{
				gestureState = GestureState.GestureStateFailed;
			}
		}
		
		
		private static float m_fMaxSwipeDuration = 0.75f;
//		private static float m_maxAngularChangeDeg = 5.0f;
		
		private float m_fStartTime;
		
		private Vector2 m_pxInitialTouchPosition;
		private Vector2 m_pxVelocityVector;
	}
}