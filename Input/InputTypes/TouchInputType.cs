using UnityEngine;
using System.Collections;

namespace BaseFramework.InputManager
{
	/// <summary>
	/// Touch input type. Terrible Implementation!!
	/// 
	/// TODO : REDO TouchInputType. SRSLY!
	/// </summary>
	public class TouchInputType : InputType
	{
		public int m_id;
		public float m_deltaTime;
		public Vector2 m_position;
		public Vector2 m_deltaPos;
		public TouchPhase m_phase;
		
		void Start ()
		{
		}
		
		void Update() // todo : different forms of input
		{
			int nTouches = Input.touchCount;
			
			for (int i=0; i<nTouches; i++)
			{
				// todo (efficiency) : Only Update when there has been a change?
				InputData data = WrapTouch ( Input.touches[i] );
				
				switch (Input.touches[i].phase)
				{
					case TouchPhase.Began:
					{
					
						InputStarted (ref data);
						break;
					}
					
					case TouchPhase.Ended:
					case TouchPhase.Canceled:
					{
						InputStopped (ref data);
						break;
					}
						
					case TouchPhase.Stationary:	
					case TouchPhase.Moved:
					{
						InputTick (ref data);
						break;
					}
				}
			}
		}
		
		InputData WrapTouch (Touch t)
		{
			InputData f = new InputData();
			f.Type = InputMethod.TouchInput;
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
	}
}
