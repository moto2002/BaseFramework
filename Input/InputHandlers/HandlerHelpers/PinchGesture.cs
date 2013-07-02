using System;
using UnityEngine;


namespace BaseFramework.InputManager
{
	public class PinchGesture : Gesture
	{
		public override int NumberOfTouchesRequiredForGesture( )
		{
			throw new NotImplementedException ();
		}
		
		public override Vector2 UpdateGesture( InputData[] data )
		{
			throw new NotImplementedException ();
		}
	}
}