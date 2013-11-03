using UnityEngine;
using BaseFramework.Core;
using System.Collections.Generic;

namespace BaseFramework.MouseInput
{
	public class MouseInputManager : MonoSingleton<MouseInputManager>
	{
		public static string LAYER_MASK_NAME = "Input";
		
		public void AddMouseAction( MouseAction xAction )
		{
			m_mouseActions.Add( xAction );
		}
		
		public void RemoveMouseAction( MouseAction xAction )
		{
			m_mouseActions.Remove( xAction );
		}
		
		private void Awake()
		{
			GameObject xCameraGameObject = GameObject.FindGameObjectWithTag( "MainCamera" );
			m_camera = xCameraGameObject.GetComponent<Camera>();
			m_mouseActions = new List<MouseAction>();
		}
		
		private void Update()
		{
			Collider xColliderHit = ColliderHit();
			Vector3 xCursorPosition = Input.mousePosition;
			
			//BUG: Gesture can't end if you go outside of bounds
			if ( xColliderHit != null )
			{
				foreach ( MouseAction xAction in m_mouseActions )
				{
					Collider xThisCollider = xAction.collider;
					MouseActionState xState = xAction.state;
					
					switch ( xState )
					{
						case MouseActionState.MouseActionPossible:
						{
							if ( xThisCollider == xColliderHit )
							{
								xAction.OnHover( xCursorPosition );
								SendButtonPressEventsForAction( xAction );
							}
							break;
						}

						case MouseActionState.MouseActionBegan:
						case MouseActionState.MouseActionChanged:
						{
							xAction.OnHover( xCursorPosition );
							SendButtonPressEventsForAction( xAction );	
						
							break;
						}
						
						case MouseActionState.MouseActionEnded:
						case MouseActionState.MouseActionRecognised:
						{
							xAction.ResetAction();
							xAction.state = MouseActionState.MouseActionPossible;
							break;
						}
						
						case MouseActionState.MouseActionFailed:
						{
							//TODO: Only Reset after all mouse buttons have been released.
							xAction.ResetAction();
							xAction.state = MouseActionState.MouseActionPossible;
							break;
						}
					}
				}
			}
		}
		
		private Collider ColliderHit()
		{
			Vector2 xTouchPoint = Input.mousePosition;
			Ray xTouchRay = m_camera.ScreenPointToRay( xTouchPoint );
			RaycastHit xHitInfo;
			
			int  iLayerMask = 1 << LayerMask.NameToLayer( LAYER_MASK_NAME );
			bool bRaycastHit = Physics.Raycast( xTouchRay, out xHitInfo, 50.0f, iLayerMask );

			if ( bRaycastHit )
			{
				return xHitInfo.collider;
			}
			return null;
		}
		
		private void SendButtonPressEventsForAction( MouseAction xAction )
		{
			int iNumberOfMouseButtons = 3;
			for ( int iMouseButtonIndex = 0; iMouseButtonIndex < iNumberOfMouseButtons; iMouseButtonIndex++ )
			{
				bool bButtonBecameDown = Input.GetMouseButtonDown( iMouseButtonIndex );
				if ( bButtonBecameDown )
				{
					xAction.OnButtonPressed( iMouseButtonIndex );
				}
				
				bool bButtonBecameUp = Input.GetMouseButtonUp( iMouseButtonIndex );
				if ( bButtonBecameUp )
				{
					xAction.OnButtonReleased( iMouseButtonIndex );
				}
			}
		}
		
		private Camera m_camera;
		private List<MouseAction> m_mouseActions;
	}
}