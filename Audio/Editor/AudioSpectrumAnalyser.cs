using UnityEngine;
using UnityEditor;

namespace BaseFramework.Audio
{
	public class AudioSpectrumAnalyser
	{
		private void DrawSpectrumAnalysis( float[] data, Rect bounds )
		{
			float baseLine = bounds.y + bounds.height / 2;
			float sampleWidth = bounds.width / data.Length;
			
			for (int i=0; i<data.Length; i++)
			{
				float sampleMagnitude = data[ i ] * bounds.height;
				
				Rect pos = new Rect (
					bounds.xMin + i * sampleWidth,
					sampleMagnitude > 0 ? baseLine - sampleMagnitude : baseLine,
					sampleWidth,
					Mathf.Abs( sampleMagnitude )
				);
				GUI.DrawTexture( pos, m_sampleTexture );
			}
		}
		
		private void SetCurrentSampleWindow( float currAudioTime )
		{
			m_currWindow = new float[ m_windowResolution ];
			
			int initialSample = (int)(m_frequency * currAudioTime);
			for (int i=0; i<m_windowResolution; i++)
			{
				int currSampleIndex = initialSample + i;
				if ( currSampleIndex < m_sampleData.Length )
					m_currWindow[ i ] = m_sampleData[ currSampleIndex ];//m_sampleData[ currSampleIndex ];
				else
					m_currWindow[ i ] = 0.0f;
			}
		}
		
		private bool m_playingAudio;
		
		private int m_frequency;
		
		private int m_windowUpdateTTL = 0;
		private int m_windowUpdateRate = 1;
		private int m_windowResolution = 256;
		
		private float[] m_sampleData;
		private float[] m_currWindow;
		
		private Texture2D m_sampleTexture;
	}
}