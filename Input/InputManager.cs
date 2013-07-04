using UnityEngine;
using BaseFramework.Core;
using System.Collections.Generic;


namespace BaseFramework.InputManager
{
	/// <summary>
	/// Input manager. Provides a framework for handling complex input.
	/// Complex input could include touch gestures, accelerometer events, and key combinations.
	/// The input manager also serves as a layer between input and action, making for easier preprocessing.
	/// </summary>
	public class InputManager : MonoSingleton<InputManager>, IFacade
	{
		// InputEvent delegate
		public delegate void InputEvent (InputData f);
		
		// InputEvents thrown
		public event InputEvent OnInputStart;
		public event InputEvent OnInputTick;
		public event InputEvent OnInputEnd;
		
		private void Awake ()
		{
			m_facadeRegistry = new Dictionary<System.Type, object>();
			
			GestureInputHandler gestureIHandler = new GestureInputHandler();
			RegisterObject<GestureInputHandler>( gestureIHandler );
			
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
		
		#region IFacade Implementation
		public T RetrieveObject<T> ()
		{
			object result;
			m_facadeRegistry.TryGetValue( typeof( T ), out result );
			return (T)result;
		}
		
		public void RegisterObject<T> ( T instance )
		{
			m_facadeRegistry.Add( typeof( T ), instance );
		}
		
		private Dictionary<System.Type, object> m_facadeRegistry;
		#endregion
		
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