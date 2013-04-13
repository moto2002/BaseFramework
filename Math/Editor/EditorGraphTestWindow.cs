using UnityEngine;
using UnityEditor;

namespace BaseFramework.Math
{
	/// <summary>
	/// Editor Window used to test the functionality of the EditorGraph drawing.
	/// </summary>
	public class EditorGraphTestWindow : EditorWindow
	{
		[MenuItem( "BaseFramework/Test Windows/Graph Test" )]
		private static void OpenWindow()
		{
			EditorWindow.GetWindow<EditorGraphTestWindow>();
		}
		
		private void OnEnable()
		{
			m_testGraph = new Graph();
			m_testGraph.Min_Y = -1;
			m_testGraph.Max_Y = 1;
			m_testGraph.WindowWidth = 64;
			m_testGraph.Channels[ 0 ].Data = GenerateTestData();
			m_testGraph.Channels[ 0 ].isActive = true;
		}
		
		private float[] GenerateTestData()
		{
			int sampleDataLength = 1024;
			float[] sampleData = new float[ sampleDataLength ];
			for( int i=0; i<sampleDataLength; i++ )
			{
				sampleData[ i ] = Mathf.Sin( i * Mathf.PI * 0.2f );
				//5+2*Mathf.Sin( i * Mathf.PI / 8f ) + 0.75f * Mathf.Cos( i * 2 * Mathf.PI );
				//5 + 2 * Mathf.Cos( 2 * Mathf.PI * i - (90 * Mathf.Deg2Rad) ) + 3 * Mathf.Cos( 4 * Mathf.PI * i );
			}
			return sampleData;
		}
		
		private float[] GetSampleData()
		{
			float[] sampleData = new float[ m_clip.samples ];
			m_clip.GetData( sampleData, 0 );
			return sampleData;
		}
		
		private void OnGUI()
		{
			AudioClip newClip = (AudioClip)EditorGUILayout.ObjectField( "Audio Clip", m_clip, typeof( AudioClip ), false );
			m_testGraph.WindowWidth = EditorGUILayout.IntField( "Resolution", m_testGraph.WindowWidth );
			m_testGraph.X_Offset = EditorGUILayout.IntField( "Sample Position", m_testGraph.X_Offset );
			
			EditorGUILayout.BeginHorizontal();
			m_testGraph.Min_Y = EditorGUILayout.FloatField( "Min Limit", m_testGraph.Min_Y );
			m_testGraph.Max_Y = EditorGUILayout.FloatField( "Max Limit", m_testGraph.Max_Y );
			if ( GUILayout.Button( "Reset" ) )
			{
				m_testGraph.Max_Y = 1;
				m_testGraph.Min_Y = -1;
			}
			EditorGUILayout.EndHorizontal();
			
			if (newClip != m_clip)
			{
				m_clip = newClip;
				if (newClip == null)
					m_testGraph.Channels[ 0 ].Data = GenerateTestData();
				else
					m_testGraph.Channels[ 0 ].Data = GetSampleData();
			}
			
			if (m_testGraph.Channels[ 0 ].Data != null)
				EditorGraph.DrawGraph( m_testGraph, new Rect( 0, Screen.height/2, Screen.width, Screen.height/4 ) );
		}
		
		private AudioClip m_clip;
		private Graph m_testGraph;
	}
}