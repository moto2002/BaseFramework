using UnityEngine;
using System.Collections;

namespace BaseFramework
{
	public class CameraDolly : MonoBehaviour
	{
		public Vector3 m_axis;
		public float m_minAngle, m_maxAngle;
		
		private Quaternion m_minQuaternion, m_maxQuaternion;
		
		// TODO : Dolly (Smooth) Paths support
		// TODO : Rotation Constraints
		
		void Start()
		{
		}
		
		void LateUpdate()
		{
			Quaternion currRotation = transform.rotation;
			
			
		}
	}
}