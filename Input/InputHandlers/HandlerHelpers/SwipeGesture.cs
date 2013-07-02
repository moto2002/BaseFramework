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
			throw new NotImplementedException ();
		}
		
		public override Vector2 UpdateGesture( InputData[] data )
		{
			throw new System.NotImplementedException ();
		}
	}
}