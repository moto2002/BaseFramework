using UnityEngine;

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
				return m_state;
			}
			
			set
			{
				m_state = value;
				
				if ( gestureDelegate != null &&
					m_state != GestureState.GestureStatePossible )
				{
					gestureDelegate( this );
				}
			}
		}
		
		public GestureRecogniser( Collider xCollider, GestureRecogniserDelegate xDelegate )
		{
			numberOfTouches = 1;
			gestureCollider = xCollider;
			gestureDelegate = xDelegate;
		}
		
		public virtual void InputBegan( Touch[] xTouches )
		{
			CalculateFocusFromAverageWithTouches( xTouches );
		}
		
		public virtual void InputStationary( Touch[] xTouches )
		{
		}
		
		public virtual void InputChanged( Touch[] xTouches )
		{
			CalculateFocusFromAverageWithTouches( xTouches );
		}
		
		public virtual void InputEnded( Touch[] xTouches )
		{
		}
		
		public virtual void InputCancelled( Touch[] xTouches )
		{
		}
		
		public virtual void ResetGesture()
		{
		}
		
		private void CalculateFocusFromAverageWithTouches( Touch[] xTouches )
		{
			focus = Vector2.zero;
			foreach ( Touch xTouch in xTouches )
			{
				Vector2 xTouchPoint = xTouch.position;
				focus += xTouchPoint;
			}
			focus /= xTouches.Length;
		}
		
		private GestureState m_state;
	}
}