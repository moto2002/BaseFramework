using UnityEngine;
using System.Collections;

namespace BaseFramework
{
	[RequireComponent (typeof (Collider))]
	public class OnScreen : MonoBehaviour
	{
		private Camera m_camera;
		
		private bool m_wasOnScreen;
		private float m_offScreenDistance = 1.0f;
		
		void Start ()
		{
			m_camera = Camera.mainCamera;
			m_wasOnScreen = IsOnScreen ();
		}
		
		void Update () // todo (functionality / efficiency) : would it be better to use events?
		{
			bool onScreen = IsOnScreen ();
			
			if (onScreen && !m_wasOnScreen)
			{
				// entered view
				SendMessageUpwards ("AppearedOnScreen", collider, SendMessageOptions.DontRequireReceiver);
			}
			
			if (!onScreen && m_wasOnScreen)
			{
				// exited view
				SendMessageUpwards ("WentOffScreen", collider, SendMessageOptions.DontRequireReceiver);
			}
			
			m_wasOnScreen = onScreen;
		}
		
		bool IsOnScreen ()
		{
			Vector3 min = m_camera.WorldToViewportPoint (collider.bounds.min);
			Vector3 max = m_camera.WorldToViewportPoint (collider.bounds.max);
			
			return min.x <= m_offScreenDistance
				&& min.x > 0
				&& min.y <= m_offScreenDistance
				&& min.y > 0
				&& max.x <= m_offScreenDistance
				&& max.x > 0
				&& max.y <= m_offScreenDistance
				&& max.y > 0;
		}
	}
}