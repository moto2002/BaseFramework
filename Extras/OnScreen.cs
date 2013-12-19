using UnityEngine;
using System.Collections;

namespace BaseFramework
{
    [RequireComponent (typeof( Collider ))]
    public class OnScreen : MonoBehaviour
    {
        private void Awake()
        {
            m_pxCamera = Camera.main;
            m_pxCollider = collider;
            m_bWasOnScreen = IsOnScreen();
        }
        
        private void OnEnable()
        {
            m_bWasOnScreen = IsOnScreen();
        }
		
        private void Update()
        {
            bool bOnScreen = IsOnScreen();
			
            if( bOnScreen && !m_bWasOnScreen )
            {
                // entered view
                SendMessageUpwards( "AppearedOnScreen", m_pxCollider, SendMessageOptions.DontRequireReceiver );
            }
			
            if( !bOnScreen && m_bWasOnScreen )
            {
                // exited view
                SendMessageUpwards( "WentOffScreen", m_pxCollider, SendMessageOptions.DontRequireReceiver );
            }
			
            m_bWasOnScreen = bOnScreen;
        }
		
        private bool IsOnScreen()
        {
            Bounds pxBounds = m_pxCollider.bounds;
            Vector3 pxMin = m_pxCamera.WorldToViewportPoint( pxBounds.min );
            Vector3 pxMax = m_pxCamera.WorldToViewportPoint( pxBounds.max );
			
            bool bMinBoundsAreOnScreen = pxMin.x <= m_fOffScreenDistance && pxMin.x >= 0;
            bMinBoundsAreOnScreen &= pxMin.y <= m_fOffScreenDistance && pxMin.y >= 0;
            
            bool bMaxBoundsAreOnScreen = pxMax.x <= m_fOffScreenDistance && pxMax.x >= 0;
            bMaxBoundsAreOnScreen = pxMax.y <= m_fOffScreenDistance && pxMax.y >= 0;
            
            return bMinBoundsAreOnScreen && bMaxBoundsAreOnScreen;
        }
        
        private bool m_bWasOnScreen;
        private float m_fOffScreenDistance = 1.0f;
        
        private Camera m_pxCamera;
        private Collider m_pxCollider;
    }
}