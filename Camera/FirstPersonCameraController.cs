using UnityEngine;
using System.Collections;

namespace BaseFramework
{
	public class FirstPersonCameraController : MonoBehaviour
	{
		public Transform CameraTransform;
		public Transform CharacterTransform;
		
		public bool InvertX = false;
		public bool InvertY = false;
		
		public float HorizontalSpeed = 10.0f;
		public float VerticalSpeed = 5.0f;
		
		void Start ()
		{
			if (CameraTransform == null)
			{
				Debug.LogError ("CameraTransform is null!", this);
			}
			
			if (CharacterTransform == null)
			{
				Debug.LogError ("CharacterTransform is null!", this);
			}
		}
		
		void OnEnable ()
		{
			Screen.lockCursor = true;
		}
		
		void OnDisable ()
		{
			Screen.lockCursor = false;
		}
		
		void Update ()
		{
			float x = Input.GetAxis ("Mouse X") * HorizontalSpeed;
			float y = Input.GetAxis ("Mouse Y") * VerticalSpeed;
			
			x = InvertX ? -x : x;
			y = InvertY ? y : -y;
			
			// todo: mouse smoothing
			
			//x *= Time.deltaTime;
			//y *= Time.deltaTime;
			
			CameraTransform.Rotate (y, 0, 0, Space.Self);
			CharacterTransform.Rotate (0, x, 0, Space.Self);
		}
	}
}
