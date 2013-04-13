using UnityEngine;

namespace BaseFramework.UnityWiki
{
	public class Graph 
	{
		public Graph() : this( 3, 1024, -1, 1) { }
		private Graph( int nChannels, int windowSize, int yMin, int yMax )
		{
			m_xPos = 0;
			m_windowSize = windowSize;
			Min_Y = m_yMin = yMin;
			Max_Y = m_yMax = yMax;
			
			m_channels = new Channel[ nChannels ]; // todo : Support for nChannels != 3
			m_channels[ 0 ] = new Channel( Color.red );
			m_channels[ 1 ] = new Channel( Color.green );
			m_channels[ 2 ] = new Channel( Color.blue );
		}
		
		public Channel[] Channels
		{
			get { return m_channels; } // todo : Support for adding / removing channels on the fly
		}
		
		public int WindowWidth
		{
			get { return m_windowSize; }
			set { m_windowSize = value; }
		}
		
		public int X_Offset
		{
			get { return m_xPos; }
			set { m_xPos = value; }
		}
		
		public float Max_Y;
		/*
		{
			get { return m_yMax; }
			set { m_yMax = Mathf.Max( value, m_yMin ); }
		}
		//*/
		
		public float Min_Y;
		/*
		{
			get { return m_yMin; }
			set { m_yMin = Mathf.Min( value, m_yMax ); }
		}
		//*/
		
		private int m_xPos;
		private int m_windowSize;
		private float m_yMin;
		private float m_yMax;
		private Channel[] m_channels;
	}
}