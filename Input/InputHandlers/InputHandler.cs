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
		protected virtual void InitialiseInputHandler() { }
		protected virtual void DestroyInputHandler() { }
		
		public abstract void OnInputStart( InputData f );
		public abstract void OnInputTick( InputData f );
		public abstract void OnInputEnd( InputData f );
		
		#endregion
		
		private void Start( )
		{
			m_validInputMethods = ValidInputMethods( );
			
			m_input = InputManager.Instance;
			
			m_input.OnInputStart += InputBeganWrapper;
			m_input.OnInputTick += InputChangedWrapper;
			m_input.OnInputEnd += InputStoppedWrapper;
			
			InitialiseInputHandler( );
		}
		
		private void OnDestroy( )
		{
			m_input.OnInputStart -= InputBeganWrapper;
			m_input.OnInputTick -= InputChangedWrapper;
			m_input.OnInputEnd -= InputStoppedWrapper;
			
			DestroyInputHandler( );
		}
		
		private bool AcceptsInputMethod( InputData data )
		{
			return ( m_validInputMethods & data.Type ) != InputMethod.None;
		}
		
		public void InputBeganWrapper( InputData data )
		{
			if ( AcceptsInputMethod( data ) )
			{
				OnInputStart( data );
			}
		}
		
		public void InputChangedWrapper( InputData data )
		{
			if ( AcceptsInputMethod( data ) )
			{
				OnInputTick( data );
			}
		}
		
		public void InputStoppedWrapper( InputData data )
		{
			if ( AcceptsInputMethod( data ) )
			{
				OnInputEnd( data );
			}
		}
	}
}