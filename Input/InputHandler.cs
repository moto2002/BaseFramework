using UnityEngine;
using System.Collections;

namespace BaseFramework
{
	public abstract class InputHandler : MonoBehaviour
	{
		private InputManager m_input;
		
		protected virtual void Awake ()
		{
			m_input = InputManager.Instance;
			
			m_input.InputStart += InputBegan;
			m_input.InputUpdate += InputChanged;
			m_input.InputEnd += InputStopped;
		}
		
		protected virtual void OnDestroy ()
		{
			m_input.InputStart -= InputBegan;
			m_input.InputUpdate -= InputChanged;
			m_input.InputEnd -= InputStopped;
		}
		
		public abstract void InputBegan (InputData f);
		public abstract void InputChanged (InputData f);
		public abstract void InputStopped (InputData f);
	}
}