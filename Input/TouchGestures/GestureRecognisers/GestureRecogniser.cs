using UnityEngine;
using System.Collections.Generic;

namespace BaseFramework.Gestures
{
	public delegate void GestureRecogniserDelegate( GestureRecogniser xGesture );
	
	public enum GestureState
	{
		GestureStatePossible,
		GestureStateBegan,
		GestureStateChanged,
		GestureStateEnded,
		GestureStateRecognised,
		GestureStateCancelled,
		GestureStateFailed,
		GestureStateWaiting
	};
	
	public abstract class GestureRecogniser
	{
		public Vector2 focus;
		public int numberOfTouches;
		
		public Collider gestureCollider;
		
		public bool DebugEnabled {
			get { return m_bDebuggingEnabled;  }
			set
			{
				m_bDebuggingEnabled = value;
				if ( m_bDebuggingEnabled )
				{
					// Log current state
					if ( m_bDebuggingEnabled )
					{
						Debug.Log( "Gesture State: " + gestureState + "(" + this + ")" );
					}
				}
			}
		}
		
		public GestureState gestureState
		{
			get { return m_kGestureState; }
			set
			{
				bool bGestureHasEnded = m_kGestureState == GestureState.GestureStateFailed 
										|| m_kGestureState == GestureState.GestureStateCancelled
										|| m_kGestureState == GestureState.GestureStateEnded;
				bool bGestureWillReset = value == GestureState.GestureStatePossible || value == GestureState.GestureStateWaiting;
				bool bGestureHasNoTouchesRegistered = m_iActiveTouches == 0;
				
				bool bGestureStateIsRepeatable = value == GestureState.GestureStateChanged;
				bool bGestureStateIsDifferent = value != m_kGestureState;
				
				bool bGestureIsDifferentAndNotFinished = (bGestureStateIsDifferent || bGestureStateIsRepeatable) && !bGestureHasEnded;
				bool bGestureWillTransitionFromEndedToPossible = (bGestureHasEnded && bGestureWillReset && bGestureHasNoTouchesRegistered);
								
				if ( bGestureIsDifferentAndNotFinished || bGestureWillTransitionFromEndedToPossible )
				{
					m_kGestureState = value;
					
					// Log the new state
					if ( m_bDebuggingEnabled )
					{
						Debug.Log( "Gesture State: " + gestureState + "(" + this + ")" );
					}
					
					// Perform Action depending on state
					switch ( m_kGestureState )
					{
						case GestureState.GestureStatePossible:
						case GestureState.GestureStateWaiting:
						{
							ResetGesture();
							break;
						}
						
						case GestureState.GestureStateBegan:
						case GestureState.GestureStateChanged:
						{
							InformDelegates();
							break;
						}
						
						case GestureState.GestureStateEnded:
						case GestureState.GestureStateRecognised:
						{
							InformDelegates();
							break;
						}
						
						case GestureState.GestureStateFailed:
						case GestureState.GestureStateCancelled:
						{
							InformDelegates();
							break;
						}
					}
				}
			}
		}
		
		public GestureRecogniser( Collider pxCollider, GestureRecogniserDelegate pxDelegate )
		{
			m_iMaxTouches = 4;
			m_pxActiveTouches = new Touch[ m_iMaxTouches ];
			m_pxGestureDelegates = new List<GestureRecogniserDelegate>();
			
			AddDelegate( pxDelegate );
			numberOfTouches = 1;
			gestureCollider = pxCollider;
		}
		
		public void AddDelegate( GestureRecogniserDelegate pxDelegate )
		{
			m_pxGestureDelegates.Add( pxDelegate );
		}
		
		public void RemoveDelegate( GestureRecogniserDelegate pxDelegate )
		{
			m_pxGestureDelegates.Remove( pxDelegate );
		}
		
		public void RequireGestureToFail( GestureRecogniser pxOther )
		{
			gestureState = GestureState.GestureStateWaiting;
			m_pxBuffer = new GestureRecogniserBuffer( pxOther, this );
		}
		
		public void ReceiveTouch( Touch pxTouch, bool bIntersectsWithCollider )
		{
			TouchPhase kTouchPhase = pxTouch.phase;
			if ( m_pxBuffer != null && gestureState == GestureState.GestureStateWaiting )
			{
				m_pxBuffer.QueueTouch( pxTouch, bIntersectsWithCollider );
			}
			else
			{
				if ( bIntersectsWithCollider )
				{
					switch ( kTouchPhase )
					{
						case TouchPhase.Began:
						{
							InputBegan( pxTouch );
							break;
						}
						
						case TouchPhase.Moved:
						{
							InputChanged( pxTouch );
							break;
						}
						
						case TouchPhase.Stationary:
						{
							InputStationary( pxTouch );
							break;
						}
						
						case TouchPhase.Ended:
						{
							InputEnded( pxTouch );
							break;
						}
						
						case TouchPhase.Canceled:
						{
							InputCancelled( pxTouch );
							break;
						}
					}
				}
				else
				{
					InputEnded( pxTouch );
				}
			}
		}
		
		#region Virtual Methods
		
		protected virtual void InputBegan( Touch pxTouch )
		{
			RegisterTouch( pxTouch );
		}
		
		protected virtual void InputStationary( Touch pxTouch )
		{
		}
		
		protected virtual void InputChanged( Touch pxTouch )
		{
			CalculateFocusFromActiveTouches();
		}
		
		protected virtual void InputEnded( Touch pxTouch )
		{
			RemoveTouch( pxTouch );
		}
		
		protected virtual void InputCancelled( Touch pxTouch )
		{
			RemoveTouch( pxTouch );
		}
		
		protected virtual void ResetGesture()
		{
		}
		
		#endregion
		
		#region Helper Methods
		
		private void InformDelegates()
		{
			foreach ( GestureRecogniserDelegate pxDelegate in m_pxGestureDelegates )
			{
				pxDelegate( this );
			}
		}
		
		private void RegisterTouch( Touch pxTouch )
		{
//			if ( m_bDebuggingEnabled )
//			{
//				Debug.Log( "Add Touch(" + this + "): " + pxTouch.fingerId );
//			}
			
			if ( pxTouch.fingerId <= m_iMaxTouches )
			{
				m_iActiveTouches++;
				int iTouchIndex = pxTouch.fingerId;
				m_pxActiveTouches[ iTouchIndex ] = pxTouch;
				
				CalculateFocusFromActiveTouches();
			}
		}
		
		private void RemoveTouch( Touch pxTouch )
		{
			if ( pxTouch.fingerId <= m_iMaxTouches )
			{
				m_iActiveTouches--;
				CalculateFocusFromActiveTouches();
				
				if ( m_iActiveTouches == 0 )
				{
					if ( m_pxBuffer == null )
					{
						gestureState = GestureState.GestureStatePossible;
					}
					else
					{
						gestureState = GestureState.GestureStateWaiting;
					}
				}
			}
		}
		
		private void CalculateFocusFromActiveTouches()
		{
			focus = Vector2.zero;
			foreach ( Touch pxTouch in m_pxActiveTouches )
			{
				Vector2 pxTouchPoint = pxTouch.position;
				focus += pxTouchPoint;
			}
			focus /= m_iActiveTouches;
		}
		
		#endregion
		
		private bool m_bDebuggingEnabled;
		private GestureState m_kGestureState;
		private GestureRecogniserBuffer m_pxBuffer;
		
		private int m_iMaxTouches;
		private int m_iActiveTouches;
		private Touch[] m_pxActiveTouches;
		private List<GestureRecogniserDelegate> m_pxGestureDelegates;
	}
}