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
			GestureRecogniser[] xTempArray = m_gestures;
			int iNewArraySize = xTempArray.Length + 1;
			
		  	m_gestures = new GestureRecogniser[ iNewArraySize ];
			for ( int iGestureIndex = 0; iGestureIndex < iNewArraySize; iGestureIndex++ )
			{
				m_gestures[ iGestureIndex ] = xGesture;
			}
		}
		
		public void RemoveGesture( GestureRecogniser xGesture )
		{
			int iFoundGestureAtIndex = -1;
			int iNumberOfGestures = m_gestures.Length;
			
			// Search for InputHandler
			for ( int iGestureIndex = 0; iGestureIndex < iNumberOfGestures; iGestureIndex++ )
			{
				GestureRecogniser xThisGesture = m_gestures[ iGestureIndex ];
				if ( xThisGesture == xGesture )
				{
					iFoundGestureAtIndex = iGestureIndex;
					break;
				}
			}
			
			// Remove InputHandler if we found it.
			if ( iFoundGestureAtIndex >= 0 )
			{
				GestureRecogniser[] xTempArray = m_gestures;
				int iNewArraySize = iNumberOfGestures - 1;
				
				m_gestures = new GestureRecogniser[ iNewArraySize ];
				for ( int iGestureIndex = 0; iGestureIndex < iNewArraySize; iGestureIndex++ )
				{
					// Check this isn't the Handler to remove
					if ( iGestureIndex != iFoundGestureAtIndex )
					{
						// Make sure we correct the index for InputHandlers who came after the
						// Handler we are to remove (shift them a space forward in the array)
						if ( iGestureIndex >= iFoundGestureAtIndex )
						{
							m_gestures[ iGestureIndex ] = xTempArray[ iGestureIndex - 1 ];
						}
						else
						{
							m_gestures[ iGestureIndex ] = xTempArray[ iGestureIndex ];
						}
					}
				}
			}
		}
		
		private void Awake()
		{
			GameObject xCameraGameObject = GameObject.FindGameObjectWithTag( "MainCamera" );
			m_camera = xCameraGameObject.GetComponent<Camera>();
			m_gestures = new GestureRecogniser[0];
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
			
			for ( int iGestureIndex = 0; iGestureIndex < m_gestures.Length; iGestureIndex++ )
			{
				GestureRecogniser xHandler = m_gestures[ iGestureIndex ];
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
				
				bool bRaycastHit = xCollider.Raycast( xTouchRay, out xHitInfo, 50.0f );
				if ( bRaycastHit )
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
			return xValidTouchInput.ToArray();
		}
		
		private Camera m_camera;
		private GestureRecogniser[] m_gestures;
	}
}