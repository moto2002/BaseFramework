using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Collections.Generic;

namespace BaseFramework.Utils
{
	public class BaseAssetPathControl
	{
		public static string New( string prefix, string path )
		{
			return New( prefix, path, "" );
		}
		
		public static string New( string prefix, string path, string fileExtension )
		{
			List<string> subDirContentsByName = GetAssetRelativeDirectories( "", fileExtension );
			int foundIndex = subDirContentsByName.FindIndex( x => x.CompareTo( path ) > 0 );
			
			EditorGUILayout.BeginHorizontal();
			if ( !string.IsNullOrEmpty( prefix ) )
			{
				EditorGUILayout.PrefixLabel( prefix );
			}
			int index = foundIndex > 0 ? foundIndex : 0;
			index = EditorGUILayout.Popup( index, subDirContentsByName.ToArray() );
			EditorGUILayout.EndHorizontal();
			
			int directoryIndex = subDirContentsByName[ index ].IndexOf( '.' );
			if ( directoryIndex > -1 )
			{
				path = subDirContentsByName[ index ].Substring( 0, directoryIndex );
			}
			return path;
		}
		
		private static List<string> GetAssetRelativeDirectories( string path, string fileExtension )
		{
			const char subDirSeperator = '/'; //TODO: Windows support??
			
			List<string> directories = new List<string>();
			if ( string.IsNullOrEmpty( path ) )
			{
				directories.Add( "." );
			}
			else
			{
				directories.Add( path + subDirSeperator + '.' );
			}
			
			DirectoryInfo currDirInfo = new DirectoryInfo( Application.dataPath + subDirSeperator + path );
			DirectoryInfo[] subDirInfo = currDirInfo.GetDirectories();
			FileInfo[] fileDirInfo = currDirInfo.GetFiles( fileExtension );
			
			for ( int i=0; i<subDirInfo.Length; i++ )
			{
				if ( string.IsNullOrEmpty( path ) )
				{
					directories.AddRange( GetAssetRelativeDirectories( subDirInfo[ i ].Name, fileExtension ) );
				}
				else
				{
					directories.AddRange( GetAssetRelativeDirectories( path + subDirSeperator + subDirInfo[ i ].Name, fileExtension ) );
				}
			}
			
			for ( int i=0; i<fileDirInfo.Length; i++ )
			{
				directories.Add( path + subDirSeperator + fileDirInfo[ i ].Name );
			}
			
			return directories;
		}
	}
}