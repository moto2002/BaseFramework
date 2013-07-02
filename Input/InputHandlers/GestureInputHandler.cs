using UnityEngine;
using System.Collections.Generic;


namespace BaseFramework.InputManager
{
	public class GestureInputHandler : InputHandler
	{
		protected override void InitialiseInputHandler ()
		{
			registeredGestures = new List<Gesture>();
			registeredGestures.Add( new SwipeGesture() );
			
			m_touches = new List<InputData>();
		}
		
		protected override InputMethod ValidInputMethods()
		{
			return InputMethod.TouchInput;
		}
		
		public override void OnInputStart( InputData data )
		{
			m_touches.Add( data );
		}
		
		public override void OnInputTick( InputData data )
		{
			for ( int gIndex = 0; gIndex < registeredGestures.Count; gIndex++ )
			{
				Gesture g = registeredGestures[ gIndex ];
				if ( g.NumberOfTouchesRequiredForGesture() == m_touches.Count )
				{
					g.UpdateGesture( m_touches.ToArray() ); //TODO: Something with this result
				}
			}
		}
		
		public override void OnInputEnd( InputData data )
		{
			for ( int gIndex = 0; gIndex < registeredGestures.Count; gIndex++ )
			{
				Gesture g = registeredGestures[ gIndex ];
				if ( g.NumberOfTouchesRequiredForGesture() == m_touches.Count )
				{
					g.UpdateGesture( m_touches.ToArray() ); //TODO: Something with this result
				}
			}
			m_touches.Remove( data );
		}
		
		private List<Gesture> registeredGestures;
		private List<InputData> m_touches;
	}
}