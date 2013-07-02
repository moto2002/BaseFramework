using UnityEngine;
using System.Collections;

namespace BaseFramework.InputManager
{
	/// <summary>
	/// Touch input type.
	/// 
	/// TODO: Handle maxTouches here? Surely thats not an InputType thing, but an InputHandler thing?
	/// TODO: fingerID represented in the InputData somehow? May be useful for an InputHandler to know which finger is which.
	/// </summary>
	public class TouchInputType : InputType
	{
		private int maxTouchesToDetect = 2;
		private InputData[] touchInput;
		
		#region Properties
		
		public int TouchesToDetect
		{
			get { return maxTouchesToDetect; }
			set
			{
				maxTouchesToDetect = Mathf.Clamp( value, 1, 5 );
				touchInput = new InputData[ maxTouchesToDetect ];
				for ( int i = 0; i < touchInput.Length; i++ )
				{
					touchInput[ i ] = new InputData();
				}
			}
		}
		
		#endregion
		
		#region Monobehaviour Overrides
		
		void Update()
		{
			for ( int i = 0; i < Input.touchCount; i++ )
			{
				Touch t = Input.touches[ i ];
				
				if ( t.fingerId >= maxTouchesToDetect )
				{
					// Not detecting this finger.
				}
				else
				{
					if ( ProcessTouchToData( touchInput[ t.fingerId ], t ) )
					{
						switch ( t.phase )
						{
							case TouchPhase.Began:
							{
								OnInput ( touchInput[ t.fingerId ], InputEventType.InputStart );
								break;
							}
							
							case TouchPhase.Ended:
							case TouchPhase.Canceled:
							{
								OnInput ( touchInput[ t.fingerId ], InputEventType.InputEnd );
								ResetTouch( touchInput[ t.fingerId ] );
								break;
							}
								
							case TouchPhase.Stationary:	
							case TouchPhase.Moved:
							{
								OnInput ( touchInput[ t.fingerId ], InputEventType.InputTick );
								break;
							}
						}
					}
				}
			}
		}
		
		#endregion
		
		#region Helper Methods
		
		private void ResetTouch( InputData data )
		{
			data.Active = false;
			data.Focus  = Vector3.zero;
			data.Type   = InputMethod.None;
		}
		
		private bool ProcessTouchToData( InputData data, Touch touch )
		{
			data.Active = touch.phase != TouchPhase.Canceled && touch.phase != TouchPhase.Ended;
			data.Focus  = touch.position;
			data.Type   = InputMethod.TouchInput;
			
			return true;
		}
		
		#endregion
	}
}
