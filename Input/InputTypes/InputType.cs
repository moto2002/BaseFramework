using UnityEngine;
using System;
using System.Collections;

namespace BaseFramework
{
	public enum InputMethod
	{
		MouseInput,
		TouchInput,
		AccelerometerInput,
		KeyboardInput
	};
	
	public struct InputData
	{
		public bool Active;
		public Vector3 Focus;
	}
	
	public class InputType : MonoBehaviour
	{
		// Should attempt to add itself to the input manager as a child transform if on a supported platform
	}
}