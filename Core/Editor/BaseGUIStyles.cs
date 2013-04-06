using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class BaseGUIStyles
{
	public static GUIStyle Default;
	public static GUIStyle Header;
	
	static BaseGUIStyles()
	{
		Default = new GUIStyle();
		Default.padding = new RectOffset( 5, 5, 5, 5 );
		Default.richText = true;
		
		Header = new GUIStyle( Default );
		Header.fontStyle = FontStyle.Bold;
		Header.fontSize = 24;
		Header.padding.bottom += 5;
	}
}
