using UnityEngine;

namespace BaseFramework.Math
{
	/// <summary>
	/// Graph data structure.
	/// 
	/// TODO : Auto yMin / yMax scale adjustment
	/// TODO : Support for nChannels != 3
	/// TODO : Support for adding / removing channels on the fly
	/// TODO : Allow for positive Min_Y values and negative Max_Y values! Will require changes to the EditorGraph Class.
	/// </summary>
	public class Graph
	{
		public Graph() : this( 3, 1024, -1, 1) { }
		private Graph( int nChannels, int windowSize, int yMin, int yMax )
		{
			m_xPos = 0;
			m_windowSize = windowSize;
			m_yMin = yMin;
			m_yMax = yMax;
			
			m_channels = new Channel[ nChannels ];
			m_channels[ 0 ] = new Channel( Color.red );
			m_channels[ 1 ] = new Channel( Color.green );
			m_channels[ 2 ] = new Channel( Color.blue );
		}
		
		public Channel[] Channels
		{
			get { return m_channels; }
		}
		
		public int WindowWidth
		{
			get { return m_windowSize; }
			set { m_windowSize = value; }
		}
		
		public int X_Offset
		{
			get { return m_xPos; }
			set { m_xPos = Mathf.Abs( value ); }
		}
		
		public float Max_Y
		{
			get { return m_yMax; }
			set { m_yMax = Mathf.Max( value, 0 ); }
			//set { m_yMax = Mathf.Max( value, m_yMin ); }
		}
		
		public float Min_Y
		{
			get { return m_yMin; }
			set { m_yMin = Mathf.Min( value, 0 ); }
			//set { m_yMin = Mathf.Min( value, m_yMax ); }
		}
		
		private int m_xPos;
		private int m_windowSize;
		private float m_yMin;
		private float m_yMax;
		private Channel[] m_channels;
	}
}