using UnityEngine;
using System.Collections;

namespace BaseFramework.InputManager
{
	/// <summary>
	/// Stores InputData for 3 Buttons. Could be improved to support more.
	/// Checks every button, every tick.
	/// Uses .Equals.. bit primitive, no?
	/// 
	/// TODO : Support for more buttons in MouseInputType
	/// TODO : Better (Customizable?) Input for MouseInputType
	/// </summary>
	public class MouseInputType : InputType
	{
		public float Sensitivity = 5.0f;
		private InputData[] m_data;
		
		void Start ()
		{
			m_data = new InputData[3];
			
			for (int i=0; i<m_data.Length; i++)
			{
				m_data[i] = new InputData ();
				m_data[i].Type = InputMethod.MouseInput;
			}
		}
		
		void Update () // bugged?!!?!
		{
			//*
			for (int i=0; i<m_data.Length; i++)
			{
				InputData before = m_data[i];
				
				m_data[i].Active = Input.GetMouseButton (i);
				m_data[i].Focus = Input.mousePosition;
				
				if (!before.Active && m_data[i].Active)
				{
					InputBegin (m_data[i]);
				}
				if (!m_data[i].Equals (before))
				{
					InputTick (m_data[i]);
				}
				else if (m_data[i].Equals (before))
				{
					InputEnd (m_data[i]);
				}
			}
			//*/
		}
	}
}