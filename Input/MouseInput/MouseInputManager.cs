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
			m_pxCamera = xCameraGameObject.GetComponent<Camera>();
			m_mouseActions = new List<MouseAction>();
		}
		
		private void Update()
		{
			Vector3 pxCursorPosition = Input.mousePosition;
			
			foreach ( MouseAction pxAction in m_mouseActions )
			{
				Collider pxThisCollider = pxAction.collider;
				bool bColliderHit = ColliderHit( pxThisCollider );
				
				pxAction.ReceivedCursorPosition( pxCursorPosition, bColliderHit );
				SendButtonPressEventsForAction( pxAction, bColliderHit );
			}
		}
		
		#region Helper Methods
		
		private bool ColliderHit( Collider pxThisCollider )
		{
			Vector2 xTouchPoint = Input.mousePosition;
			Ray pxTouchRay = m_pxCamera.ScreenPointToRay( xTouchPoint );
			RaycastHit pxHitInfo;
			
			int  iLayerMask = 1 << LayerMask.NameToLayer( LAYER_MASK_NAME );
			bool bRaycastHit = Physics.Raycast( pxTouchRay, out pxHitInfo, 50.0f, iLayerMask );

			if ( bRaycastHit )
			{
				bool bIntersectsWithCollider = pxHitInfo.collider == pxThisCollider;
				return bIntersectsWithCollider;
			}
			return false;
		}
		
		private void SendButtonPressEventsForAction( MouseAction pxAction, bool bIntersectsWithCollider )
		{
			int iNumberOfMouseButtons = 3;
			for ( int iMouseButtonIndex = 0; iMouseButtonIndex < iNumberOfMouseButtons; iMouseButtonIndex++ )
			{
				bool bButtonBecameDown = Input.GetMouseButtonDown( iMouseButtonIndex );
				if ( bButtonBecameDown )
				{
					pxAction.ReceivedButtonPressed( iMouseButtonIndex, bIntersectsWithCollider );
				}
				
				bool bButtonBecameUp = Input.GetMouseButtonUp( iMouseButtonIndex );
				if ( bButtonBecameUp )
				{
					pxAction.ReceivedButtonReleased( iMouseButtonIndex, bIntersectsWithCollider );
				}
			}
		}
		
		#endregion
		
		private Camera m_pxCamera;
		private List<MouseAction> m_mouseActions;
	}
}