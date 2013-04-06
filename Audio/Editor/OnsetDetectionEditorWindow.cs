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
		
		private void OnGUI()
		{
			Debug.Log( "OnGUI()" );
		}
	}
}