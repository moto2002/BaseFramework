using UnityEngine;
using System.Collections.Generic;

namespace BaseFramework.Gestures
{
	public class GestureRecogniserBuffer
	{
		struct TouchInfo
		{
			public Touch m_pxTouch;
			public bool m_bCollided;
			
			public TouchInfo( Touch pxTouch, bool bCollided )
			{
				m_pxTouch = pxTouch;
				m_bCollided = bCollided;
			}
		}
		
		public GestureRecogniserBuffer( GestureRecogniser pxGestureToFail, GestureRecogniser pxGestureWaiting )
		{
			m_pxTouchQueue = new Queue<TouchInfo>();
//			m_pxGestureToFail = pxGestureToFail;
			m_pxGestureWaiting = pxGestureWaiting;
			
			pxGestureToFail.AddDelegate( FailGestureHandler );
		}
		
		public void QueueTouch( Touch pxTouch, bool bCollided )
		{
			TouchInfo pxInfo = new TouchInfo( pxTouch, bCollided );
			m_pxTouchQueue.Enqueue( pxInfo );
		}
		
		private void DequeueTouches()
		{
			while ( m_pxTouchQueue.Count > 0 )
			{
				TouchInfo pxTouchInfo = m_pxTouchQueue.Dequeue();
				
				Touch pxTouch  = pxTouchInfo.m_pxTouch;
				bool bCollided = pxTouchInfo.m_bCollided;
				m_pxGestureWaiting.ReceiveTouch( pxTouch, bCollided );
			}
		}
		
		private void FailGestureHandler( GestureRecogniser pxGestureToFail )
		{
			if ( pxGestureToFail.gestureState == GestureState.GestureStateFailed )
			{
				m_pxGestureWaiting.gestureState = GestureState.GestureStatePossible;
				DequeueTouches();
			}
			if ( pxGestureToFail.gestureState == GestureState.GestureStateRecognised
				|| pxGestureToFail.gestureState == GestureState.GestureStateEnded 
				|| pxGestureToFail.gestureState == GestureState.GestureStateCancelled )
			{
				m_pxTouchQueue.Clear();
			}
		}
		
//		private GestureRecogniser m_pxGestureToFail;
		private GestureRecogniser m_pxGestureWaiting;
		
		private Queue<TouchInfo> m_pxTouchQueue;
	}
}
