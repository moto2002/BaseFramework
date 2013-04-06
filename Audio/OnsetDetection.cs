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
		
		// clip.GetData retrieves gain as float values with a range of -1.0 - 1.0
		public void DebugTrackInfo()
		{
			if (m_audioClip == null)
				return;
			
			float length = m_audioClip.length;
			int nSamples = m_audioClip.samples;
			int channels = m_audioClip.channels;
			int frequency = m_audioClip.frequency;
			
			Debug.Log( "length: " + length );
			Debug.Log( "nSamples:  " + nSamples );
			Debug.Log( "channels: " + channels );
			Debug.Log( "frequency: " + frequency );
			
			Debug.Log( "Calculated number of Samples: "+(length*frequency) ); // correct!
		}
		
		bool[] m_data;
		public bool BeatDetectedThisFrame()
		{
			return m_data[((int)audio.time*1000)];
		}
		
		public void Process()
		{
			m_data = SimpleSoundEnergy_1();
		}
		
		public bool[] SimpleSoundEnergy_1(  )
		{
			bool[] beatDetectedAtTimeInterval = new bool[(int)(m_audioClip.length * 1000)];
			
			float sensitivity = 1.3f;
			
			int averageSampleWindow = (int)(m_audioClip.frequency * m_averageEnergyTimeWindow * 2);
			int instantSampleWindow = (int)(m_audioClip.frequency * m_instantEnergyTimeWindow * 2);
			
			//Debug.Log( "nSamples in Average Window: " + averageSampleWindow );
			//Debug.Log( "nSamples in Instant Window: " + instantSampleWindow );
			for (float timeInterval=0.0f; timeInterval<m_audioClip.length-m_instantEnergyTimeWindow; timeInterval += m_instantEnergyTimeWindow)
			{
				int currSample = (int)(m_audioClip.frequency * timeInterval);
				
				float localInstantEnergy = 0.0f;
				float[] instantWindow = new float[instantSampleWindow];
				m_audioClip.GetData( instantWindow, currSample );
				for (int i=0; i<instantWindow.Length; i++)
					localInstantEnergy += instantWindow[i];
				
				float localAverageEnergy = 0.0f;
				float[] averageWindow = new float[averageSampleWindow];
				m_audioClip.GetData( averageWindow, currSample );
				for (int i=0; i<averageWindow.Length; i++)
					localAverageEnergy += averageWindow[i];
				localAverageEnergy *= instantSampleWindow / averageSampleWindow;
				
				int milliInterval = (int)timeInterval*1000;
				beatDetectedAtTimeInterval[milliInterval] = localInstantEnergy > sensitivity * localAverageEnergy;
			}
			
			return beatDetectedAtTimeInterval;
		}
		
		//public float[] FFT( float[] samples )
		//{
		//}
	}
}