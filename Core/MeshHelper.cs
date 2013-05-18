using UnityEngine;
using System.Collections;

namespace BaseFramework.Core
{
	public static class MeshHelper
	{
		public static Mesh JoinMeshes( Mesh first, Mesh second ) // todo : make extensdion method
		{
			int newVertLength = first.vertices.Length + second.vertices.Length;
			int newTriLength  = first.triangles.Length + second.triangles.Length;
			
			Vector3[] newVerts = new Vector3[ newVertLength ];
			Vector2[] newUVs = new Vector2[ newVertLength ];
			int[] newTris = new int[ newTriLength ];
			
			for ( int v=0; v<newVertLength; v++ )
			{
				if ( v == second.vertices.Length ) break;
				newVerts[ v ] = v >= first.vertices.Length ? second.vertices[ v ] : first.vertices[ v ];
				newUVs[ v ] = v >= first.vertices.Length ? second.uv[ v ] : first.uv[ v ];
			}
			
			for ( int t=0; t<newTriLength; t++ )
			{
				if ( t == second.triangles.Length ) break;
				newTris[ t ] = t >= first.triangles.Length ? second.triangles[ t ] : first.triangles[ t ];
			}
			
			Mesh newMesh = new Mesh();
			newMesh.vertices = newVerts;
			newMesh.uv = newUVs;
			newMesh.triangles = newTris;
			newMesh.RecalculateNormals();
			newMesh.RecalculateBounds();
			return newMesh;
		}
	}
}