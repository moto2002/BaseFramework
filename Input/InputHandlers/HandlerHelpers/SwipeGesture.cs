using System;
using UnityEngine;


namespace BaseFramework.InputManager
{
	/// <summary>
	/// A SwipeGesture looks for multiple(?) touches in the same
	/// direction, and returns their direction and speed.
	/// 
	/// TODO: What would be the best way to implement multi-touch swipe gestures? In one class or several?
	/// </summary>
	public class SwipeGesture : Gesture
	{
		public override int NumberOfTouchesRequiredForGesture ()
		{
			return 1;
		}
		
		public override Vector2 UpdateGesture( InputData[] data )
		{
			// Get the one and only touch.
			if ( data.Length != 1 ) return Vector2.zero;
			InputData touch = data[0];
			
			// Initialise the gesture if the touch has just started
			string beganPhase = TouchInputType.TouchPhases[ (int)TouchPhase.Began ];
			if ( string.Compare( touch.Details, beganPhase ) == 0 )
			{
				m_touchData = touch;
				m_gestureTime = Time.time;
				m_result = touch.Focus;
			}
			
			if ( m_touchData != null )
			{
//				Vector3 dir = touch.Focus - m_touchData.Focus;
				
				//TODO: Check if swipe gesture is a relatively straight line
				//TODO: Check if swipe gesture is performed within a certain time limit.
			}
			
			// Calculate & return resulting velocity vector
			string endPhase = TouchInputType.TouchPhases[ (int)TouchPhase.Ended ];
			if ( string.Compare( touch.Details, endPhase ) == 0 )
			{
				m_gestureTime = Time.time - m_gestureTime;
				m_result.x = touch.Focus.x - m_result.x;
				m_result.y = touch.Focus.y - m_result.y;
				
				Vector2 velocity = m_result / m_gestureTime;
				return velocity;
			}
			return Vector2.zero;
		}
		
//		private static float m_maxTime = 2.0f;
//		private static float m_maxAngularChangeDeg = 5.0f;
		
		private float m_gestureTime;
		private Vector2 m_result;
		private InputData m_touchData;
	}
}