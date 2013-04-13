using UnityEngine;
using UnityEditor;
using System.Collections;

namespace BaseFramework.UnityWiki
{
	/// <summary>
	/// A static class that provides functionality for drawing a Graph object in the editor.
	/// 
	/// Todo List (sorted)
	/// - Scale magnitudes to fit in Rectangle
	/// - Trim magnitudes between Min / Max Y
	/// - Draw the axis
	/// - Improvements to the axis
	/// - Automatic positioning (when supplied with a width/height) would be nice!
	/// </summary>
	[InitializeOnLoad]
	public static class EditorGraph
	{
		private static Material m_lineMaterial;
		static EditorGraph()
		{
			// No idea how to use shaders or materials properly, so we'll just use the one from the reference.
			m_lineMaterial = new Material(
				"Shader \"Lines/Colored Blended\" {" +
				"SubShader { Pass { " +
				"    Blend Off " + // SrcAlpha OneMinusSrcAlpha " +
				"    ZWrite Off  Cull Off  Fog { Mode Off } " +
				"    BindChannels {" +
				"      Bind \"vertex\", vertex Bind \"color\", color }" +
				"} } }"
			);
			
			// Lets not keep any of this hanging around outside the editor.
			m_lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			m_lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		}
		
		public static void DrawGraph( Graph graph, Rect position )
		{
			GUI.Box( position, "Graph" );
			
			// Grab the current channel as C
			Channel[] channels = graph.Channels;
			
			// Check if channels exist
			if( channels == null )
				return;
			
			// Set the material pass.... for reasons.
			m_lineMaterial.SetPass( 0 );
			
			// The x-axis spacing
			float xSpacing = position.width / ( graph.WindowWidth - 1 );
			
			// The y-axis spacing
			float ySpacing = position.height / ( graph.Max_Y - graph.Min_Y );
			
			// The axis' position offsets
			int xOffset = (int)( position.x + 10 );
			int yOffset = (int)( position.y + position.height / ( graph.Max_Y - graph.Min_Y ) ); // - position.height / ( graph.Max_Y - graph.Min_Y ) ) does the opposite duh!
			
			// if min y >= 0, yOffset == position.height
			// if max y <= 0, yOffset == -position.height
			
			// Let's start drawing!
			GL.PushMatrix();
			GL.LoadPixelMatrix();
			GL.Begin( GL.LINES );
			
			// Draw axis!
			GL.Color( Color.cyan );
			
			// x axis
			GL.Vertex3( position.x, yOffset, 0.0f );
			GL.Vertex3( position.x + position.width, yOffset, 0.0f );
			
			// y axis
			GL.Vertex3( xOffset, position.y, 0.0f );
			GL.Vertex3( xOffset, position.y + position.height, 0.0f  );
			
			for( int chan = 0; chan < channels.Length; chan++ )
			{
				Channel C = channels[ chan ];
				
				// Should never happen. Log an error if it does.
				if( C == null )
				{
					Debug.LogError( "Channel [" + chan +"] is of null value." );
					continue;
				}
	 			
				// If this channel is inactive or contains no data we don't want to do jack!
				if( !C.isActive || C.Data == null )
					continue;
	 			
				// This is where we set the colour!
				GL.Color( C.Colour );
				
				// Variables to hold previous positions. The initial positions are equal to the axis offsets.
				int lastX = xOffset;
				int lastY = yOffset;
				
				// Now we begin processing each sample for the current window
				for( int dataIndex = 1; dataIndex < graph.WindowWidth; dataIndex++ )
				{
					// We don't want no out of range exceptions!
					if (dataIndex + graph.X_Offset >= C.Data.Length)
						break;
					
					// Retrieve the amplitude
					float amplitude = C.Data[ graph.X_Offset + dataIndex ];
					
					// Calculate the current sample point
					int xPix = (int)( xOffset + dataIndex * xSpacing );
					int yPix = (int)( yOffset + -1 * Mathf.Clamp( amplitude, graph.Min_Y, graph.Max_Y ) * ySpacing );
					
					// Draw from the last point to the current point.
					GL.Vertex3( lastX, lastY, 0 );
					GL.Vertex3( xPix, yPix, 0 );
					
					// Record current point as the next iterations' last point.
					lastX = xPix;
					lastY = yPix;
				}
			}
			
			GL.End();
			GL.PopMatrix();
		}
	}
}