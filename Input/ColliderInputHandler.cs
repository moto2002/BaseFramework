using UnityEngine;
using System.Collections;

namespace BaseFramework
{
	public class ColliderInputHandler : InputHandler
	{
		private Collider[] m_colliders;
		private Camera m_camera; // todo : better camera support?
		
		void Start ()
		{
			m_colliders = GetComponentsInChildren<Collider>();
			m_camera = Camera.mainCamera;
		}
		
		bool InputIntersects (Vector3 inputFocus)
		{
			bool intersects = false;
			Ray touchRay = m_camera.ScreenPointToRay (inputFocus);
			RaycastHit hitInfo;
			
			if (Physics.Raycast(touchRay, out hitInfo))
			{
				foreach (Collider c in m_colliders)
				{
					intersects |= c.bounds.Contains (hitInfo.point);
				}
				return intersects;
			}
			
			return false;
		}
		
		public override void InputBegan (ref InputData data)
		{
			if (InputIntersects (data.Focus))
				SendMessageUpwards ("WasTouched", SendMessageOptions.DontRequireReceiver);
		}
		
		public override void InputChanged (ref InputData data)
		{
			if (InputIntersects (data.Focus))
				SendMessageUpwards ("IsBeingTouched", SendMessageOptions.DontRequireReceiver);
		}
		
		public override void InputStopped (ref InputData data)
		{
			if (InputIntersects (data.Focus))
				SendMessageUpwards ("StoppedTouching", SendMessageOptions.DontRequireReceiver);
		}
	}
}