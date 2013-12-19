using UnityEngine;
using System.Collections;

namespace BaseFramework.Gestures.TestScenes
{
	public class SphereController : MonoBehaviour
	{
		private void Start()
		{
			m_pxRigidbody = rigidbody;
			m_fSwipeVelocityScaleFactor = 1.0f;

			GameObject pxSwipeColliderObject = GameObject.FindGameObjectWithTag( "Input" );
			SwipeGestureRecogniser pxSwipeGesture = pxSwipeColliderObject.GetComponent<SwipeGestureRecogniser>();
			pxSwipeGesture.DebugEnabled = true;
			pxSwipeGesture.AddDelegate( HandleSwipe );
		}

		private void HandleSwipe( GestureRecogniser pxGesture )
		{
			if ( pxGesture.State == GestureState.GestureStateRecognised )
			{
				SwipeGestureRecogniser pxSwipeGesture = (SwipeGestureRecogniser)pxGesture;

				Vector2 pxSwipeVelocity = pxSwipeGesture.Velocity;
				pxSwipeVelocity /= m_fSwipeVelocityScaleFactor;

				Vector3 pxVelocityForce = Vector3.zero;
				pxVelocityForce.x = pxSwipeVelocity.x;
				pxVelocityForce.y = 0.0f;
				pxVelocityForce.z = pxSwipeVelocity.y;

				m_pxRigidbody.AddForce( pxVelocityForce );
			}
		}

		private float m_fSwipeVelocityScaleFactor;
		private Rigidbody m_pxRigidbody;
	}
}
