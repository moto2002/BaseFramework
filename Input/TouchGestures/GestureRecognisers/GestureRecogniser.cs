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
		GestureStateFailed
	};
	
	public abstract class GestureRecogniser
	{
		public Vector2 focus;
		public int numberOfTouches;
		
		public Collider gestureCollider;
		public GestureRecogniserDelegate gestureDelegate;
		
		public GestureState gestureState
		{
			get
			{
				return m_kGestureState;
			}
			
			set
			{
				if ( m_kGestureState != value )
				{
					m_kGestureState = value;
					
					switch ( m_kGestureState )
					{
						default:
						{
							break;
						}
							
						case GestureState.GestureStateEnded:
						case GestureState.GestureStateRecognised:
						{
							//Return to 'Possible' state.
							ResetGesture();
							gestureState = GestureState.GestureStatePossible;
							break;
						}
						
						case GestureState.GestureStateFailed:
						case GestureState.GestureStateCancelled:
						{
							// Return to 'Possible' state when no touches remain.
							if ( numberOfTouches == 0 )
							{
								ResetGesture();
								gestureState = GestureState.GestureStatePossible;
							}
							break;
						}
					}
				}
				
				if ( m_kGestureState != GestureState.GestureStatePossible )
				{
					gestureDelegate( this );
				}
			}
		}
		
		public GestureRecogniser( Collider pxCollider, GestureRecogniserDelegate pxDelegate )
		{
			m_pxActiveTouches = new List<Touch>();
			
			numberOfTouches = 1;
			gestureCollider = pxCollider;
			gestureDelegate = pxDelegate;
		}
		
		public void ReceiveTouch( Touch pxTouch, bool bIntersectsWithCollider )
		{
			TouchPhase kTouchPhase = pxTouch.phase;
			
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
			m_pxActiveTouches.Add( pxTouch );
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
		
		private GestureState m_kGestureState;
		private List<Touch> m_pxActiveTouches;
	}
}