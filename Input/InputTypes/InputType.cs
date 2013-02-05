using UnityEngine;
using System;
using System.Collections;

namespace BaseFramework.InputManager
{
	[Flags]
	public enum InputMethod
	{
		None				= 0,
		MouseInput			= 1,
		TouchInput			= 2,
		AccelerometerInput	= 4,
		KeyboardInput		= 8
	};
	
	public struct InputData
	{
		public bool Active;
		public Vector3 Focus;
		
		public InputMethod Type;
	}
	
	public abstract class InputType : MonoBehaviour
	{
		// Should attempt to add itself to the input manager as a child transform if on a supported platform
		
		protected void InputStarted (ref InputData data)
		{
			SendMessageUpwards ("InputStarted", data);
		}
		
		protected void InputTick (ref InputData data)
		{
			SendMessageUpwards ("InputStarted", data);
		}
		
		protected void InputStopped (ref InputData data)
		{
			SendMessageUpwards ("InputStarted", data);
		}
	}
}