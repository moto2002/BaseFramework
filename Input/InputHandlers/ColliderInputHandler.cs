using UnityEngine;
using System.Collections;

namespace BaseFramework.InputManager
{
	/// <summary>
	/// Collider input handler. Sends Messages if Input interacts with any colliders attached or defined as children.
	/// 
	/// Messages:
	/// WasTouched
	/// IsBeingTouched
	/// StoppedTouching
	/// 
	/// Handles:
	/// Mouse Input
	/// Touch Input
	/// </summary>
	public class ColliderInputHandler : InputHandler
	{
		private Collider[] m_colliders;
		private Camera m_camera; // todo : better camera support?
		
		protected override void Start ()
		{
			base.Start();
			
			m_colliders = GetComponentsInChildren<Collider>(true);
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
		
		protected override InputMethod ValidInputMethods ()
		{
			return InputMethod.MouseInput | InputMethod.TouchInput;
		}
		
		public override void OnInputStart (InputData data)
		{
			if (InputIntersects (data.Focus))
				SendMessageUpwards ("WasTouched", SendMessageOptions.DontRequireReceiver);
		}
		
		public override void OnInputTick (InputData data)
		{
			if (InputIntersects (data.Focus))
				SendMessageUpwards ("IsBeingTouched", SendMessageOptions.DontRequireReceiver);
		}
		
		public override void OnInputEnd (InputData data)
		{
			if (InputIntersects (data.Focus))
				SendMessageUpwards ("StoppedTouching", SendMessageOptions.DontRequireReceiver);
		}
	}
}