using UnityEngine;
using System.Collections;

namespace BaseFramework
{
	[RequireComponent (typeof (Camera))]
	public class CameraDolly : MonoBehaviour
	{
		private Camera m_viewCamera;
		
		// todo : send messages when angular limits are hit
		//        these can be received by the actor script, apply a character rotation or whatever.
		private float m_pitchLimit;
		private float m_yawLimit;
		private float m_rollLimit;
		
		public float PitchAngularLimit
		{
			get { return m_pitchLimit; }
			set { m_pitchLimit = value; }
		}
		
		public float YawAngularLimit
		{
			get { return m_yawLimit; }
			set { m_yawLimit = value; }
		}
		
		public float RollAngularLimit	
		{
			get { return m_rollLimit; }
			set { m_rollLimit = value; }
		}
		
		public Camera ViewCamera
		{
			get { return m_viewCamera; }
		}
		
		void Awake ()
		{
			if (m_viewCamera == null)
			{
				m_viewCamera = GetComponent<Camera> ();
				
				if (m_viewCamera == null)
				{
					Debug.LogError ("No View Camera found!", this);
				}
			}
		}
		
		void LateUpdate()
		{
			//Quaternion maxRotation = Quaternion.Euler (m_pitchLimit, m_yawLimit, m_rollLimit);
			
			
		}
		
		void OnDrawGizmos ()
		{
			Gizmos.color = Color.cyan;
			
			/*
			Quaternion tempRotation = Quaternion.Euler (PitchAngularLimit, YawAngularLimit, RollAngularLimit);
			Vector3 tempPos = Quaternion.LookRotation(transform.forward, transform.up);
			
			Gizmos.DrawLine (transform.position, tempPos);
			*/
		}
	}
}