using UnityEngine;
using BaseFramework.Gestures;

public class ChangeColourOnTap : MonoBehaviour
{
	private void Start ()
	{
		Collider xMyCollider = collider;
		TapGestureRecogniser xTapGesture = new TapGestureRecogniser( xMyCollider, OnTap );
		
		GestureManager xGestureManager = GestureManager.Instance;
		xGestureManager.AddGesture( xTapGesture );
		
		m_myMaterial = renderer.material;
	}
	
	private void OnTap( GestureRecogniser xGesture )
	{
		if ( xGesture.gestureState == GestureState.GestureStateRecognised )
		{
			m_currentColourIndex += 1;
			if ( m_currentColourIndex >= m_coloursToCycle.Length )
			{
				m_currentColourIndex = 0;
			}
			
			m_myMaterial.color = m_coloursToCycle[ m_currentColourIndex ];
		}
	}
	
	private Material m_myMaterial;
	private int m_currentColourIndex;
	private Color[] m_coloursToCycle = new Color[]
	{
		Color.red,
		Color.green,
		Color.blue
	};
}
