using UnityEngine;
using System.Collections;

namespace BaseFramework
{
	[RequireComponent( typeof( Camera ) )]
	public class ThirdPersonCameraController : MonoBehaviour
	{
		public Transform CameraPivot;
		
		public bool InvertX = false;
		public bool InvertY = false;
		
		public float HorizontalSpeed = 10.0f;
		public float VerticalSpeed = 5.0f;
		
		public float CamDist
		{
			get { return m_cameraDistance; }
			set { m_cameraDistance = Mathf.Clamp( value, MINIMUM_CAMERA_DISTANCE, MAXIMUM_CAMERA_DISTANCE ); }
		}
		
		private void Start()
		{
			m_currDir = (transform.position - CameraPivot.position).normalized;
			m_cameraDistance = 50.0f; // todo : smoothing when this value changes.
		}
		
		private void OnEnable()
		{
			Screen.lockCursor = true;
		}
		
		private void OnDisable()
		{
			Screen.lockCursor = false;
		}
		 
		private void Update()
		{
			// TODO : Slight bug- Mouse Axis hasstrange initial value when launching in editor?
			float x = Input.GetAxis ("Mouse X") * HorizontalSpeed;
			float y = Input.GetAxis ("Mouse Y") * VerticalSpeed;
			
			x = InvertX ? -x : x;
			y = InvertY ? y : -y;
			
			// TODO : Fix rotation calculation-
			// The problem is that the up vector of the camera is always +y. we want it to be able to change.
			Quaternion rotation = Quaternion.AngleAxis( y, transform.right ) * Quaternion.AngleAxis( x, transform.up );
			
			// transform camera about a pivot
			m_currDir = rotation * m_currDir;
			transform.position = CameraPivot.position + (m_currDir * m_cameraDistance);
			
			// rotate camera to aim at pivot
			transform.rotation = Quaternion.LookRotation( CameraPivot.position - transform.position, transform.up );
		}
		
		private float m_cameraDistance;
		private Vector3 m_currDir;
		
		private const float MINIMUM_CAMERA_DISTANCE = 5.0f;
		private const float MAXIMUM_CAMERA_DISTANCE = 20.0f;
	}
}
