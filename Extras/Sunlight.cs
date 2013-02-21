using UnityEngine;
using System.Collections;

namespace BaseFramework
{
	[RequireComponent (typeof (Light))]
	public class Sunlight : MonoBehaviour
	{
		public Transform CentreOfMass;
		
		public bool m_cycle = false;
		public float m_timescale;
		
		private float m_orbitRadius;
		private float m_angle = 0.0f;
		
		
		public float DayLengthInSeconds // todo : expose in inspector
		{
			get { return m_timescale; }
			set { m_timescale = value; }
		}
		
		public float DayLengthInMinutes
		{
			get { return m_timescale / 60; } // todo : expose in inspector
			set { m_timescale = value * 60; }
		}
		
		void Start ()
		{
			if (CentreOfMass == null)
			{
				Debug.LogError ("Ne CentreOfMass set.", this);
				return;
			}
			
			light.type = LightType.Directional;
			m_orbitRadius = (transform.position - CentreOfMass.position).magnitude;
		}
		
		
		void Update ()
		{
			if (!m_cycle)
			{
				return;
			}
			
			// trigonometry. shit.
			
			// soh cah toa
			
			//
			//    o          x
			//     \        /
			//      \  _  / 
			//       \''/
			//        O  
			// 
			// 
			
			// so the angle is determined by the orbit speed.
			//
			// deltaAngleDegrees = timescale (seconds) / 360
			//
			// OrbitRandius * sin (deltaAngleDegrees) =
			// OrbitRandius * cos (deltaAngleDegrees) =
			
			m_angle += Mathf.Deg2Rad * (m_timescale/360); // todo: private var, eliminate timescale?
			
			float x = m_orbitRadius * Mathf.Sin (m_angle);
			//Debug.Log ("Sin: " + x);
			
			float y = m_orbitRadius * Mathf.Cos (m_angle);
			//Debug.Log ("Cos: " + y);
			
			Vector3 dir = CentreOfMass.position - transform.position;
			transform.rotation = Quaternion.LookRotation (dir);
			transform.position = new Vector3 (x, y, transform.position.z);
		}
		
		void OnDrawGizmosSelected ()
		{
			if (CentreOfMass != null)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine (transform.position, CentreOfMass.position);
			}
			else
			{
				Debug.LogWarning ("Ne CentreOfMass set.", this);
			}
		}
	}
	
}