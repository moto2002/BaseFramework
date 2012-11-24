using UnityEngine;
using System.Collections.Generic;

namespace GenericLib
{
	//TODO Abstract TouchGestures and implement a version for accepting mouse input?
	public class TouchGestures : MonoSingleton<TouchGestures>
	{	
		#region Helper Classes
		
		// Data Structure to hold information on what a finger is doing
		// This exists to abstract the Touch class for suture mouse support.
		// TODO: Rethink Finger Class!
		public class Finger
		{
			public int m_id;
			public float m_deltaTime;
			public Vector2 m_position;
			public Vector2 m_deltaPos;
			public TouchPhase m_phase;
		}
		
		// Data Structure to define a Gesture
		public class Gesture
		{
			int m_nFingers;
			Vector2[][] m_vertices;
		}
		
		#endregion
		
		#region Events
		
		// Delegates for returning Fingers / Gestures
		public delegate void TouchEvent(Finger f);
		public delegate void GestureEvent(Gesture g);
		
		// Touch & Gesture events thrown
		public event TouchEvent TouchBegan;
		public event TouchEvent TouchChanged;
		public event TouchEvent TouchEnded;
		
		public event GestureEvent GestureFound;
		
		#endregion
		
		#region Event Wrappers
		
		public void DoTouchBegan(Finger f)
		{
			if (TouchBegan != null)
				TouchBegan(f);
		}
		
		public void DoTouchChanged(Finger f)
		{
			if (TouchChanged != null)
				TouchChanged(f);
		}
		
		public void DoTouchEnded(Finger f)
		{
			if (TouchEnded != null)
				TouchEnded(f);
		}
		
		public void DoGestureFound(Gesture g)
		{
			if (GestureFound != null)
				GestureFound(g);
		}
		
		#endregion
		
		void Update()
		{
			int nTouches = Input.touchCount;
			
			for (int i=0; i<nTouches; i++)
			{
				// TODO: Only Update when there has been a change?
				// TODO: Store References of all touches?
				Touch t = Input.touches[i];
				
				switch (t.phase)
				{
					case TouchPhase.Began:
					{
						DoTouchBegan(WrapFinger(t));
						
						break;
					}
					
					case TouchPhase.Ended:
					case TouchPhase.Canceled:
					{
						DoTouchEnded(WrapFinger(t));
						break;
					}
						
					case TouchPhase.Moved:
					{
						DoTouchChanged(WrapFinger(t));
						break;
					}
						
					case TouchPhase.Stationary:
					{
						DoTouchChanged(WrapFinger(t));
						break;
					}
				}
			}
		}
		
		#region Helper Functions
		
		Finger WrapFinger(Touch t)
		{
			Finger f = new Finger();
			
			f.m_id 	= t.fingerId;
			f.m_phase = t.phase;
			f.m_position = t.position;
			f.m_deltaTime = t.deltaTime;
			f.m_deltaPos = t.deltaPosition;
			
			return f;
		}
		
		bool UpdateFinger (Finger f, Touch t)
		{
			bool changed =
				f.m_phase != t.phase ||
				f.m_position != t.position ||
				f.m_deltaTime != t.deltaTime ||
				f.m_deltaPos != t.deltaPosition;
			
			f.m_phase = t.phase;
			f.m_position = t.position;
			f.m_deltaTime = t.deltaTime;
			f.m_deltaPos = t.deltaPosition;
			
			return changed;
		}
		
		Gesture GetGesture()
		{
			return null;
		}
		
		#endregion
	}
}