using UnityEngine;
using System.Collections;

namespace BaseFramework
{
	[RequireComponent( typeof( Camera ) )]
	public class FirstPersonCameraController : MonoBehaviour
	{
		public Transform CharacterTransform;
		
		public bool InvertX = false;
		public bool InvertY = false;
		
		public float HorizontalSpeed = 10.0f;
		public float VerticalSpeed = 5.0f;
		
		void Start ()
		{
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
			
			transform.Rotate (y, 0, 0, Space.Self); // todo : should rotate about a point.
			CharacterTransform.Rotate (0, x, 0, Space.Self);
		}
	}
}
