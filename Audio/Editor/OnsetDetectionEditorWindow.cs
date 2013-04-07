using UnityEngine;
using UnityEditor;

using BaseFramework.Math;

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
			title = "Onset Detection";
			
			/*
			if (m_sampleTexture == null)
			{
				m_sampleTexture = new Texture2D(2, 2);
				m_sampleTexture.hideFlags = HideFlags.DontSave;
				
				m_sampleTexture.SetPixel( 0, 0, Color.blue );
				m_sampleTexture.SetPixel( 1, 0, Color.blue );
				m_sampleTexture.SetPixel( 0, 1, Color.blue );
				m_sampleTexture.SetPixel( 1, 1, Color.blue );
				m_sampleTexture.Apply();
			}
			
			if (m_currWindow == null)
			{
				m_currWindow = new float[ m_windowResolution ];
			}
			*/
		}
		
		private void OnDisable()
		{
		}
		
		private void OnGUI()
		{
			EditorGUILayout.TextArea( "Onset Detection", BaseGUIStyles.Header);
			
			// Check we have a reference to the OnsetDetection object.
			// If we don't, give the developer the option to create one.
			if (m_onsetDetectionObject == null)
			{
				if ( GUILayout.Button( "Create Onset Detection Singleton" ) )
				{
					OnsetDetectionEditor.CreateOnsetDetectionSingleton();
				}
				m_onsetDetectionObject = (OnsetDetection)GameObject.FindObjectOfType( typeof( OnsetDetection ) );
				return;
			}
			
			// Make sure the local AudioClip variable is synchronised with the OnsetDetection object.
			// If it is not, resynchronise.
			if (m_audio == null || m_audio != m_onsetDetectionObject.m_audioClip)
			{
				// Make sure the OnsetDetection object holds some reference.
				if (m_onsetDetectionObject.m_audioClip != null)
				{
					m_audio = m_onsetDetectionObject.m_audioClip;
					//m_sampleData = new float[ m_audio.samples * m_audio.channels ]; // Need to be able to retrieve FFT of this data!
					//m_audio.GetData( m_sampleData, 0 );
				}
				else
				{
					// The OnsetDetection object has no reference to an AudioClip object.
					// Ask the developer to specify one!
					m_onsetDetectionObject.m_audioClip = (AudioClip)EditorGUILayout.ObjectField( "Audio Clip", m_onsetDetectionObject.m_audioClip, typeof( AudioClip ), false );
				}
				return;
			}
			
			//TestAnalyzer();
			DrawAndHandleControls();
		}
		
		/*
		private void TestAnalyzer()
		{
			// Define a test array with a given window size
			int windowSize = 256;
			float[] test = new float[ windowSize ];
			
			// Populate test array with random values
			for (int i=0; i<windowSize; i++)
				test[ i ] = Random.Range( -1.0f, 1.0f );
			
			// Define Analyzer's Position
			float halfWidth = Screen.width / 2;
			float halfHeight = Screen.height / 2;
			Rect pos = new Rect(
				halfWidth - halfWidth / 2,
				halfHeight - halfHeight / 2,
				halfWidth,
				halfHeight / 4
			);
			
			// Draw it!
			DrawSpectrumAnalysis( test, pos );
		}
		*/
		
		private void DrawAndHandleControls()
		{
			// Define Start / Stop Testing Button
			bool wasPlaying = m_playingAudio;
			if ( !m_playingAudio )
			{
				m_playingAudio = GUILayout.Button( "Test Audio Track" );
				//m_currWindow = new float[ m_windowResolution ];
			}
			else
			{
				m_playingAudio = !GUILayout.Button( "Stop Playing" );
			}
			
			/*
			// Define Debug Fourier Transform Button
			if ( GUILayout.Button ( "Transform" ) )
			{
				m_transformedData = BaseMath.FastFourierTransform_CooleyTukey( m_sampleData );
				//for ( int i=0; i<fTransform.Length; i++ )
				//{
				//	Debug.Log( "[" + i + "] : " + fTransform[i] );
				//}
				
				//Debug.Log("Length : "+fTransform.Length);
			}
			*/
			
			// Handle Start / Stop Playing Events
			if ( m_playingAudio && !wasPlaying )
			{
				// Started playing
				m_onsetDetectionObject.audio.Play();
				//SetCurrentSampleWindow();
			}
			else if ( !m_playingAudio && wasPlaying )
			{
				// Stopped playing the track
				m_onsetDetectionObject.audio.Stop();
			}
			
			/*
			// Define Spectral Analyser
			Rect spectralAnalysizerPosition = new Rect (
				0,
				Screen.height / 2,
				Screen.width,
				Screen.height / 4
			);
			DrawSpectrumAnalysis( m_currWindow, spectralAnalysizerPosition );
			*/
		}
		
		/*
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
		
		private void Update()
		{
			if (m_playingAudio)
			{
				// use a ttl variable to decide which update we shall generate a new window on?
				// we don't really need to generate new windows each time we are called.
				
				if (m_windowUpdateTTL++ > m_windowUpdateRate-1)
				{
					m_windowUpdateTTL = 0;
					SetCurrentSampleWindow();
					Repaint();
				}
			}
		}
		
		private void SetCurrentSampleWindow()
		{
			if (!m_onsetDetectionObject.audio.isPlaying)
				return;
			
			m_currWindow = new float[ m_windowResolution ];
			
			int initialSample = (int)(m_audio.frequency * m_onsetDetectionObject.audio.time);
			for (int i=0; i<m_windowResolution; i++)
			{
				int currSampleIndex = initialSample + i;
				if ( currSampleIndex < m_transformedData.Length )
					m_currWindow[ i ] = m_transformedData[ currSampleIndex ];//m_sampleData[ currSampleIndex ];
				else
					m_currWindow[ i ] = 0.0f;
			}
		}
		*/
		
		/*
		private int m_windowUpdateTTL = 0;
		private int m_windowUpdateRate = 1;
		private int m_windowResolution = 256;
		
		private float[] m_sampleData;
		private float[] m_currWindow;
		private float[] m_transformedData;
		
		private Texture2D m_sampleTexture;
		*/
		
		private AudioClip m_audio;
		private bool m_playingAudio;
		private OnsetDetection m_onsetDetectionObject;
	}
}