using UnityEngine;
using System.Collections.Generic;

namespace BaseFramework.InputManager
{
	/// <summary>
	/// Input manager. Provides a framework for handling complex input.
	/// Complex input could include touch gestures, accelerometer events, and key combinations.
	/// The input manager also serves as a layer between input and action, making for easier preprocessing.
	/// </summary>
	public class InputManager : MonoSingleton<InputManager>
	{
		// InputEvent delegate
		public delegate void InputEvent (InputData f);
		
		// InputEvents thrown
		public event InputEvent OnInputStart;
		public event InputEvent OnInputTick;
		public event InputEvent OnInputEnd;
		
		private void Start ()
		{
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
			AddInput<MouseInputType>();
			//AddInput<KeyboardInputType>();
			//AddInput<ControllerInputType>();
#endif

#if UNITY_IPHONE || UNITY_ANDROID
			AddInput<TouchInputType>();
			AddInput<AccelerometerInputType>();
#endif
		}
		
		private void AddInput <T> () where T : InputType
		{
			GameObject go = new GameObject ();
			go.transform.parent = transform;
			go.AddComponent <T> ();
			go.name = typeof(T).Name;
		}
		
		private void InputStart (InputData data)
		{
			if (OnInputStart != null)
				OnInputStart (data);
		}
		
		private void InputTick (InputData data)
		{
			if (OnInputTick != null)
				OnInputTick (data);
		}
		
		private void InputEnd (InputData data)
		{
			if (OnInputEnd != null)
				OnInputEnd (data);
		}
	}
}