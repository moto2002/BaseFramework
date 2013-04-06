using UnityEngine;
using UnityEditor;

namespace BaseFramework.Audio
{
	[CustomEditor( typeof( OnsetDetection ) )]
	public class OnsetDetectionEditor : Editor
	{
		[MenuItem( "BaseFramework/Add/Audio/Onset Detection" )]
		public static void CreateOnsetDetectionSingleton()
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
		
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
			
			// Set the audiosource's clip to the same value as the AudioProcessing objects' clip.
			Target.audio.clip = Target.m_audioClip;
		}
		
		private OnsetDetection Target { get { return (OnsetDetection)target; } }
	}
}