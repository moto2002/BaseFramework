using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace BaseFramework.EditorUtils
{
	public class ScriptableObjectUtility : MonoBehaviour
	{
		/// <summary>
		//	This makes it easy to create, name and place unique new ScriptableObject asset files.
		/// </summary>
		public static void CreateAssetAtPath( ScriptableObject pxObject, string pxPath, string pxFilename )
		{
			string pxFullPath = "";
			
			string pxAssetPath = "Assets";
			string pxExtension = ".asset";
			
			bool bPathSupplied = !string.IsNullOrEmpty( pxPath );
			bool bFilenameSupplied  = !string.IsNullOrEmpty( pxFilename );
			
			if ( bPathSupplied )
			{
				List<string> pxPathComponents = new List<string>();
				
				bool bAssetPathSupplied = pxPath.StartsWith( pxAssetPath );
				if ( !bAssetPathSupplied )
				{
					pxPathComponents.Add( pxAssetPath );
					pxPathComponents.Add( "/" );
				}
				
				pxPathComponents.Add( pxPath );
				
				bool bPathHasTrailingSlash = pxPath.EndsWith( "/" );
				if ( !bPathHasTrailingSlash )
				{
					pxPathComponents.Add( "/" );
				}
				
				if ( bFilenameSupplied )
				{
					pxPathComponents.Add( pxFilename );
					
					bool bExtensionSupplied = pxFilename.EndsWith( pxExtension );
					if ( !bExtensionSupplied )
					{
						pxPathComponents.Add( pxExtension );
					}
				}
				else
				{
					pxPathComponents.Add( pxObject.name );
					pxPathComponents.Add( pxExtension );
				}
				
				string[] pxComponentArray = pxPathComponents.ToArray();
				pxFullPath = string.Concat( pxComponentArray );
			} 
			else
			{
				string pxObjectTypeName = pxObject.GetType().Name;
				pxFullPath = pxAssetPath + "/" + pxObjectTypeName + pxExtension;
			}
	 		
			AssetDatabase.CreateAsset( pxObject, pxFullPath );
			AssetDatabase.SaveAssets();
			
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = pxObject;
		}
		
		public static void CreateAssetAtRootDirectory( ScriptableObject pxObject )
		{
			CreateAssetAtPath( pxObject, null, null );
		}
	}
}
