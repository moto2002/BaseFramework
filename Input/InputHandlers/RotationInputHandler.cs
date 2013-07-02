//using UnityEngine;
//using System.Collections;
//
//namespace BaseFramework.InputManager
//{
//	/// <summary>
//	/// Rotation input handler. Will rotate the transform it is attached to using accelerometer input.
//	/// 
//	/// TODO: Rename to AccelerometerInputHandler
//	/// </summary>
//	public class RotationInputHandler : InputHandler
//	{
////		private Quaternion m_initialRotation;
//		
//		protected override void InitialiseInputHandler ()
//		{
////			m_initialRotation = Quaternion.identity; // todo : make editable
//		}
//		
//		protected override InputMethod ValidInputMethods ()
//		{
//			return InputMethod.AccelerometerInput | InputMethod.KeyboardInput;
//		}
//		
//		public override void OnInputStart (InputData f)
//		{
//		}
//		
//		public override void OnInputTick (InputData f)
//		{
////			Vector3 up = GetOrientation();
////			Vector3 axis = Vector3.forward;
//			
//			
//			transform.rotation = Quaternion.LookRotation (-f.Focus);
//		}
//		
//		public override  void OnInputEnd (InputData f)
//		{
//		}
//		
//		
//		Vector3 GetOrientation ()
//		{
//			switch (Input.deviceOrientation)
//			{
//			default:
//			case DeviceOrientation.Portrait:
//			case DeviceOrientation.Unknown:
//				return Vector3.down;
//				
//			case DeviceOrientation.PortraitUpsideDown:
//				return Vector3.up;
//				
//			case DeviceOrientation.FaceUp:
//				return Vector3.forward;
//				
//			case DeviceOrientation.FaceDown:
//				return Vector3.back;
//				
//			case DeviceOrientation.LandscapeLeft:
//				return Vector3.left;
//			
//			case DeviceOrientation.LandscapeRight:
//				return Vector3.right;
//			}
//		}
//	}
//}