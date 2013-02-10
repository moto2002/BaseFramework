using UnityEngine;
using System.Collections;

namespace BaseFramework.InputManager
{
	public abstract class InputHandler : MonoBehaviour
	{
		private InputManager m_input;
		private InputMethod m_validInputMethods;
		
		#region Abstract Methods
		
		protected abstract InputMethod ValidInputMethods();
		
		public abstract void InputBegan (InputData f);
		public abstract void InputChanged (InputData f);
		public abstract void InputStopped (InputData f);
		
		#endregion
		
		protected virtual void Start  ()
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
		
		public void InputBeganWrapper (InputData f)
		{
			if ((m_validInputMethods & f.Type) != InputMethod.None)
			{
				InputBegan (f);
			}
		}
		
		public void InputChangedWrapper (InputData f)
		{
			if ((m_validInputMethods & f.Type) != InputMethod.None)
			{
				InputChanged (f);
			}
		}
		
		public void InputStoppedWrapper (InputData f)
		{
			if ((m_validInputMethods & f.Type) != InputMethod.None)
			{
				InputStopped (f);
			}
		}
	}
}