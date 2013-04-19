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
			AudioClip newAudio = (AudioClip)EditorGUILayout.ObjectField( "Audio Clip", m_audio, typeof( AudioClip ), false );
			m_sampleDataInitialised = m_audio == newAudio && newAudio != null;
			m_audio = newAudio;
			
			if (m_audio == null)
				return;
			
			if ( m_sampleDataInitialised )
			{
				DrawTimeDomainClip();
			}
			else
			{
				InitTimeDomainClip();
				m_windowedSampleDataInitialised = m_frequencyDataInitialised = false;
			}
			
			if ( m_windowedSampleDataInitialised )
			{
				DrawWindowedClip();
			}
			else
			{
				InitWindowedClip();
				m_frequencyDataInitialised = false;
			}
			
			if ( m_frequencyDataInitialised )
			{
				DrawFrequencyDomainClip();
			}
			else
			{
				InitFrequencyDomainClip();
			}
			
		}
		
		private void DrawTimeDomainClip()
		{
			float width = Screen.width / 2;
			float height = Screen.height / 3;
			EditorGraph.DrawGraph( m_originalClip, new Rect( 0, 0, width, height ) );
		}
		
		private void DrawWindow()
		{
		}
		
		private void DrawWindowedClip()
		{
			float width = Screen.width / 2;
			float height = Screen.height / 3;
			EditorGraph.DrawGraph( m_windowedClip, new Rect( 0, height, width, height ) );
		}
		
		private void DrawFrequencyDomainClip()
		{
			float width = Screen.width / 2;
			float height = Screen.height / 3;
			EditorGraph.DrawGraph( m_fftApplied, new Rect( 0, height*2, width, height ) );
		}
		
		private void InitTimeDomainClip()
		{
			float yScale = 1;
			
			m_sampleData = new float[ s_windowSize ];
			m_audio.GetData( m_sampleData, 0 );
			
			m_originalClip.WindowWidth = s_windowSize;
			m_originalClip.Max_Y = yScale;
			m_originalClip.Min_Y = -yScale;
			m_originalClip.Channels[ 0 ].Data = m_sampleData;
			m_originalClip.Channels[ 0 ].isActive = true;
			
			m_sampleDataInitialised = true;
		}
		
		private void InitWindowedClip()
		{
			m_windowedSampleData = new float[ s_windowSize ];
			// todo : assign windowed data
			
			m_windowedClip.WindowWidth = s_windowSize;
			m_windowedClip.Channels[ 0 ].Data = m_windowedSampleData;
			m_windowedClip.Channels[ 0 ].isActive = true;
			
			m_windowedSampleDataInitialised = true;
		}
		
		private void InitFrequencyDomainClip()
		{
			m_frequencyDomainData = new float[ s_windowSize ];
			// todo : assign frequency domain data
			
			m_fftApplied.WindowWidth = s_windowSize;
			m_fftApplied.Channels[ 0 ].Data = m_frequencyDomainData;
			m_fftApplied.Channels[ 0 ].isActive = true;
			
			m_frequencyDataInitialised = true;
		}
		
		private AudioClip m_audio;
		
		private static int s_windowSize = 44100;
		
		private bool m_sampleDataInitialised = false;
		private bool m_windowedSampleDataInitialised = false;
		private bool m_frequencyDataInitialised = false;
		
		private float[] m_sampleData;
		private float[] m_windowedSampleData;
		private float[] m_frequencyDomainData;
		
		private Graph m_originalClip = new Graph();
		//private Graph m_window = new Graph();
		private Graph m_windowedClip = new Graph();
		private Graph m_fftApplied = new Graph();
		//private Graph m_fftNoiseReduced = new Graph();
	}
}