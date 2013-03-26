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
			m_cameraDistance = 25.0f; // todo : smoothing when this value changes.
			
			if (CameraPivot == null)
			{
				Debug.LogError ("CameraPivot is null!", this);
			}
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
			float x = Input.GetAxis ("Mouse X") * HorizontalSpeed;
			float y = Input.GetAxis ("Mouse Y") * VerticalSpeed;
			
			x = InvertX ? -x : x;
			y = InvertY ? y : -y;
			
			Quaternion rotation = Quaternion.Euler( y, x, 0 ); // todo : limit vertical rotation
			
			// transform camera about a pivot
			Vector3 currDir = (transform.position - CameraPivot.position).normalized;
			transform.position = CameraPivot.position + (rotation * currDir * m_cameraDistance);
			
			// rotate camera to aim at pivot
			transform.rotation = Quaternion.LookRotation( CameraPivot.position - transform.position );
		}
		
		private float m_cameraDistance;
		private const float MINIMUM_CAMERA_DISTANCE = 5.0f;
		private const float MAXIMUM_CAMERA_DISTANCE = 20.0f;
	}
}
