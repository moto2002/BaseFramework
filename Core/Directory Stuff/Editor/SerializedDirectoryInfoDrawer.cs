using UnityEngine;
using UnityEditor;

namespace BaseFramework.Utils
{
	[CustomPropertyDrawer( typeof( SerializedDirectoryInfoDrawer ) )]
	public class SerializedDirectoryInfoDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight (property, label);
		}
		
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			base.OnGUI (position, property, label);
		}
	}
}