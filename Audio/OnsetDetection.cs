using UnityEngine;
using System.Collections.Generic;


namespace BaseFramework.Audio
{
	[RequireComponent( typeof( AudioSource ) )]
	public class OnsetDetection : MonoSingleton<OnsetDetection>
	{
		public AudioClip m_audioClip; // The AudioClip to process
		
		public float m_averageEnergyTimeWindow = 1.0f; // The size of the window in which an average energy is calculated
		public float m_instantEnergyTimeWindow = 0.05f; // The size of the window in which an instant energy is calculated
		
		public static float MeanSquareRoot( float[] samples )
		{
			float sumOfSamples = 0.0f;
			for (int i=0; i<samples.Length; i++)
			{
				sumOfSamples += samples[i];
			}
			sumOfSamples /= samples.Length;
			return Mathf.Sqrt( sumOfSamples );
		}
		
		public void DebugTrackInfo()
		{
			if (m_audioClip == null)
				return;
			
			float length = m_audioClip.length;
			int nSamples = m_audioClip.samples;
			int channels = m_audioClip.channels;
			int frequency = m_audioClip.frequency;
			
			Debug.Log( "length: " + length );
			Debug.Log( "channels: " + channels );
			Debug.Log( "nSamples:  " + nSamples );
			Debug.Log( "frequency: " + frequency );
		}
		
		public bool[] ProcessTrack()
		{
			if (m_audioClip == null)
				return null;
			
			return null;
		}
		
		//public float[] FFT( float[] samples )
		//{
		//}
	}
}