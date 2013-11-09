using UnityEngine;
using System.Collections.Generic;
using BaseFramework.Core;

namespace BaseFramework.Gestures
{
	/// <summary>
	/// Gesture manager. Facilitates adding, removing, and updating gesture recognisers.
	/// </summary>
	public class GestureManager : MonoSingleton<GestureManager>
	{
		public static string LAYER_MASK_NAME = "Input";
		
		public void AddGesture( GestureRecogniser xGesture )
		{
			m_pxGestureRecognisers.Add( xGesture );
		}
		
		public void RemoveGesture( GestureRecogniser xGesture )
		{
			m_pxGestureRecognisers.Remove( xGesture );
		}
		
		private void Awake()
		{
			GameObject xCameraGameObject = GameObject.FindGameObjectWithTag( "MainCamera" );
			m_pxCamera = xCameraGameObject.GetComponent<Camera>();
			m_pxGestureRecognisers = new List<GestureRecogniser>();
		}
		
		private void Update()
		{
			Touch[] pxTouches    = Input.touches;
			int iNumberOfTouches = Input.touchCount;
			
			// Update each GestureRecogniser
			foreach ( GestureRecogniser pxGesture in m_pxGestureRecognisers )
			{
				Collider pxGestureCollider = pxGesture.gestureCollider;
				
				// With each Touch.
				for ( int iTouchIndex = 0; iTouchIndex < iNumberOfTouches; iTouchIndex++ )
				{
					Touch pxTouch = pxTouches[ iTouchIndex ];
					bool bTouchIntersectsCollider = InputIntersectsCollider( pxTouch, pxGestureCollider );
					
					pxGesture.ReceiveTouch( pxTouch, bTouchIntersectsCollider );
				}
			}
		}
		
		private bool InputIntersectsCollider( Touch pxTouch, Collider pxCollider )
		{
			Vector2 pxTouchPoint = pxTouch.position;
			
			RaycastHit pxHitInfo;
			Ray pxTouchRay = m_pxCamera.ScreenPointToRay( pxTouchPoint );
			
			int iLayerMask = 1 << LayerMask.NameToLayer( LAYER_MASK_NAME );
			
			bool bRaycastHit = Physics.Raycast( pxTouchRay, out pxHitInfo, 50.0f, iLayerMask );
			if ( bRaycastHit )
			{
				//TODO: Lookup Collider in a Dictionary<Collider, List<GestureRecogniser>>.
				bool bHitThisCollider = pxHitInfo.collider == pxCollider;
				if ( bHitThisCollider )
				{
					Vector3 xHitPoint = pxHitInfo.point;
					Bounds xColliderBounds = pxCollider.bounds;
					
					bool bIntersects = xColliderBounds.Contains( xHitPoint );
					return bIntersects;
				}
			}
			return false;
		}
		
		private Camera m_pxCamera;
		private List<GestureRecogniser> m_pxGestureRecognisers;
	}
}