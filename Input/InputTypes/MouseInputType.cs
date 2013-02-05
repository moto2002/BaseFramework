using UnityEngine;
using System.Collections;

namespace BaseFramework
{
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
			}
		}
		
		void Update () // todo (efficiency) : this is disgusting. clean it up.
		{
			for (int i=0; i<m_data.Length; i++)
			{
				InputData before = m_data[i];
				
				m_data[i].Active = Input.GetMouseButton (i);
				m_data[i].Focus = Input.mousePosition;
				
				if (!before.Active && m_data[i].Active)
				{
					SendMessageUpwards ("InputStarted", m_data[i]);
				}
				if (!m_data[i].Equals (before))
				{
					SendMessageUpwards ("InputChanged", m_data[i]);
				}
				else if (m_data[i].Equals (before))
				{
					SendMessageUpwards ("InputStopped", m_data[i]);
				}
			}
		}
	}
}