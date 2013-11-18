using UnityEngine;
using UnityEditor;

using System;
using System.Reflection;

namespace BaseFramework.EditorUtils
{
	public class CreateScriptableObjectWithNameEditorWindow : EditorWindow
	{
		public static void SetType( Type t )
		{
			if ( t.IsClass && !t.IsAbstract && t.IsSubclassOf( typeof( ScriptableObject ) ) )
			{
				m_pxScriptableObjectType = t;
			}
		}
		
		public void OnGUI()
		{
			EditorGUILayout.LabelField( "Create Scriptable Object" );
			if ( m_pxScriptableObjectType == null )
			{
				EditorGUILayout.LabelField( "No ScriptableObject type to create!" );
				return;
			}
			
			//TODO: Create a "Make Another" checkbox.
			
			m_pcAssetName = EditorGUILayout.TextField( "Asset Name", m_pcAssetName );
			m_pcAssetPath = BaseAssetPathControl.New( "Asset Path", m_pcAssetPath );
			
			bool OK = GUILayout.Button( "Create" );
			
			if ( OK && !string.IsNullOrEmpty( m_pcAssetName ) )
			{
				ScriptableObject obj = ScriptableObject.CreateInstance( m_pxScriptableObjectType ) as ScriptableObject;
				AssetDatabase.CreateAsset( obj, "Assets/" + m_pcAssetPath + m_pcAssetName + ".asset" );
				Close();
			}
		}
		
		private static Type m_pxScriptableObjectType;
		private string m_pcAssetName;
		private string m_pcAssetPath;
	}
}