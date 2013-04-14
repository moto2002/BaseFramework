using UnityEngine;

namespace BaseFramework.Math
{
	/// <summary>
	/// A Channel that holds data for a Graph object.
	/// 
	/// TODO : Improvements to the Data format such that it can contain an X, Y, and Z value.
	/// 	   This would require modifications to the EditorGraph.DrawGraph function also.
	/// </summary>
	public class Channel
	{
		public bool isActive = false;
		
		public Channel() : this( Color.red, null ) { }
		public Channel( Color colour ) : this( colour, null ) { }
		public Channel( Color colour, float[] data )
		{
			m_data = data;
			m_colour = colour;
			isActive = true;
		}
		
		public float[] Data
		{
			get { return m_data; }
			set { m_data = value; }
		}
		
		public Color Colour
		{
			get { return m_colour; }
			set { m_colour = value; }
		}
		
		private float[] m_data;
		private Color m_colour = Color.white;
	}
}