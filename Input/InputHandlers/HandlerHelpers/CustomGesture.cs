using System;
using UnityEngine;

namespace BaseFramework.InputManager
{
	[Serializable]
	public class CustomGestureVO
	{
		public int FramesTracked;
		public int FingersTracked;
		public Vector2[][] MotionTracked;
	}
	
	/// <summary>
	/// A custom gesture is a pattern-matching Gesture.
	/// Touch Input is matched against pre-recorded motion paths.
	/// </summary>
	public class CustomGesture : Gesture
	{
		public override int NumberOfTouchesRequiredForGesture ()
		{
			throw new NotImplementedException ();
		}
		
		public override Vector2 UpdateGesture( InputData[] data )
		{
			throw new NotImplementedException ();
		}
	}
}