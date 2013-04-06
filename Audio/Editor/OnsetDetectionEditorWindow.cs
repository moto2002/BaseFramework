using UnityEngine;
using UnityEditor;

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
			
			m_sampleTexture = new Texture2D(2, 2);
			m_sampleTexture.hideFlags = HideFlags.DontSave;
			
			m_sampleTexture.SetPixel( 0, 0, Color.blue );
			m_sampleTexture.SetPixel( 1, 0, Color.blue );
			m_sampleTexture.SetPixel( 0, 1, Color.blue );
			m_sampleTexture.SetPixel( 1, 1, Color.blue );
			m_sampleTexture.Apply();
		}
		
		private void OnDisable()
		{
		}
		
		private void OnGUI()
		{
			EditorGUILayout.TextArea( "Onset Detection", BaseGUIStyles.Header);
			
			// Check we have a reference to the OnsetDetection object.
			// If we don't, give the developer the option to create one.
			if (m_onsetDetection == null)
			{
				if ( GUILayout.Button( "Create Onset Detection Singleton" ) )
				{
					OnsetDetectionEditor.CreateOnsetDetectionSingleton();
				}
				m_onsetDetection = (OnsetDetection)GameObject.FindObjectOfType( typeof( OnsetDetection ) );
				return;
			}
			
			// Make sure the local AudioClip variable is synchronised with the OnsetDetection object.
			// If it is not, resynchronise.
			if (m_audio == null || m_audio != m_onsetDetection.m_audioClip)
			{
				// Make sure the OnsetDetection object holds some reference.
				if (m_onsetDetection.m_audioClip != null)
				{
					m_audio = m_onsetDetection.m_audioClip;
					m_sampleData = new float[ m_audio.samples * m_audio.channels ];
					m_audio.GetData( m_sampleData, 0 );
				}
				else
				{
					// The OnsetDetection object has no reference to an AudioClip object.
					// Ask the developer to specify one!
					m_onsetDetection.m_audioClip = (AudioClip)EditorGUILayout.ObjectField( "Audio Clip", m_onsetDetection.m_audioClip, typeof( AudioClip ), false );
				}
				return;
			}
			
			//*
			int windowSize = 256;
			float[] test = new float[ windowSize ];
			
			for (int i=0; i<windowSize; i++)
				test[ i ] = Random.Range( -1.0f, 1.0f );
			//*/
			
			// Draw Spectrum Analysis of the audio
			
			float halfWidth = Screen.width / 2;
			float halfHeight = Screen.height / 2;
			
			DrawSpectrumAnalysis( test, new Rect( halfWidth - halfWidth / 2, halfHeight - halfHeight / 2, halfWidth, halfHeight / 4 ) );
		}
		
		private void Update()
		{
		}
		
		private void DrawSpectrumAnalysis( float[] data, Rect bounds )
		{
			float baseLine = bounds.y + bounds.height / 2;
			float sampleWidth = bounds.width / data.Length;
			
			for (int i=0; i<data.Length; i++)
			{
				float sampleMagnitude = data[ i ] * bounds.height;
				
				Rect pos = new Rect(
					bounds.xMin + i * sampleWidth,
					sampleMagnitude > 0 ? baseLine - sampleMagnitude : baseLine,
					sampleWidth,
					Mathf.Abs( sampleMagnitude )
				);
				GUI.DrawTexture( pos, m_sampleTexture );
			}
		}
		
		private AudioClip m_audio;
		private float[] m_sampleData;
		private Texture2D m_sampleTexture;
		
		private OnsetDetection m_onsetDetection;
	}
}