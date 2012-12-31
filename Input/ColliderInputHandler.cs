using UnityEngine;
using System.Collections;

namespace BaseFramework
{
	public class ColliderInputHandler : InputHandler
	{
		private Camera m_camera; // todo : better camera support?
		
		void Start ()
		{
			m_camera = Camera.mainCamera;
		}
		
		bool InputIntersects (Vector3 inputFocus)
		{
			Ray touchRay = m_camera.ScreenPointToRay (inputFocus);
			RaycastHit hitInfo;
			
			if (Physics.Raycast(touchRay, out hitInfo))
			{
				return collider.bounds.Contains (hitInfo.point);
			}
			
			return false;
		}
		
		public override void InputBegan (InputData data)
		{
			if (InputIntersects (data.Focus))
				SendMessageUpwards ("WasTouched", SendMessageOptions.DontRequireReceiver);
		}
		
		public override void InputChanged (InputData data)
		{
			if (InputIntersects (data.Focus))
				SendMessageUpwards ("IsBeingTouched", SendMessageOptions.DontRequireReceiver);
		}
		
		public override void InputStopped (InputData data)
		{
			if (InputIntersects (data.Focus))
				SendMessageUpwards ("StoppedTouching", SendMessageOptions.DontRequireReceiver);
		}
	}
}