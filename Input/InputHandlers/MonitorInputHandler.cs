using UnityEngine;
using System.Collections;

namespace BaseFramework.InputManager
{
	/// <summary>
	/// Monitor input handler. Displays a GUI overlay on screen showing input debug shizzle.
	/// </summary>
	public class MonitorInputHandler : InputHandler
	{
		protected override InputMethod ValidInputMethods ()
		{
			return InputMethod.AccelerometerInput | InputMethod.MouseInput;
		}
		
		public override void InputBegan (InputData f)
		{
			LogInput (f);
		}
		
		public override void InputChanged (InputData f)
		{
			LogInput (f);
		}
		
		public override void InputStopped (InputData f)
		{
			LogInput (f);
		}
		
		private void Update()
		{
			
		}
		
		private void LogInput (InputData data)
		{
			Debug.Log ("Received Input ["+data.Type.ToString()+"]");
			Debug.Log ("-- Active: "+data.Active);
			Debug.Log ("-- Focus:  "+data.Focus);
		}
	}
}