using UnityEngine;
using System.Collections;

namespace BaseFramework.MouseInput
{
	public class MouseSwipeAction : MouseAction
	{
		public Vector3 Velocity { get { return m_pxVelocity; } }
		
		public float SwipeTime
		{
			get { return m_fMaximumGestureTime;  }
			set { m_fMaximumGestureTime = value; }
		}
		
		public float MinimumMove
		{
			get { return m_fMinimumMoveDistance;  }
			set { m_fMinimumMoveDistance = value; }
		}
		
		public MouseSwipeAction( Collider pxCollider, MouseActionDelegate pxDelegate ) : base( pxCollider, pxDelegate )
		{
			m_fMaximumGestureTime  = 1.25f;
			m_fMinimumMoveDistance = 2.0f;
		}
		
		protected override void OnButtonPressed( int iButtonID )
		{
			base.OnButtonPressed( iButtonID );
			
			m_fStartTime = Time.time;
			m_pxInitialPosition = focus;
		}
		
		protected override void OnHover( Vector3 pxCursorPosition )
		{
			base.OnHover( pxCursorPosition );
			
			float fDistanceMoved = delta.magnitude;
			float fTimeElapsed = Time.time - m_fStartTime;
			
			bool bTimedOut  = fTimeElapsed >= m_fMaximumGestureTime;
			bool bNotASwipe = fDistanceMoved < m_fMinimumMoveDistance;
			if ( bNotASwipe || bTimedOut )
			{
				state = MouseActionState.MouseActionFailed;
			}
		}
		
		protected override void OnButtonReleased( int iButtonID )
		{
			base.OnButtonReleased( iButtonID );
			
			float fTimeElapsed = Time.time - m_fStartTime;
			bool bSuccessfulSwipe = fTimeElapsed < m_fMaximumGestureTime;
			if ( bSuccessfulSwipe )
			{
				Vector3 pxDisplacement = focus - m_pxInitialPosition;
				m_pxVelocity = pxDisplacement / fTimeElapsed;
				
				state = MouseActionState.MouseActionRecognised;
			}
			else
			{
				state = MouseActionState.MouseActionFailed;
			}
		}
		
		protected override void ResetAction()
		{
			base.ResetAction();
			
			m_pxVelocity = Vector3.zero;
			m_pxInitialPosition = Vector3.zero;
		}
		
		private float m_fStartTime;
		private float m_fMaximumGestureTime;
		private float m_fMinimumMoveDistance;
		
		private Vector3 m_pxVelocity;
		private Vector3 m_pxInitialPosition;
	}
}