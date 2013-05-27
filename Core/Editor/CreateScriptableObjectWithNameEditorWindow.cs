using UnityEngine;
using UnityEditor;

using System;
using System.Reflection;

namespace BaseFramework.Utils
{
	public class CreateScriptableObjectWithNameEditorWindow : EditorWindow
	{
		public static void SetType( Type t )
		{
			if ( t.IsClass && !t.IsAbstract && t.IsSubclassOf( typeof( ScriptableObject ) ) )
			{
				m_scriptableObjectType = t;
			}
		}
		
		public void OnGUI()
		{
			EditorGUILayout.LabelField( "Create Scriptable Object" );
			if ( m_scriptableObjectType == null )
			{
				EditorGUILayout.LabelField( "No ScriptableObject type to create!" );
				return;
			}
			
			
//			m_info = EditorGUILayout.PropertyField( m_info );
			
			m_assetName = EditorGUILayout.TextField( "Asset Name", m_assetName );
			m_assetPath = EditorGUILayout.TextField( "Asset Path", m_assetPath );
			
			bool OK = GUILayout.Button( "Create" );
			
			if ( OK &&
				!string.IsNullOrEmpty( m_assetName ) && 
				!string.IsNullOrEmpty( m_assetPath ) )
			{
				ScriptableObject obj = ScriptableObject.CreateInstance( m_scriptableObjectType ) as ScriptableObject;
				AssetDatabase.CreateAsset( obj, m_assetPath + m_assetName + ".asset" );
				Close();
			}
		}
		
		private static Type m_scriptableObjectType;
		private string m_assetName;
		private string m_assetPath;
//		private SerializedDirectoryInfo m_info; // TODO: Directory Drop down property
	}
}