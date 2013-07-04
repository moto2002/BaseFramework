using System;
using UnityEngine;


namespace BaseFramework.InputManager
{
	public abstract class InputHandler
	{
		private InputManager m_input;
		private InputMethod m_validInputMethods;
		
		#region Abstract Methods
		
		protected abstract InputMethod ValidInputMethods();
		
		public abstract void OnInputStart( InputData f );
		public abstract void OnInputTick( InputData f );
		public abstract void OnInputEnd( InputData f );
		
		#endregion
		
		public InputHandler( )
		{
			m_validInputMethods = ValidInputMethods( );
			
			m_input = InputManager.Instance;
			
			m_input.OnInputStart += InputBeganWrapper;
			m_input.OnInputTick += InputChangedWrapper;
			m_input.OnInputEnd += InputStoppedWrapper;
		}
		
		private void OnDestroy( )
		{
			m_input.OnInputStart -= InputBeganWrapper;
			m_input.OnInputTick -= InputChangedWrapper;
			m_input.OnInputEnd -= InputStoppedWrapper;
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