using UnityEngine;
using System.Collections;

namespace BaseFramework.InputManager
{
	/// <summary>
	/// Accelerometer input type. Basic Implementation.
	/// 
	/// Holds one always-on InputData and updates it each frame.
	/// Acceleration Events should also be handled here, but will require changes to the Inputdata type...
	/// 
	/// TODO : Change InputType to hold more generic data (rather than Vector3)
	/// TODO : Handle AccelerationEvents in the AccelerometerInputType
	/// </summary>
	public class AccelerometerInputType : InputType
	{
		private void Start ()
		{
			m_data = new InputData();
			m_data.Active = true;
			m_data.Type = InputMethod.AccelerometerInput;
			
			OnInput (m_data, InputEventType.InputStart);
		}
		
		private void Update ()
		{
			m_data.Focus = Input.acceleration;
			
			OnInput (m_data, InputEventType.InputTick);
			
			// todo : smoothing?
			int nEvents = Input.accelerationEventCount;
			for (int eventIndex=0; eventIndex<nEvents; eventIndex++)
			{
				//AccelerationEvent accelEvent = Input.accelerationEvents[eventIndex];
				
				//InputData temp = new InputData();
				//temp.Active = true;
				//temp.Type = InputMethod.AccelerometerInput;
				//temp.Focus = accelEvent.acceleration && accelEvent.deltaTime;
			}
		}
		
		private InputData m_data;
	}
}