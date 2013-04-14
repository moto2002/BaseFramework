using UnityEngine;
using UnityEditor;

namespace BaseFramework.Math
{
	/// <summary>
	/// A static class that provides functionality for drawing a Graph object in the editor.
	/// 
	/// TODO - OpenGL Draw code optimizations over large windows!
	/// TODO - Support for a more generic x-axis
	/// TODO - Draw the axis a bit nicer (with values etc)
	/// TODO - Allow for negative Max_Y values and positive Min_Y values! Requires minor changes to the Graph class.
	/// TODO - Automatic positioning (when supplied with a width/height) would be nice!
	/// </summary>
	[InitializeOnLoad]
	public static class EditorGraph
	{
		private static Material m_lineMaterial;
		static EditorGraph()
		{
			// No idea how to use shaders or materials properly, so we'll just use the one from the unity reference site.
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
			
			// Check if channels exist
			if( graph.Channels == null )
				return;
			
			// Set the material pass.... for reasons.
			m_lineMaterial.SetPass( 0 );
			
			// The x-axis spacing
			float xSpacing = position.width / ( graph.WindowWidth - 1 );
		
			// The y-axis spacing
			float ySpacing = position.height / ( graph.Max_Y - graph.Min_Y );
			
			// The x-axis position offset
			int xOffset = (int)( position.x );
			
			// The y-axis position offset
			float ratio = Mathf.Clamp01( graph.Max_Y / ( graph.Max_Y - graph.Min_Y ) );
			int yOffset = (int)( position.y + (position.height * ratio) );
			
			// Let's start drawing!
			GL.PushMatrix();
			GL.LoadPixelMatrix();
			GL.Begin( GL.LINES );
			
			// Draw the axis!
			GL.Color( Color.cyan );
			
			// x axis
			GL.Vertex3( position.x, yOffset, 0.0f );
			GL.Vertex3( position.x + position.width, yOffset, 0.0f );
			
			// y axis
			GL.Vertex3( xOffset, position.y, 0.0f );
			GL.Vertex3( xOffset, position.y + position.height, 0.0f  );
			
			// Examine all channels in the graph
			for( int channelIndex = 0; channelIndex < graph.Channels.Length; channelIndex++ )
			{
				// Grab the current channel as C
				Channel C = graph.Channels[ channelIndex ];
				
				// Should never happen. Log an error if it does.
				if( C == null )
				{
					Debug.LogError( "Channel [" + channelIndex +"] is of null value." );
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
				for( int dataIndex = 0; dataIndex < graph.WindowWidth; dataIndex++ )
				{
					// We don't want no out of range exceptions!
					if (dataIndex + graph.X_Offset > C.Data.Length - 1)
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