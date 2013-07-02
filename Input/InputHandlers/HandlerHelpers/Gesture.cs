using UnityEngine;

namespace BaseFramework.InputManager
{
	public abstract class Gesture
	{
		/// <summary>
		/// Registers a new touch for the Gesture to consider.
		/// </summary>
		/// <param name='t'>
		/// An InputData object, t.
		/// </param>
		public abstract void RegisterTouch( InputData t );
		
		/// <summary>
		/// Deregisters a registered touch from the Gesture calculations.
		/// Some gestures are only complete in their final frame, therefore DeregisterTouch
		/// may return a velocity vector.
		/// </summary>
		/// <param name='t'>
		/// An InputData object, t.
		/// </param>
		/// <returns>
		/// A Vector2 with magnitude equal to Gesture velocity,
		/// and angle to represent the direction of the Gesture.
		/// Returns a Vector2 of zero length if Gesture is not recognised.
		/// </returns>
		public abstract Vector2 DeregisterTouch( InputData t );
		
		/// <summary>
		/// Calculates a Vector2 to represent velocity and direction of the Gesture.
		/// </summary>
		/// <returns>
		/// A Vector2 with magnitude equal to Gesture velocity,
		/// and angle to represent the direction of the Gesture.
		/// Returns a Vector2 of zero length if Gesture is not recognised.
		/// </returns>
		public abstract Vector2 UpdateGesture( );
	}
}