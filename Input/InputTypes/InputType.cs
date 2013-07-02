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
	
	[System.Serializable]
	public class InputData
	{
		public bool Active;
		public Vector3 Focus;
		
		public InputMethod Type;
	}
	
	public enum InputEventType
	{
		InputStart = 0,
		InputTick  = 1,
		InputEnd   = 2
	}
	
	public abstract class InputType : MonoBehaviour
	{
		private static string[] MESSAGE_TYPES = 
		{
			"InputStart",
			"InputTick",
			"InputEnd"
		};
		
		protected void OnInput (InputData data, InputEventType type)
		{
			try
			{
				string message = MESSAGE_TYPES[(int)type];
				if (!string.IsNullOrEmpty( message ))
				{
					SendMessageUpwards( message, data );
				}
			}
			catch( IndexOutOfRangeException e )
			{
				Debug.LogError( "No message defined for InputEventType " + type.ToString() );
				Debug.LogError( e.Message );
				return;
			}
		}
	}
}