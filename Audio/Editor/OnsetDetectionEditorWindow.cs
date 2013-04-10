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
		
		private float[] m_testData;
		private ComplexNumber[] m_fftOutput;
			
		private void OnEnable()
		{
			title = "Onset Detection";
			GenerateTestData();
		}
		
		private void OnDisable()
		{
		}
		
		private void GenerateTestData()
		{
			int testLength = 4;
			m_testData = new float[testLength];
			
			for (int i=0; i<testLength; i++)
				m_testData[ i ] = 5 + 2 * Mathf.Cos( (Mathf.PI / 2) * i - (90 * Mathf.Deg2Rad) ) + 3 * Mathf.Cos( Mathf.PI * i );
			Debug.Log (m_testData[0]);
		}
		
		private void OnGUI()
		{
			EditorGUILayout.TextArea( "Onset Detection", BaseGUIStyles.Header);
			
			// Check we have a reference to the OnsetDetection object.
			// If we don't, give the developer the option to create one.
			if ( m_onsetDetectionObject == null )
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
			if ( m_audio == null || m_audio != m_onsetDetectionObject.m_audioClip )
			{
				// Make sure the OnsetDetection object holds some reference.
				if ( m_onsetDetectionObject.m_audioClip != null )
				{
					m_audio = m_onsetDetectionObject.m_audioClip;
					m_sampleData = new float[ m_audio.samples * m_audio.channels ]; // Need to be able to retrieve FFT of this data!
					m_audio.GetData( m_sampleData, 0 );
				}
				else
				{
					// The OnsetDetection object has no reference to an AudioClip object.
					// Ask the developer to specify one!
					m_onsetDetectionObject.m_audioClip = (AudioClip)EditorGUILayout.ObjectField( "Audio Clip", m_onsetDetectionObject.m_audioClip, typeof( AudioClip ), false );
				}
				return;
			}
			
			if ( GUILayout.Button( "Test" ) )
			{
				m_fftOutput = BaseMath.FastFourierTransform_CooleyTukey( m_testData );
				
				foreach (ComplexNumber n in m_fftOutput)
					Debug.Log( n.RealNumber );
			}
			
			Rect testRect = new Rect();
			testRect.yMin = Screen.height / 2;
			testRect.xMin = 0.0f;
			testRect.yMax = Screen.height;
			testRect.xMax = Screen.width;
			
			//AudioSpectrumAnalyser.Draw( testRect, m_testData );
			//DrawAndHandleControls();
		}
		
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
		}
		
		private AudioClip m_audio;
		private float[] m_sampleData;
		private bool m_playingAudio;
		private OnsetDetection m_onsetDetectionObject;
	}
}