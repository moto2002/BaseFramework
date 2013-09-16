using UnityEngine;
using BaseFramework.Gestures;

public class DragSphereController : MonoBehaviour
{
	public Camera m_camera;
	
	private void Start()
	{
		m_transform = transform;
		
		Transform xCameraTransform = m_camera.transform;
		Vector3 xCameraPosition = xCameraTransform.position;
		m_distanceFromCamera = Mathf.Abs( xCameraPosition.y - m_transform.position.y );
		
		Collider xCollider = collider;
		LongPressGestureRecogniser xLongPressGesture = new LongPressGestureRecogniser( xCollider, HandleLongPress );
		
		GestureManager xGestureManager = GestureManager.Instance;
		xGestureManager.AddGesture( xLongPressGesture );
	}
	
	private void HandleLongPress( GestureRecogniser xGesture )
	{
		Vector3 xScreenPoint = new Vector3();
		xScreenPoint.x = xGesture.focus.x;
		xScreenPoint.y = xGesture.focus.y;
		xScreenPoint.z = m_distanceFromCamera;
		
		Vector3 xWorldPoint = m_camera.ScreenToWorldPoint( xScreenPoint );
		
		m_transform.position = xWorldPoint;
	}
	
	private Transform m_transform;
	private float m_distanceFromCamera;
}