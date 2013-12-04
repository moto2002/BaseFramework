using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BaseFramework.Gestures
{
	public delegate void GestureRecogniserDelegate( GestureRecogniser xGesture );
	
	public enum GestureState
	{
		GestureStatePossible,
		GestureStateBegan,
		GestureStateChanged,
		GestureStateEnded,
		GestureStateRecognised,
		GestureStateCancelled,
		GestureStateFailed,
		GestureStateWaiting
	}
	;
	
	public abstract class GestureRecogniser : MonoBehaviour
	{
		#region Properties
		
		public Vector2 Focus {
			get
			{
#if UNITY_EDITOR
				return Input.mousePosition;
#else
				return CalculateFocusFromActiveTouches();
#endif
			}
		}
		
		public Collider GestureCollider {
			get { return m_pxCollider; }
		}
		
		public bool DebugEnabled {
			get { return m_bDebuggingEnabled; }
			set
			{
				m_bDebuggingEnabled = value;
				if ( m_bDebuggingEnabled )
				{
					// Log current state
					if ( m_bDebuggingEnabled )
					{
						Debug.Log( "Gesture State: " + State + "(" + this + ")" );
					}
				}
			}
		}
		
		public GestureState State {
			get { return m_kGestureState; }
			set
			{
				bool bGestureHasEnded = m_kGestureState == GestureState.GestureStateFailed 
					|| m_kGestureState == GestureState.GestureStateCancelled
					|| m_kGestureState == GestureState.GestureStateEnded;
				bool bGestureWillReset = value == GestureState.GestureStatePossible || value == GestureState.GestureStateWaiting;
				bool bGestureHasNoTouchesRegistered = Input.touchCount == 0;
				
				bool bGestureStateIsRepeatable = value == GestureState.GestureStateChanged;
				bool bGestureStateIsDifferent = value != m_kGestureState;
				
				bool bGestureIsDifferentAndNotFinished = (bGestureStateIsDifferent || bGestureStateIsRepeatable) && !bGestureHasEnded;
				bool bGestureWillTransitionFromEndedToPossible = (bGestureHasEnded && bGestureWillReset && bGestureHasNoTouchesRegistered);
								
				if ( bGestureIsDifferentAndNotFinished || bGestureWillTransitionFromEndedToPossible )
				{
					m_kGestureState = value;
					
					// Log the new state
					if ( m_bDebuggingEnabled )
					{
						Debug.Log( "Gesture State: " + State + "(" + this + ")" );
					}
					
					// Perform Action depending on state
					switch ( m_kGestureState )
					{
						case GestureState.GestureStatePossible:
						case GestureState.GestureStateWaiting:
						{
							m_bTrackingGesture = false;
							ResetGesture();
							break;
						}
						
						case GestureState.GestureStateEnded:
						case GestureState.GestureStateRecognised:
						{
							InformDelegates();
							
							bool bHasBuffer = m_pxBuffer != null;
							if ( bHasBuffer )
							{
								State = GestureState.GestureStateWaiting;
							}
							else
							{
								State = GestureState.GestureStatePossible;
							}
							break;
						}
						
						case GestureState.GestureStateBegan:
						case GestureState.GestureStateChanged:
						case GestureState.GestureStateFailed:
						case GestureState.GestureStateCancelled:
						{
							InformDelegates();
							break;
						}
					}
				}
			}
		}
		
		#endregion
		
		#region Gesture Ordering
		
		public void AddDelegate( GestureRecogniserDelegate pxDelegate )
		{
			m_pxGestureDelegates.Add( pxDelegate );
		}
		
		public void RemoveDelegate( GestureRecogniserDelegate pxDelegate )
		{
			m_pxGestureDelegates.Remove( pxDelegate );
		}
		
		public void RequireGestureToFail( GestureRecogniser pxOther )
		{
			State = GestureState.GestureStateWaiting;
			m_pxBuffer = new GestureRecogniserBuffer( pxOther, this );
		}
		
		#endregion
		
		protected abstract IEnumerator ProcessTouchGesture();
		protected abstract IEnumerator ProcessMouseGesture();
		protected abstract void ResetGesture();
		
		private void Awake()
		{
			m_pxCollider = collider;
			m_pxGestureDelegates = new List<GestureRecogniserDelegate>();
			
			GameObject xCameraGameObject = GameObject.FindGameObjectWithTag( "MainCamera" );
			m_pxCamera = xCameraGameObject.GetComponent<Camera>();
		}
		
		private void Update()
		{
			bool bHasTouches = Input.touchCount > 0;
			
			bool bIsReady = State == GestureState.GestureStatePossible;
			bool bIsWaiting = State == GestureState.GestureStateWaiting;
			
			bool bIsTracking = m_bTrackingGesture;
			bool bShouldStartTracking = bHasTouches && !bIsTracking && (bIsReady || bIsWaiting);
			
			if ( bShouldStartTracking )
			{
				bool bHasBuffer = m_pxBuffer != null;
				bool bInputIntesects = InputIntersectsCollider();
				
				if ( bInputIntesects )
				{
					if ( bHasBuffer && bIsWaiting )
					{
//						m_pxBuffer.QueueTouch( pxTouch, bIntersectsWithCollider );
					}
					else
					{
						m_bTrackingGesture = true;
						StartCoroutine( ProcessMouseGesture() );
					}
				}
			}

#if UNITY_EDITOR
			bool bHasMouseInput = Input.GetMouseButtonDown( 0 );
			bool bShouldStartTrackingMouse = bHasMouseInput && !bHasTouches && !bIsTracking && (bIsReady || bIsWaiting);
			if ( bShouldStartTrackingMouse )
			{
				bool bHasBuffer = m_pxBuffer != null;
				bool bInputIntesects = InputIntersectsCollider();

				if ( bInputIntesects )
				{
					if ( bHasBuffer && bIsWaiting )
					{
//						m_pxBuffer.QueueTouch( pxTouch, bIntersectsWithCollider );
					}
					else
					{
						m_bTrackingGesture = true;
						StartCoroutine( ProcessTouchGesture() );
					}
				}
			}
#endif
		}
		
		#region Helper Methods
		
		private void InformDelegates()
		{
			foreach ( GestureRecogniserDelegate pxDelegate in m_pxGestureDelegates )
			{
				pxDelegate( this );
			}
		}
		
		private Vector2 CalculateFocusFromActiveTouches()
		{
			Vector2 pxFocus = Vector2.zero;
			Touch[] pxActiveTouches = Input.touches;
			
			foreach ( Touch pxTouch in pxActiveTouches )
			{
				Vector2 pxTouchPoint = pxTouch.position;
				pxFocus += pxTouchPoint;
			}
			
			int iActiveTouches = Input.touchCount;
			pxFocus /= iActiveTouches;
			
			return pxFocus;
		}
		
		private bool InputIntersectsCollider()
		{
			Collider pxCollider = m_pxCollider;
			Vector2 pxFocus = Focus;
			
			RaycastHit pxHitInfo;
			Ray pxTouchRay = m_pxCamera.ScreenPointToRay( pxFocus );

			bool bRaycastHit =  pxCollider.Raycast( pxTouchRay, out pxHitInfo, Mathf.Infinity );
			if ( bRaycastHit )
			{
				bool bHitThisCollider = pxHitInfo.collider == pxCollider;
				if ( bHitThisCollider )
				{
					Vector3 xHitPoint = pxHitInfo.point;
					Bounds xColliderBounds = pxCollider.bounds;
					
					bool bIntersects = xColliderBounds.Contains (xHitPoint);
					if ( bIntersects )
					{
						return true;
					}
				}
			}
			return false;
		}
		
		#endregion
		
		private Camera m_pxCamera;
		private Collider m_pxCollider;
		
		private bool m_bDebuggingEnabled;
		private bool m_bTrackingGesture;
		
		private GestureState m_kGestureState;
		private GestureRecogniserBuffer m_pxBuffer;
		private List<GestureRecogniserDelegate> m_pxGestureDelegates;
	}
}