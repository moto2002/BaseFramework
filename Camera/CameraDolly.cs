using UnityEngine;
using BaseFramework.Core;

namespace BaseFramework.CameraUtils
{
	[RequireComponent( typeof( Camera ) )]
	public class CameraDolly : MonoBehaviour
	{
		private void Awake()
		{
			m_pxTransform = transform;

			float fMinViewportLimitX = 0.5f + 0.5f * m_fViewportLockX;
			float fMinViewportLimitY = 0.5f + 0.5f * m_fViewportLockY;

			float fMaxViewportLimitX = 0.5f - 0.5f * m_fViewportLockX;
			float fMaxViewportLimitY = 0.5f - 0.5f * m_fViewportLockY;

			Vector3 pxCameraPosition = m_pxTransform.position;
			Vector3 pxObjectPosition = m_pxTrackedTransform.position;
			float fCameraDistance = pxCameraPosition.z - pxObjectPosition.z;

			Vector3 pxMinOffsetViewport = new Vector3( fMinViewportLimitX, fMinViewportLimitY, fCameraDistance );
			Vector3 pxMaxOffsetViewport = new Vector3( fMaxViewportLimitX, fMaxViewportLimitY, fCameraDistance );

			Camera pxCamera = camera;
			Vector3 pxCameraWorldOrigin = pxCamera.ViewportToWorldPoint( Vector3.zero );

			m_pxMinimumOffset = m_pxCamera.ViewportToWorldPoint( pxMinOffsetViewport );
			m_pxMinimumOffset -= pxCameraWorldOrigin;

			m_pxMaximumOffset = m_pxCamera.ViewportToWorldPoint( pxMaxOffsetViewport );
			m_pxMaximumOffset -= pxCameraWorldOrigin;
		}
		
		private void Update()
		{
			ClampToWorldLimits();
		}

		private void ClampToWorldLimits()
		{
			Vector3 pxCurrentOffset = CalculateWorldPositionOffset();
			Vector3 pxClampedOffset = pxCurrentOffset.Clamp( m_pxMinimumOffset, m_pxMaximumOffset );

			Debug.Log( pxCurrentOffset + " - " + pxClampedOffset );

			Vector3 pxObjectPosition = m_pxTrackedTransform.position;
			Vector3 pxNewCameraPosition = pxObjectPosition + pxClampedOffset;
			
			m_pxTransform.position = pxNewCameraPosition;
		}

		private Vector3 CalculateWorldPositionOffset()
		{
			Vector3 pxCameraPosition = m_pxTransform.position;
			Vector3 pxObjectPosition = m_pxTrackedTransform.position;
			
			Vector3 pxOffset = pxCameraPosition - pxObjectPosition;
			return pxOffset;
		}
		
		private Vector3 m_pxMinimumOffset;
		private Vector3 m_pxMaximumOffset;
		private Transform m_pxTransform;
		private Camera m_pxCamera;

		public Transform m_pxTrackedTransform;
		public float m_fViewportLockX;
		public float m_fViewportLockY;

		public Vector3 m_pxLockDistance; // Distance object can move before camera will begin to follow.
//		public Vector3 m_pxLockRotation; //TODO: Degrees camera can rotate around object.
	}
}