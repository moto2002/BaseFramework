using UnityEngine;
using System.Collections;

namespace BaseFramework.InputManager
{
	public abstract class InputHandler : MonoBehaviour
	{
		private InputManager m_input;
		private InputType m_validInputMethods;
		
		
		protected abstract int ValidInputMethods();
		
		protected virtual void Awake ()
		{
			m_validInputMethods = ValidInputMethods();
			
			m_input = InputManager.Instance;
			
			m_input.InputStart += InputBeganWrapper;
			m_input.InputUpdate += InputChangedWrapper;
			m_input.InputEnd += InputStoppedWrapper;
		}
		
		protected virtual void OnDestroy ()
		{
			m_input.InputStart -= InputBeganWrapper;
			m_input.InputUpdate -= InputChangedWrapper;
			m_input.InputEnd -= InputStoppedWrapper;
		}
		
		public void InputBeganWrapper (ref InputData f)
		{
			if (m_validInputMethods & f.Type != InputMethod.None)
			{
				InputBegan (ref f);
			}
		}
		
		public void InputChangedWrapper (ref InputData f)
		{
			if (m_validInputMethods & f.Type != InputMethod.None)
			{
				InputChanged (ref f);
			}
		}
		
		public void InputStoppedWrapper (ref InputData f)
		{
			if (m_validInputMethods & f.Type != InputMethod.None)
			{
				InputStopped (ref f);
			}
		}
		
		public abstract void InputBegan (ref InputData f);
		public abstract void InputChanged (ref InputData f);
		public abstract void InputStopped (ref InputData f);
	}
}