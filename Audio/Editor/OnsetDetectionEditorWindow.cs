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
			title = "Onset Detection Testing";
		}
		
		private void OnGUI()
		{
			if ( m_audio == null )
			{
				m_audio = (AudioClip)EditorGUILayout.ObjectField( "Audio Clip", m_audio, typeof( AudioClip ), false );
			}
			
			if ( m_audio == null )
				return;
			
			if ( m_sampleData == null || m_sampleData.Length != m_audio.samples )
			{
				int windowSize = 1024;
				float yScale = 0.0025f;
				
				m_sampleData = new float[ windowSize ]; // TODO : HANDLE MULTIPLE CHANNELS //* m_audio.channels ];
				m_audio.GetData( m_sampleData, 300 );
				
				m_originalClip.WindowWidth = windowSize;
				m_originalClip.Max_Y = yScale;
				m_originalClip.Min_Y = -yScale;
				m_originalClip.Channels[ 0 ].Data = m_sampleData;
				m_originalClip.Channels[ 0 ].isActive = true;
			}
			
			float width = Screen.width / 2;
			float height = Screen.height / 3;
			EditorGraph.DrawGraph( m_originalClip, new Rect( 0, 0, width, height ) );
		}
		
		private void HammingWindow(int width)
		{
			float[] data = new float[ width ];
			
			m_window.WindowWidth = width;
			//m_window.Channels[ 0 ].Data = 
			m_window.Channels[ 0 ].isActive = true;
		}
		
		private float[] m_sampleData;
		private AudioClip m_audio;
		
		private Graph m_originalClip = new Graph();
		private Graph m_window = new Graph();
		private Graph m_windowedClip = new Graph();
		private Graph m_fftApplied = new Graph();
		private Graph m_fftNoiseReduced = new Graph();
	}
}