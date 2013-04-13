using UnityEngine;

namespace BaseFramework.UnityWiki
{
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
			set
			{
				m_data = new float[ value.Length ];
				float maxValue = 1.0f;
				for ( int i=0; i<value.Length;i++ )
				{
					m_data[ i ] = value[ i ];
					maxValue = Mathf.Abs( value[ i ] ) > maxValue ? Mathf.Abs( value[ i ] ) : maxValue;
				}
				for ( int i=0; i<m_data.Length; i++ )
					m_data[ i ] /= maxValue;
			}
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