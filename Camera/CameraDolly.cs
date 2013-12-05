using UnityEngine;
using BaseFramework.Core;

namespace BaseFramework.CameraUtils
{
	public class CameraDolly : MonoBehaviour
	{
		private void Awake()
		{
			m_pxTransform = transform;
			
			Vector3 pxInitialOffset = CalculateOffset();
			
			m_pxMinimumOffset = pxInitialOffset - m_pxLockDistance;
			m_pxMaximumOffset = pxInitialOffset + m_pxLockDistance;
		}
		
		private void LateUpdate()
		{
			Vector3 pxCurrentOffset = CalculateOffset();
			Vector3 pxClampedOffset = pxCurrentOffset.Clamp( m_pxMinimumOffset, m_pxMaximumOffset );
			
			Vector3 pxObjectPosition = m_pxTrackedTransform.position;
			Vector3 pxNewCameraPosition = pxObjectPosition + pxClampedOffset;
			
			m_pxTransform.position = pxNewCameraPosition;
		}
		
		private Vector3 CalculateOffset()
		{
			Vector3 pxCameraPosition = m_pxTransform.position;
			Vector3 pxObjectPosition = m_pxTrackedTransform.position;
			
			Vector3 pxOffset = pxCameraPosition - pxObjectPosition;
			return pxOffset;
		}
		
		private Vector3 m_pxMinimumOffset;
		private Vector3 m_pxMaximumOffset;
		private Transform m_pxTransform;
		
		//TODO: Expose following ivars using editor script (for correct name label)
		public Transform m_pxTrackedTransform;
		public Vector3 m_pxLockDistance; // Distance object can move before camera will begin to follow.
//		public Vector3 m_pxLockRotation; //TODO: Degrees camera can rotate around object.
	}
}