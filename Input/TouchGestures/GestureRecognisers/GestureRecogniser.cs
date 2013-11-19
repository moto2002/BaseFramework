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
		
		public bool DebugEnabled
		{
			get { return m_bDebuggingEnabled;  }
			set { m_bDebuggingEnabled = value; }
		}
		
		public GestureState gestureState
		{
			get
			{
				return m_kGestureState;
			}
			
			set
			{
				if ( m_kGestureState != value || value == GestureState.GestureStateChanged )
				{
					m_kGestureState = value;
					
					switch ( m_kGestureState )
					{
						default:
						{
							break;
						}
						
						case GestureState.GestureStateChanged:
						{
							InformDelegates();
							break;
						}
						
						case GestureState.GestureStateEnded:
						case GestureState.GestureStateRecognised:
						{
							InformDelegates();
						
							//Return to 'Possible' state.
							ResetGesture();
							
							if ( m_pxBuffer != null )
							{
								gestureState = GestureState.GestureStatePossible;
							}
							else
							{
								gestureState = GestureState.GestureStateWaiting;
							}
							
							if ( m_bDebuggingEnabled )
							{
								Debug.Log( "Gesture State: " + gestureState + "(" + this + ")" );
							}
							
							break;
						}
						
						case GestureState.GestureStateFailed:
						case GestureState.GestureStateCancelled: // Will Reset immediately!!!!
						{
							InformDelegates();
							
							// Return to 'Possible' state when no touches remain.
//							if ( m_pxActiveTouches.Count == 0 ) //numberOfTouches == 0 )
							{
								ResetGesture();
								
								if ( m_pxBuffer == null )
								{
									gestureState = GestureState.GestureStatePossible;
								}
								else
								{
									gestureState = GestureState.GestureStateWaiting;
								}
							
								if ( m_bDebuggingEnabled )
								{
									Debug.Log( "Gesture State: " + gestureState + "(" + this + ")" );
								}
							}
							break;
						}
					}
				}
			}
		}
		
		public GestureRecogniser( Collider pxCollider, GestureRecogniserDelegate pxDelegate )
		{
			m_pxActiveTouches = new List<Touch>();
			m_pxGestureDelegates = new List<GestureRecogniserDelegate>();
			
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
				if ( m_bDebuggingEnabled )
				{
					Debug.Log( "Buffering Touch (" + this + ")\n Touch Phase: " + kTouchPhase );
				}
				m_pxBuffer.QueueTouch( pxTouch, bIntersectsWithCollider );
			}
			else
			{
				if ( bIntersectsWithCollider )
				{
					if (   gestureState == GestureState.GestureStatePossible
						|| gestureState == GestureState.GestureStateBegan
						|| gestureState == GestureState.GestureStateChanged )
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
						
						if ( m_bDebuggingEnabled )
						{
							Debug.Log( "Received Touch (" + this + ")\nGesture State: " + gestureState + "\n Touch Phase: " + kTouchPhase );
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
			m_pxActiveTouches.Add( pxTouch );
			CalculateFocusFromActiveTouches();
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
			m_pxActiveTouches.Remove( pxTouch );
			CalculateFocusFromActiveTouches();
		}
		
		protected virtual void InputCancelled( Touch pxTouch )
		{
			m_pxActiveTouches.Remove( pxTouch );
			CalculateFocusFromActiveTouches();
		}
		
		protected virtual void ResetGesture()
		{
			m_pxActiveTouches.Clear();
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
		
		private void CalculateFocusFromActiveTouches()
		{
			focus = Vector2.zero;
			foreach ( Touch pxTouch in m_pxActiveTouches )
			{
				Vector2 pxTouchPoint = pxTouch.position;
				focus += pxTouchPoint;
			}
			focus /= m_pxActiveTouches.Count;
		}
		
		#endregion
		
		private bool m_bDebuggingEnabled;
		private GestureState m_kGestureState;
		private GestureRecogniserBuffer m_pxBuffer;
		private List<Touch> m_pxActiveTouches;
		private List<GestureRecogniserDelegate> m_pxGestureDelegates;
	}
}