using UnityEngine;
using System.Collections;

namespace BaseFramework.InputManager
{
	public class DummyInputType : InputType
	{
		private InputData m_data;
		
		void Start ()
		{
			m_data = new InputData();
			m_data.Active = true;
			m_data.Focus = Vector3.zero;
			m_data.Type = InputMethod.None;
		}
		
		void Update ()
		{
			OnInput (m_data, InputEventType.InputTick);
		}
	}
}