using UnityEngine;
using BaseFramework.Gestures;

namespace BaseFramework.Gestures.TestScenes
{
	public class DragSphereController : MonoBehaviour
	{
		public Camera m_camera;
		
		private void Start()
		{
			m_transform = transform;
			
			Transform xCameraTransform = m_camera.transform;
			Vector3 xCameraPosition = xCameraTransform.position;
			m_distanceFromCamera = Mathf.Abs( xCameraPosition.y - m_transform.position.y );

			LongPressGestureRecogniser pxLongPressGesture = GetComponent<LongPressGestureRecogniser>();
			pxLongPressGesture.DebugEnabled = true;
			pxLongPressGesture.AddDelegate( HandleLongPress );
		}
		
		private void HandleLongPress( GestureRecogniser pxGesture )
		{
			GestureState kState = pxGesture.State;

			if ( kState == GestureState.GestureStateChanged )
			{
				Vector3 xScreenPoint = new Vector3();
				xScreenPoint.x = pxGesture.Focus.x;
				xScreenPoint.y = pxGesture.Focus.y;
				xScreenPoint.z = m_distanceFromCamera;
				
				Vector3 xWorldPoint = m_camera.ScreenToWorldPoint( xScreenPoint );
				
				m_transform.position = xWorldPoint;
			}
		}
		
		private Transform m_transform;
		private float m_distanceFromCamera;
	}
}