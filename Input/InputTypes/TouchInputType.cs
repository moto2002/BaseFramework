using UnityEngine;
using System.Collections;

namespace BaseFramework
{
	public class TouchInputType : InputType
	{
		public int m_id;
		public float m_deltaTime;
		public Vector2 m_position;
		public Vector2 m_deltaPos;
		public TouchPhase m_phase;
		
		void Update() // todo : different forms of input
		{
			int nTouches = Input.touchCount;
			
			for (int i=0; i<nTouches; i++)
			{
				// todo (efficiency) : Only Update when there has been a change?
				Touch t = Input.touches[i];
				
				switch (t.phase)
				{
					case TouchPhase.Began:
					{
					
						SendMessageUpwards ("InputStarted", WrapTouch (t));
						break;
					}
					
					case TouchPhase.Ended:
					case TouchPhase.Canceled:
					{
						SendMessageUpwards ("InputStopped", WrapTouch (t));
						break;
					}
						
					case TouchPhase.Moved:
					{
						SendMessageUpwards ("InputChanged", WrapTouch (t));
						break;
					}
						
					case TouchPhase.Stationary:
					{
						SendMessageUpwards ("InputChanged", WrapTouch (t));
						break;
					}
				}
			}
		}
		
		#region Helper Functions
		
		InputData WrapTouch (Touch t)
		{
			InputData f = new InputData();
			f.Focus = t.position;
			
			/*
			f.m_id 	= t.fingerId;
			f.m_phase = t.phase;
			f.m_position = t.position;
			f.m_deltaTime = t.deltaTime;
			f.m_deltaPos = t.deltaPosition;
			*/
			
			return f;
		}
		
		bool UpdateFinger (InputData f, Touch t)
		{
			/*
			bool changed =
				f.m_phase != t.phase ||
				f.m_position != t.position ||
				f.m_deltaTime != t.deltaTime ||
				f.m_deltaPos != t.deltaPosition;
			
			f.m_phase = t.phase;
			f.m_position = t.position;
			f.m_deltaTime = t.deltaTime;
			f.m_deltaPos = t.deltaPosition;
			*/
			return false;
		}
		
		#endregion
	}
}
