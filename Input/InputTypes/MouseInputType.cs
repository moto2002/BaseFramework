using UnityEngine;
using System.Collections;

namespace BaseFramework
{
	public class MouseInputType : InputType
	{
		public int ButtonsToTrack = 2; // todo (improvement) : better mouse button tracking
		public float Sensitivity = 5.0f;
		private InputData[] m_data;
		
		
		void Start ()
		{
			m_data = new InputData[ButtonsToTrack];
			
			for (int i=0; i<m_data.Length; i++)
			{
				m_data[i] = new InputData ();
			}
		}
		
		void Update () // todo (efficiency) : this is disgusting. clean it up.
		{
			for (int i=0; i<ButtonsToTrack; i++)
			{
				if (Input.GetMouseButton (i))
				{
					if (m_data[i].Active)
					{
						// have we changed position?
						if ((m_data[i].Focus - Input.mousePosition).sqrMagnitude > Sensitivity)
						{
							m_data[i].Focus = Input.mousePosition;
							SendMessageUpwards ("InputChanged", m_data[i]);
						}
					}
					else
					{
						// we started a click!
						m_data[i].Active = true;
						m_data[i].Focus = Input.mousePosition;
						SendMessageUpwards ("InputStarted", m_data[i]);
					}
				}
				else
				{
					if (m_data[i].Active)
					{
						// we stopped clicking
						m_data[i].Active = false;
						m_data[i].Focus = Input.mousePosition;
						SendMessageUpwards ("InputStopped", m_data[i]);
					}
				}
			}
		}
	}
}