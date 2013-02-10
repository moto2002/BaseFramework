using UnityEngine;
using System.Collections;

namespace BaseFramework.InputManager
{
	/// <summary>
	/// Rotation input handler. Will rotate the transform it is attached to using accelerometer input.
	/// 
	/// </summary>
	public class RotationInputHandler : InputHandler
	{
		private Quaternion m_initialRotation;
		
		protected override void Start()
		{
			base.Start();
			m_initialRotation = Quaternion.identity; // todo : make editable
		}
		
		protected override InputMethod ValidInputMethods ()
		{
			return InputMethod.AccelerometerInput | InputMethod.KeyboardInput;
		}
		
		public override void InputBegan (InputData f)
		{
		}
		
		public override void InputChanged (InputData f)
		{
			Vector3 up = GetOrientation();
		}
		
		public override  void InputStopped (InputData f)
		{
		}
		
		
		Vector3 GetOrientation ()
		{
			switch (Input.deviceOrientation)
			{
			default:
			case DeviceOrientation.Portrait:
			case DeviceOrientation.Unknown:
				return Vector3.up;
				
			case DeviceOrientation.PortraitUpsideDown:
				return Vector3.down;
				
			case DeviceOrientation.FaceUp:
				return Vector3.forward;
				
			case DeviceOrientation.FaceDown:
				return Vector3.back;
				
			case DeviceOrientation.LandscapeLeft:
				return Vector3.back;
			
			case DeviceOrientation.LandscapeRight:
				return Vector3.back;
			}
		}
	}
}