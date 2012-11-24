using UnityEngine;
using System.Collections;

namespace GenericLib
{
	public class TouchColliderHandler : TouchHandler
	{
		private Camera m_camera;
		
		void Start ()
		{
			m_camera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		}
		
		bool IsTouching (Vector3 touchPos)
		{
			Ray touchRay = m_camera.ScreenPointToRay (touchPos);
			RaycastHit hitInfo;
			
			if (Physics.Raycast(touchRay, out hitInfo))
			{
				return collider.bounds.Contains(hitInfo.point);
			}
			
			return false;
		}
		
		public override void TouchStart (TouchGestures.Finger f)
		{
			if (IsTouching (f.m_position))
				SendMessageUpwards ("WasTouched", SendMessageOptions.DontRequireReceiver);
		}
		
		public override void TouchUpdate (TouchGestures.Finger f)
		{
			if (IsTouching (f.m_position))
				SendMessageUpwards ("IsBeingTouched", SendMessageOptions.DontRequireReceiver);
		}
		
		public override void TouchEnd (TouchGestures.Finger f)
		{
			if (IsTouching (f.m_position))
				SendMessageUpwards ("StoppedTouching", SendMessageOptions.DontRequireReceiver);
		}
	}
}