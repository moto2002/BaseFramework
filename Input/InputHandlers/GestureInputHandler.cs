using UnityEngine;
using System.Collections;


namespace BaseFramework.InputManager
{
	//TODO: There should only ever be one GestureInputHandler, and it should notify objects of Gesture updates.
	public class GestureInputHandler : InputHandler
	{
		public Gesture[] registeredGestures;
		
		protected override InputMethod ValidInputMethods()
		{
			return InputMethod.TouchInput;
		}
		
		public override void OnInputStart( InputData data )
		{
			for ( int gIndex = 0; gIndex < registeredGestures.Length; gIndex++ )
			{
				registeredGestures[ gIndex ].RegisterTouch( data );
			}
		}
		
		public override void OnInputTick( InputData data )
		{
			for ( int gIndex = 0; gIndex < registeredGestures.Length; gIndex++ )
			{
				registeredGestures[ gIndex ].UpdateGesture( ); //TODO: Something with this result
			}
		}
		
		public override void OnInputEnd( InputData data )
		{
			for ( int gIndex = 0; gIndex < registeredGestures.Length; gIndex++ )
			{
				registeredGestures[ gIndex ].DeregisterTouch( data );
			}
		}
	}
}