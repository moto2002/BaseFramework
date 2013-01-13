using UnityEngine;
using System.Collections.Generic;

namespace BaseFramework
{
	public struct InputData
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
		public delegate void InputEvent (ref InputData f);
		
		// InputEvents thrown
		public event InputEvent InputStart;
		public event InputEvent InputUpdate;
		public event InputEvent InputEnd;
		
		
		private void InputStarted (ref InputData data)
		{
			if (InputStart != null)
				InputStart (ref data);
		}
		
		private void InputChanged (ref InputData data)
		{
			if (InputUpdate != null)
				InputUpdate (ref data);
		}
		
		private void InputStopped (ref InputData data)
		{
			if (InputEnd != null)
				InputEnd (ref data);
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