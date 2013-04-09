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
			
			AudioSpectrumAnalyser.Draw( new float[1], new Rect( Screen.width / 4, Screen.height / 2, Screen.width / 2, Screen.height / 4 ) );
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
		private bool m_playingAudio;
		private OnsetDetection m_onsetDetectionObject;
	}
}