using UnityEngine;

namespace BaseFramework.InputManager
{
	public abstract class Gesture
	{
		/// <summary>
		/// The Number of touches required for the gesture to make calculations.
		/// Used as a form of validation for the GestureInputHandler to decide when to
		/// Update a Gesture.
		/// </summary>
		/// <returns>
		/// The number of touches required for gesture.
		/// </returns>
		public abstract int NumberOfTouchesRequiredForGesture( );
		
		/// <summary>
		/// Calculates a Vector2 to represent velocity and direction of the Gesture.
		/// </summary>
		/// <returns>
		/// A Vector2 with magnitude equal to Gesture velocity,
		/// and angle to represent the direction of the Gesture.
		/// Returns a Vector2 of zero length if Gesture is not recognised.
		/// </returns>
		public abstract Vector2 UpdateGesture( InputData[] currTouches );
	}
}