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
		public override void RegisterTouch (InputData t)
		{
			throw new System.NotImplementedException ();
		}
		
		public override Vector2 DeregisterTouch (InputData t)
		{
			throw new System.NotImplementedException ();
		}
		
		public override Vector2 UpdateGesture ()
		{
			throw new System.NotImplementedException ();
		}
	}
}