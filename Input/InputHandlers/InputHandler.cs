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
		
		public abstract void OnInputStart (InputData f);
		public abstract void OnInputTick (InputData f);
		public abstract void OnInputEnd (InputData f);
		
		#endregion
		
		protected virtual void Start  ()
		{
			m_validInputMethods = ValidInputMethods();
			
			m_input = InputManager.Instance;
			
			m_input.OnInputStart += InputBeganWrapper;
			m_input.OnInputTick += InputChangedWrapper;
			m_input.OnInputEnd += InputStoppedWrapper;
		}
		
		protected virtual void OnDestroy ()
		{
			m_input.OnInputStart -= InputBeganWrapper;
			m_input.OnInputTick -= InputChangedWrapper;
			m_input.OnInputEnd -= InputStoppedWrapper;
		}
		
		public void InputBeganWrapper (InputData f)
		{
			if ((m_validInputMethods & f.Type) != InputMethod.None)
			{
				OnInputStart (f);
			}
		}
		
		public void InputChangedWrapper (InputData f)
		{
			if ((m_validInputMethods & f.Type) != InputMethod.None)
			{
				OnInputTick (f);
			}
		}
		
		public void InputStoppedWrapper (InputData f)
		{
			if ((m_validInputMethods & f.Type) != InputMethod.None)
			{
				OnInputEnd (f);
			}
		}
	}
}