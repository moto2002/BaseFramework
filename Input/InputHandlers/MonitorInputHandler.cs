using UnityEngine;
using System.Collections;

namespace BaseFramework.InputManager
{
	/// <summary>
	/// Monitor input handler. Displays a GUI overlay on screen showing input debug shizzle.
	/// </summary>
	public class MonitorInputHandler : InputHandler
	{
		protected override int ValidInputMethods ()
		{
			return InputMethod.AccelerometerInput;
		}
		
		public override void InputBegan (ref InputData f)
		{
			throw new System.NotImplementedException ();
		}
		
		public override void InputChanged (ref InputData f)
		{
			throw new System.NotImplementedException ();
		}
		
		public override void InputStopped (ref InputData f)
		{
			throw new System.NotImplementedException ();
		}
	}
}