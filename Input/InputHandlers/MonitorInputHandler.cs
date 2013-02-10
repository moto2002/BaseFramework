using UnityEngine;
using System.Collections.Generic;

namespace BaseFramework.InputManager
{
	/// <summary>
	/// Monitor input handler. Displays a GUI overlay on screen showing input debug shizzle.
	/// </summary>
	public class MonitorInputHandler : InputHandler
	{
		#region Overriden Methods
		
		protected override InputMethod ValidInputMethods ()
		{
			return InputMethod.AccelerometerInput | InputMethod.MouseInput;
		}
		
		public override void OnInputStart (InputData f)
		{
			LogInput (f, InputEventType.InputStart);
		}
		
		public override void OnInputTick (InputData f)
		{
			LogInput (f, InputEventType.InputTick);
		}
		
		public override void OnInputEnd (InputData f)
		{
			LogInput (f, InputEventType.InputEnd);
		}
		
		#endregion
		
		/// <summary>
		/// Where the GUI is drawn.
		/// </summary>
		private void OnGUI()
		{
			
		}
		
		/// <summary>
		/// Prints InputData to the console.
		/// </summary>
		/// <param name='data'>The supplied InputData</param>
		private void LogInput (InputData data, InputEventType type)
		{
			string[] eventTypeStrings = 
			{
				"START",
				"TICK",
				"END"
			};
			
			string eventType = eventTypeStrings[(int)type];
			Debug.Log (eventType+"_EVENT ["+data.Type.ToString()+"]");
			Debug.Log ("-- Active: "+data.Active);
			Debug.Log ("-- Focus:  "+data.Focus);
		}
		
		/// <summary>
		/// Draws an InputData packet to the Screen.
		/// </summary>
		/// <param name='data'>The supplied InputData.</param>
		private void DrawInput (InputData data)
		{
			string dataName = "default";
			GUILayoutOption[][] layoutOptions = GetLayoutOptions (data);
			
			switch (data.Type)
			{
			case InputMethod.AccelerometerInput: name = "Accelerometer"; break;
			case InputMethod.KeyboardInput: name = "Keyboard"; break;
			case InputMethod.MouseInput: name = "Mouse"; break;
			case InputMethod.TouchInput: name = "Touch"; break;
			}
			
			// Datatype's Name
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label (dataName);
				GUILayout.Label ("");
			}
			GUILayout.EndHorizontal();
			
			// Active?
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label ("");
				GUILayout.Label ("");
			}
			GUILayout.EndHorizontal();
			
			// Datatype's value
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label ("");
				GUILayout.Label ("");
			}
			GUILayout.EndHorizontal();
		}
		
		/// <summary>
		/// Gets layout options for an InputData packet.
		/// </summary>
		/// <returns>
		/// The layout options.
		/// </returns>
		/// <param name='data'>Supplied Data.</param>
		private GUILayoutOption[][] GetLayoutOptions (InputData data)
		{
			GUILayoutOption[][] dataLayout =
			{
				new GUILayoutOption[] {  }
			};
			
			return dataLayout;
		}
	}
}