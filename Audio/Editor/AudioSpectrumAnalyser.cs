using UnityEngine;
using UnityEditor;

namespace BaseFramework.Audio
{
	/// <summary>
	/// Audio spectrum analyser.
	/// 
	/// This draws completly wrong.
	/// X axis should represent frequency (currently represents time)
	/// y axis should represent power in dB
	/// 
	/// TODO : Should only ever draw within given rect - never outside of.
	/// </summary>
	public class AudioSpectrumAnalyser
	{
		public AudioSpectrumAnalyser( float[] sampleData ) : this( 44100, sampleData, 256 ) { }
		public AudioSpectrumAnalyser( int frequency, float[] sampleData, int resolution )
		{
			m_frequency = frequency;
			m_sampleData = sampleData;
			m_windowResolution = resolution;
			
			m_sampleTexture = new Texture2D( 2, 2 );
			m_sampleTexture.hideFlags = HideFlags.DontSave;
			
			m_sampleTexture.SetPixel( 0, 0, Color.blue );
			m_sampleTexture.SetPixel( 1, 0, Color.blue );
			m_sampleTexture.SetPixel( 0, 1, Color.blue );
			m_sampleTexture.SetPixel( 1, 1, Color.blue );
			m_sampleTexture.Apply();
		}
		
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
					m_currWindow[ i ] = m_sampleData[ currSampleIndex ];
				else
					m_currWindow[ i ] = 0.0f;
			}
		}
		
		private int m_frequency;
		private int m_windowResolution;
		
		private float[] m_sampleData;
		private float[] m_currWindow;
		
		private Texture2D m_sampleTexture;
	}
}