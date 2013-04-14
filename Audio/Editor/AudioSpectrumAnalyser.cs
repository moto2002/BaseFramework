using UnityEngine;
using UnityEditor;

using BaseFramework.Math;

namespace BaseFramework.Audio
{
	/// <summary>
	/// Audio spectrum analyser.
	/// 
	/// This draws completly wrong.
	/// X axis should represent frequency (currently represents time)
	/// y axis should represent power in dB
	/// 
	/// TODO : Automatic Scaling!
	/// </summary>
	[InitializeOnLoad]
	public static class AudioSpectrumAnalyser
	{
		static AudioSpectrumAnalyser()
		{
			m_sampleTexture = new Texture2D( 2, 2 );
			m_sampleTexture.hideFlags = HideFlags.DontSave;
			
			m_sampleTexture.SetPixel( 0, 0, Color.blue );
			m_sampleTexture.SetPixel( 1, 0, Color.blue );
			m_sampleTexture.SetPixel( 0, 1, Color.blue );
			m_sampleTexture.SetPixel( 1, 1, Color.blue );
			m_sampleTexture.Apply();
		}
		
		public static void Draw( Rect position, ComplexNumber[] data )
		{
			float[] realData = new float[ data.Length ];
			for (int i=0; i<realData.Length; i++)
			{
				realData[ i ] = data[ i ].Magnitude;
			}
			Draw( position, realData );
		}
		
		public static void Draw( Rect position, float[] data )
		{
			Draw ( position, data, 1 );
		}
		
		public static void Draw( Rect position, float[] data, int resolution )
		{
			DrawAxis( position );
			DrawSpectrumAnalysis( position, data, resolution );
		}
		
		private static void DrawAxis( Rect bounds )
		{
			GUI.Box( bounds, "Spectrum Analysis" );
		}
		
		private static void DrawSpectrumAnalysis( Rect bounds, float[] data, int resolution )
		{
			float baseLine = bounds.y + bounds.height / 2;
			float sampleWidth = bounds.width / (data.Length / 2 / resolution);
			
			NormaliseData( ref data );
			
			for (int i=0; i<data.Length/2; i+=resolution)
			{
				float sampleMagnitude = 0.0f; // = data[ i ] * bounds.height/2;
				for (int j=0; j<resolution; j++)
					sampleMagnitude += data[ i+j ];
				sampleMagnitude /= resolution;
				sampleMagnitude *= bounds.height / 2;
				
				Rect pos = new Rect(
					bounds.xMin + (i / resolution) * sampleWidth,
					sampleMagnitude > 0 ? baseLine - sampleMagnitude : baseLine,
					sampleWidth,
					Mathf.Abs( sampleMagnitude )
				);
				GUI.DrawTexture( pos, m_sampleTexture );
			}
		}
		
		private static void NormaliseData( ref float[] data )
		{
			float maxValue = 1.0f;
			foreach (float val in data)
				maxValue = Mathf.Abs( val ) > maxValue ? Mathf.Abs( val ) : maxValue;
			for (int i=0; i<data.Length; i++)
				data[ i ] /= maxValue;	
		}
		
		private static Texture2D m_sampleTexture;
	}
}