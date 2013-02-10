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
		public int FramesPerEvent = 5;
		
		private int m_tte = 0;
		private InputData m_data;
		
		void Start ()
		{
			m_data = new InputData();
			m_data.Active = true;
			m_data.Type = InputMethod.AccelerometerInput;
			
			InputBegin(m_data);
		}
		
		void Update ()
		{
			m_data.Focus = Input.acceleration;
			
			//Debug.Log (m_tte);
			if (m_tte++ < FramesPerEvent-1)
			{
				InputTick(m_data);
			}
			
			int nEvents = Input.accelerationEventCount;
			for (int eventIndex=0; eventIndex<nEvents; eventIndex++)
			{
				AccelerationEvent accelEvent = Input.accelerationEvents[eventIndex];
				
				//InputData temp = new InputData();
				//temp.Active = true;
				//temp.Type = InputMethod.AccelerometerInput;
				//temp.Focus = accelEvent.acceleration && accelEvent.deltaTime;
			}
		}
	}
}