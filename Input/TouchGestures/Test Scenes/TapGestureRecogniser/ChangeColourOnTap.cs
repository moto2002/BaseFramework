using UnityEngine;
using BaseFramework.Gestures;

namespace BaseFramework.Gestures.TestScenes
{
	public class ChangeColourOnTap : MonoBehaviour
	{
		private void Start()
		{
			TapGestureRecogniser pxTapGesture = GetComponent<TapGestureRecogniser>();
			pxTapGesture.AddDelegate( OnTap );
			pxTapGesture.DebugEnabled = true;
			m_myMaterial = renderer.material;
		}
		
		private void OnTap( GestureRecogniser xGesture )
		{
			if ( xGesture.State == GestureState.GestureStateRecognised )
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
}
