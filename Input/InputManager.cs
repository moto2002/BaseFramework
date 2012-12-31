using UnityEngine;
using System.Collections.Generic;

namespace BaseFramework
{
	public class InputData
	{
		public bool Active;
		public Vector3 Focus;
	}
	
	/// <summary>
	/// Input manager. Provides a framework for handling any complex input for a game.
	/// </summary>
	public class InputManager : MonoSingleton<InputManager>
	{
		// InputEvent delegate
		public delegate void InputEvent (InputData f);
		
		// InputEvents thrown
		public event InputEvent InputStart;
		public event InputEvent InputUpdate;
		public event InputEvent InputEnd;
		
		
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
		
		void Awake () // todo (functionality) : Add appropriate InputTypes automatically.
		{
			GameObject go = new GameObject ();
			go.transform.parent = transform;
			go.AddComponent <MouseInputType> ();
			go.name = "MouseInputType";
		}
	}
}