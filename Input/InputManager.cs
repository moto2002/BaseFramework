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
		public event InputEvent InputStart;
		public event InputEvent InputUpdate;
		public event InputEvent InputEnd;
		
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
		
		private void InputStarted (InputData data)
		{
			if (InputStart != null)
				InputStart (data);
			
		}
		
		private void InputChanged (InputData data)
		{
			if (InputUpdate != null)
				InputUpdate (data);
		}
		
		private void InputStopped (InputData data)
		{
			if (InputEnd != null)
				InputEnd (data);
		}
	}
}