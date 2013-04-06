using UnityEngine;
using UnityEditor;

namespace BaseFramework.Audio
{
	[CustomEditor( typeof( OnsetDetection ) )]
	public class OnsetDetectionEditor : Editor
	{
		[MenuItem( "BaseFramework/Add/Audio/Onset Detection" )]
		public static void CreateAudioProcesserSingleton()
		{
			Object obj = GameObject.FindObjectOfType( typeof( OnsetDetection ) );
			if (obj == null)
			{
				GameObject go = new GameObject();
				go.name = "Singleton.OnsetDetection";
				go.AddComponent<OnsetDetection>();
				Selection.activeObject = go;
			}
			else
			{
				Selection.activeObject = obj;
			}
		}
		
		private void OnEnable()
		{
			if (m_onBeat == null)
			{
				m_onBeat = new Texture2D( 2, 2, TextureFormat.ARGB32, false );
				m_onBeat.hideFlags = HideFlags.DontSave;
				
				m_onBeat.SetPixel( 0, 0, Color.blue );
				m_onBeat.SetPixel( 0, 1, Color.blue );
				m_onBeat.SetPixel( 1, 0, Color.blue );
				m_onBeat.SetPixel( 1, 1, Color.blue );
				m_onBeat.Apply();
			}
			
			if (m_offBeat == null)
			{
				m_offBeat = new Texture2D( 2, 2, TextureFormat.ARGB32, false );
				m_offBeat.hideFlags = HideFlags.DontSave;
				
				m_offBeat.SetPixel( 0, 0, Color.red );
				m_offBeat.SetPixel( 0, 1, Color.red );
				m_offBeat.SetPixel( 1, 0, Color.red );
				m_offBeat.SetPixel( 1, 1, Color.red );
				m_offBeat.Apply();
			}
		}
		
		private void OnDisable()
		{
			if (m_testing)
			{
				m_testing = false;
				Target.audio.Stop();
				EditorApplication.update -= DetectAndDisplayBeats;
			}
		}
		
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
			
			// Set the audiosource's clip to the same value as the AudioProcessing objects' clip.
			Target.audio.clip = Target.m_audioClip;
			
			// Debug Info Button
			if ( GUILayout.Button( "Output Debug Information" ) )
			{
				Target.DebugTrackInfo();
			}
			
			// Process Track Button
			if ( GUILayout.Button( "Process Track" ) )
			{
				Target.Process();
			}
			
			// Display "Test" / "Stop Test" Button and check for beat detection
			bool wasTesting = m_testing;
			if (!m_testing)
			{
				m_testing = GUILayout.Button( "Test" );
			}
			else
			{
				m_testing = !GUILayout.Button( "Stop Test" );
			}
			
			// Start / Stop the Test
			if (!wasTesting && m_testing)
			{
				Target.audio.Play();
				EditorApplication.update += DetectAndDisplayBeats;
			}
			else if (wasTesting && !m_testing)
			{
				Target.audio.Stop();
				m_beatDetected = false;
				EditorApplication.update -= DetectAndDisplayBeats;
			}
			
			GUI.DrawTexture( new Rect( 0, 0, 64, 64 ), m_beatDetected ? m_onBeat : m_offBeat );
		}
		
		private void DetectAndDisplayBeats()
		{
			bool beatWasDetected = m_beatDetected;
			m_beatDetected = Target.BeatDetectedThisFrame();
			
			if (m_beatDetected && !beatWasDetected || !m_beatDetected && beatWasDetected)
			{
				Repaint();
			}
		}
		
		private OnsetDetection Target { get { return (OnsetDetection)target; } }
		
		private static bool m_testing;
		private bool m_beatDetected;
		
		private static Texture2D m_onBeat;
		private static Texture2D m_offBeat;
	}
}