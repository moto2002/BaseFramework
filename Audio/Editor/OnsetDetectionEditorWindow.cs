using UnityEngine;
using UnityEditor;

using BaseFramework.Math;
using BaseFramework.UnityWiki;

namespace BaseFramework.Audio
{
	public class OnsetDetectionEditorWindow : EditorWindow
	{
		[MenuItem( "BaseFramework/Windows/Audio/Onset Detection" )]
		public static void GetWindow()
		{
			EditorWindow.GetWindow( typeof( OnsetDetectionEditorWindow ) );
		}
		
		private void OnEnable()
		{
			title = "Onset Detection Testing";
		}
		
		private void OnGUI()
		{
			EditorGUILayout.TextArea( "Onset Detection Testing", BaseGUIStyles.Header);
			
			// Grab all the initial data we need
			m_audioSource = (AudioSource)EditorGUILayout.ObjectField( "Audio Source", m_audioSource, typeof( AudioSource ), true );
			
			if ( m_audioSource == null ) return;
			
			m_audio = m_audioSource.clip;
			m_audioSource.clip = (AudioClip)EditorGUILayout.ObjectField( "Audio Clip", m_audio, typeof( AudioClip ), false );
			m_windowSize = EditorGUILayout.IntField( "Window Size", m_windowSize );
			
			if ( m_audio == null ) return;
			
			if ( m_sampleData == null || m_sampleData.Length != m_audio.samples )
			{
				m_sampleData = new float[ m_audio.samples ]; // TODO : HANDLE MULTIPLE CHANNELS //* m_audio.channels ];
				m_audio.GetData( m_sampleData, 0 );
			}
			
			// Define Start / Stop Testing Button
			bool wasPlaying = m_playingAudio;
			if ( !m_playingAudio )
			{
				m_playingAudio = GUILayout.Button( "Test Audio Track" );
			}
			else
			{
				m_playingAudio = !GUILayout.Button( "Stop Playing" );
			}
			
			// Handle Start / Stop Playing Events
			if ( m_playingAudio && !wasPlaying )
			{
				// Started playing
				m_audioSource.Play();
				
				m_currWindow = 0;
				//if ( m_windows == null )
					GenerateWindows();
				
				EditorApplication.update += TransformWindows;
			}
			else if ( !m_playingAudio && wasPlaying )
			{
				// Stopped playing the track
				m_audioSource.Stop();
				EditorApplication.update -= TransformWindows;
			}
			
			if ( m_windows == null ) return;
			
			// Draw Spectrum Analyser
			Rect testRect = new Rect();
			testRect.yMin = Screen.height / 2;
			testRect.xMin = 0.0f;
			testRect.yMax = Screen.height;
			testRect.xMax = Screen.width;
			
			AudioSpectrumAnalyser.Draw( testRect, m_windows[ m_currWindow ] );
		}
		
		private void GenerateWindows()
		{
			int nWindows = m_sampleData.Length / m_audio.channels / m_windowSize;
			
			m_windows = new float[nWindows][];
			m_fftOutput = new ComplexNumber[nWindows][];
			
			for (int windowIndex=0; windowIndex<nWindows; windowIndex++)
			{
				m_windows[ windowIndex ] = new float[ m_windowSize ];
				m_fftOutput[ windowIndex ] = new ComplexNumber[ m_windowSize ];
				
				for (int i=0; i<m_windowSize; i+=m_audio.channels)
				{
					m_windows[ windowIndex ][ i ] += m_sampleData[ m_windowSize * windowIndex + i ];
					//for (int c=0; c<m_audio.channels; c++)
					//	m_windows[ windowIndex ][ i ] += m_sampleData[ m_windowSize * windowIndex + i + c ];
				}
				
				m_fftOutput[ windowIndex ] = BaseDSP.FastFourierTransform( m_windows[ windowIndex ] );
			}
			
			for (int i=0; i<nWindows; i++)
			{
				for (int j=0; j<m_windowSize/2; j++)
				{
					m_windows[ i ][ j ] = m_fftOutput[ i ][ j ].RealNumber;
					m_windows[ i ][ m_windowSize/2 + j ] = m_fftOutput[ i ][ j ].ImaginaryScale;
				}
			}
		}
		
		int m_ttl = 0;
		private void TransformWindows()
		{
			//int interval = 100 / 4;
			
			//if (++m_ttl > interval)
			{
				m_ttl = 0;
				int currSample = (int)(m_audio.frequency * m_audioSource.time);
				m_currWindow = currSample / m_windowSize;
				Repaint();
			}
		}
		
		private int m_windowSize = 1024;
		private int m_currWindow;
		private float[][] m_windows;
		
		private bool m_playingAudio;
		private float[] m_sampleData;
		
		private AudioClip m_audio;
		private AudioSource m_audioSource;
		
		private ComplexNumber[][] m_fftOutput;
	}
}