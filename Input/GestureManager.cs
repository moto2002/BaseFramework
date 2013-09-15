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
		public void AddGesture( GestureRecogniser xGesture )
		{
			m_gestures.Add( xGesture );
		}
		
		public void RemoveGesture( GestureRecogniser xGesture )
		{
			m_gestures.Remove( xGesture );
		}
		
		private void Awake()
		{
			GameObject xCameraGameObject = GameObject.FindGameObjectWithTag( "MainCamera" );
			m_camera = xCameraGameObject.GetComponent<Camera>();
			m_gestures = new List<GestureRecogniser>();
		}
		
		private void Update()
		{
			int iNumberOfTouches = Input.touchCount;
			Touch[] xTouches = Input.touches;
			
			bool bInputBegan      = false;
			bool bInputChanged    = false;
			bool bInputEnded      = false;
			bool bInputCancelled  = false;
			bool bInputStationary = false;
			for ( int iTouchIndex = 0; iTouchIndex < iNumberOfTouches; iTouchIndex++ )
			{
				Touch xThisTouch = Input.touches[ iTouchIndex ];
				TouchPhase xThisTouchPhase = xThisTouch.phase;
				
				bInputBegan      |= xThisTouchPhase == TouchPhase.Began;
				bInputStationary |= xThisTouchPhase == TouchPhase.Stationary;
				bInputChanged    |= xThisTouchPhase == TouchPhase.Moved;
				bInputEnded      |= xThisTouchPhase == TouchPhase.Ended;
				bInputCancelled  |= xThisTouchPhase == TouchPhase.Canceled;
			}
			
			foreach ( GestureRecogniser xHandler in m_gestures )
			{
				int iHandlerTouchesRequired = xHandler.numberOfTouches;
				switch ( xHandler.gestureState )
				{
					case GestureState.GestureStatePossible:
					case GestureState.GestureStateBegan:
					case GestureState.GestureStateChanged:
					{
						if ( bInputBegan && iNumberOfTouches == iHandlerTouchesRequired )
						{
							// Call only with the touches that are within the bounds of the InputHandlers' Collider.
							Collider xHandlerCollider = xHandler.gestureCollider;
							Touch[] xValidTouches = InputIntersectsCollider( xTouches, xHandlerCollider );
							if ( xValidTouches.Length > 0 )
							{
								xHandler.InputBegan( xValidTouches );
							}
						}

						if ( bInputChanged )
						{
							xHandler.InputChanged( xTouches );
						}
						
						if ( bInputStationary )
						{
							xHandler.InputStationary( xTouches );
						}
						
						if ( bInputEnded )
						{
							xHandler.InputEnded( xTouches );
						}
						
						if ( bInputCancelled )
						{
							xHandler.InputCancelled( xTouches );
						}
						
						break;
					}
						
					case GestureState.GestureStateEnded:
					case GestureState.GestureStateRecognised:
					{
						//Return to 'Possible' state.
						xHandler.ResetGesture();
						xHandler.gestureState = GestureState.GestureStatePossible;
						break;
					}
					
					case GestureState.GestureStateFailed:
					case GestureState.GestureStateCancelled:
					{
						// Return to 'Possible' state when no touches remain.
						if ( iNumberOfTouches == 0 )
						{
							xHandler.ResetGesture();
							xHandler.gestureState = GestureState.GestureStatePossible;
						}
						break;
					}
				}
			}
		}
		
		private Touch[] InputIntersectsCollider( Touch[] xTouchInput, Collider xCollider )
		{
			List<Touch> xValidTouchInput = new List<Touch>();
			foreach ( Touch xThisTouch in xTouchInput )
			{
				Vector2 xTouchPoint = xThisTouch.position;
				Ray xTouchRay = m_camera.ScreenPointToRay( xTouchPoint );
				RaycastHit xHitInfo;
				
				Debug.DrawRay( xTouchRay.origin, xTouchRay.direction * 50.0f, Color.blue, 10.0f );
				
				bool bRaycastHit = Physics.Raycast( xTouchRay, out xHitInfo );
//				bool bRaycastHit = xCollider.Raycast( xTouchRay, out xHitInfo, 50.0f );
				if ( bRaycastHit )
				{
					bool bHitThisCollider = xHitInfo.collider == xCollider;
					if ( bHitThisCollider )
					{
						Vector3 xHitPoint = xHitInfo.point;
						Bounds xColliderBounds = xCollider.bounds;
						
						bool bIntersects = xColliderBounds.Contains( xHitPoint );
						if ( bIntersects )
						{
							xValidTouchInput.Add( xThisTouch );
						}
					}
				}
			}
			return xValidTouchInput.ToArray();
		}
		
		private Camera m_camera;
		private List<GestureRecogniser> m_gestures;
	}
}