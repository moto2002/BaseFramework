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
	};
	
	public abstract class GestureRecogniser : MonoBehaviour
	{
		public static string LAYER_MASK_NAME = "Input";
		
		#region Properties
		
		public Vector2 Focus {
			get {
				return CalculateFocusFromActiveTouches();
			}
		}
		
		public Collider GestureCollider {
			get {
				if ( m_pxCollider == null ) {
					m_pxCollider = collider;
				}
				return m_pxCollider;
			}
		}
		
		public bool DebugEnabled {
			get { return m_bDebuggingEnabled; }
			set {
				m_bDebuggingEnabled = value;
				if ( m_bDebuggingEnabled ) {
					// Log current state
					if ( m_bDebuggingEnabled ) {
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
							ResetGesture();
							break;
						}
						
						case GestureState.GestureStateBegan:
						case GestureState.GestureStateChanged:
						{
							InformDelegates();
							break;
						}
						
						case GestureState.GestureStateEnded:
						case GestureState.GestureStateRecognised:
						{
							InformDelegates();
							break;
						}
						
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
		
		protected virtual IEnumerator ProcessGesture()
		{
			yield return null;
		}
		
		protected virtual void ResetGesture()
		{
		}
		
		private void Awake()
		{
			m_pxGestureDelegates = new List<GestureRecogniserDelegate>();
			
			GameObject xCameraGameObject = GameObject.FindGameObjectWithTag( "MainCamera" );
			m_pxCamera = xCameraGameObject.GetComponent<Camera>();
		}
		
		private void Update()
		{
//			TouchPhase kTouchPhase = pxTouch.phase;
//			if ( m_pxBuffer != null && State == GestureState.GestureStateWaiting )
//			{
//				m_pxBuffer.QueueTouch( pxTouch, bIntersectsWithCollider );
//			}
//			else
//			{
//				
//			}
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
		
		private bool InputIntersectsCollider( Touch pxTouch )
		{
			Collider pxCollider = m_pxCollider;
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
		
		#endregion
		
		private Camera m_pxCamera;
		private Collider m_pxCollider;
		private bool m_bDebuggingEnabled;
		private GestureState m_kGestureState;
		private GestureRecogniserBuffer m_pxBuffer;
		private List<GestureRecogniserDelegate> m_pxGestureDelegates;
	}
}