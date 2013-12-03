using UnityEngine;

namespace BaseFramework.Gestures
{
//	public class LongPressGestureRecogniser : GestureRecogniser
//	{
//		public float beginDelay;
//		
//		public Vector2 moveDelta
//		{
//			get { return m_pxMoveDelta; }
//		}
//		
//		public LongPressGestureRecogniser( Collider xCollider, GestureRecogniserDelegate xDelegate ) : base( xCollider, xDelegate )
//		{
//			beginDelay = 0.5f;
//			m_fStartTime = -1.0f;
//		}
//		
//		protected override void InputBegan( Touch pxTouch )
//		{
//			base.InputBegan( pxTouch );
//			m_fStartTime = Time.time;
//		}
//			
//		protected override void InputStationary( Touch pxTouch )
//		{
//			if ( m_fStartTime >= 0.0f )
//			{
//				float fCurrentTime = Time.time;
//				float fTimeElapsed = fCurrentTime - m_fStartTime;
//				
//				bool bGestureHeld = fTimeElapsed >= beginDelay;
//				bool bGesturePossible = State == GestureState.GestureStatePossible;
//				if ( bGestureHeld && bGesturePossible )
//				{
//					State = GestureState.GestureStateBegan;
//				}
//			}
//		}
//		
//		protected override void InputChanged( Touch pxTouch )
//		{
//			if ( State == GestureState.GestureStatePossible )
//			{
//				State = GestureState.GestureStateFailed;
//			}
//			else if ( State == GestureState.GestureStateBegan ||
//				      State == GestureState.GestureStateChanged )
//			{
//				base.InputChanged( pxTouch );
//				
//				Vector2 pxDifference = pxTouch.deltaPosition;
//				m_pxMoveDelta = pxDifference;
//				
//				State = GestureState.GestureStateChanged;
//			}
//		}
//		
//		protected override void InputEnded( Touch pxTouch )
//		{
//			base.InputEnded( pxTouch );
//			
//			if ( State == GestureState.GestureStateBegan ||
//				State == GestureState.GestureStateChanged )
//			{
//				State = GestureState.GestureStateEnded;
//			}
//		}
//		
//		protected override void ResetGesture()
//		{
//			base.ResetGesture();
//			
//			m_fStartTime  = -1.0f;
//			m_pxMoveDelta = Vector3.zero;
//		}
//		
//		private float m_fStartTime;
//		private Vector2 m_pxMoveDelta;
//	}
}