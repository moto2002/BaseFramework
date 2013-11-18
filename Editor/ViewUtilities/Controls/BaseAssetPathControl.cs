using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Collections.Generic;

namespace BaseFramework.EditorUtils
{
	public class BaseAssetPathControl
	{
		public static string New( string pcPrefix, string pcPath )
		{
			return New( pcPrefix, pcPath, "" );
		}
		
		public static string New( string pcPrefix, string pcPath, string pcFileExtension )
		{
			List<string> pxRootDirectoryContentsByName = GetAssetRelativeDirectories( "", pcFileExtension );
			int iDirectoryIndex = pxRootDirectoryContentsByName.FindIndex( x => x.CompareTo( pcPath ) > 0 );
			
			EditorGUILayout.BeginHorizontal();
			
			bool bHasFieldNameLabel = !string.IsNullOrEmpty( pcPrefix );
			if ( bHasFieldNameLabel )
			{
				EditorGUILayout.PrefixLabel( pcPrefix );
			}
			int iValidatedDirectoryIndex = iDirectoryIndex > 0 ? iDirectoryIndex : 0;
			iValidatedDirectoryIndex = EditorGUILayout.Popup( iValidatedDirectoryIndex, pxRootDirectoryContentsByName.ToArray() );
			
			EditorGUILayout.EndHorizontal();
			
			int directoryIndex = pxRootDirectoryContentsByName[ iValidatedDirectoryIndex ].IndexOf( '.' );
			if ( directoryIndex > -1 )
			{
				pcPath = pxRootDirectoryContentsByName[ iValidatedDirectoryIndex ].Substring( 0, directoryIndex );
			}
			return pcPath;
		}
		
		private static List<string> GetAssetRelativeDirectories( string pcPath, string pcFileExtension )
		{
			const char cSubDirSeperator = '/';
			
			bool bDirectoryExists = string.IsNullOrEmpty( pcPath );
			List<string> pxDirectories = new List<string>();
			if ( bDirectoryExists )
			{
				pxDirectories.Add( "." );
			}
			else
			{
				pxDirectories.Add( pcPath + cSubDirSeperator + '.' );
			}
			
			string pcCurrentPath = Application.dataPath + cSubDirSeperator + pcPath;
			DirectoryInfo pxDirectoryInfo        = new DirectoryInfo( pcCurrentPath );
			DirectoryInfo[] pxSubdirectoryInfo   = pxDirectoryInfo.GetDirectories();
			FileInfo[] pxSubdirectoryContentInfo = pxDirectoryInfo.GetFiles( pcFileExtension );
			
			for ( int i=0; i<pxSubdirectoryInfo.Length; i++ )
			{
				if ( bDirectoryExists )
				{
					List<string> pcSubdirectoryContents = GetAssetRelativeDirectories( pxSubdirectoryInfo[ i ].Name, pcFileExtension );
					pxDirectories.AddRange( pcSubdirectoryContents );
				}
				else
				{
					pxDirectories.AddRange( GetAssetRelativeDirectories( pcPath + cSubDirSeperator + pxSubdirectoryInfo[ i ].Name, pcFileExtension ) );
				}
			}
			
			for ( int i=0; i<pxSubdirectoryContentInfo.Length; i++ )
			{
				pxDirectories.Add( pcPath + cSubDirSeperator + pxSubdirectoryContentInfo[ i ].Name );
			}
			
			return pxDirectories;
		}
	}
}