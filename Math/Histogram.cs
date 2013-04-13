using UnityEngine;
using System.Collections;

namespace BaseFramework.Math
{
	public class Histogram
	{
		public Histogram()
		{
			
		}
		
		public void SetData( int[] data )
		{
			int nBins = data.Length;
			m_bins = new int[ nBins ];
			for (int i=0; i<nBins; i++)
			{
				m_bins[ data[ i ] ]++; 
			}
		}
		
		public void SetData( float data )
		{
		}
		
		public float MeanValue()
		{
			// todo !
			return 0.0f;
		}
		
		public float Varience()
		{
			// todo !
			return 0.0f;
		}
		
		public float StandardDeviation()
		{
			// todo !
			return 0.0f;
		}
		
		private int[] m_bins;
		private Axis m_xAxis, m_yAxis; 
	}
	
	public class Axis
	{
		public string Label;
		public int MinimumValue;
		public int MaximumValue;
		//public float IncrementSize;
	}
}