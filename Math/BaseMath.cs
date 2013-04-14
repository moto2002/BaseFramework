using UnityEngine;
using System.Collections;

namespace BaseFramework.Math
{
	public static class BaseMath
	{
		/// <summary>
		/// Calculate the mean average value of a set of real-number data.
		/// </summary>
		/// <returns>
		/// The mean average.
		/// </returns>
		/// <param name='realData'>
		/// A set of real-number floating point data.
		/// </param>
		public static float MeanAverage( float[] realData )
		{
			float mu = 0.0f;
			for (int i=0; i<realData.Length; i++)
				mu += realData[ i ];
			mu /= realData.Length;
			return mu;
		}
		
		/// <summary>
		/// Calculates the Standard Deviation of a set of real-number data.
		/// </summary>
		/// <returns>
		/// The deviation.
		/// </returns>
		/// <param name='realData'>
		/// Real data.
		/// </param>
		public static float StandardDeviation( float[] realData )
		{
			return Mathf.Sqrt( Varience( realData ) );
		}
		
		/// <summary>
		/// Calculates the varience of a set of real-number data.
		/// </summary>
		/// <returns>
		/// The varience.
		/// </returns>
		/// <param name='realData'>
		/// A set of real-number floating point data
		/// </param>
		public static float Varience( float[] realData )
		{
			float mu = MeanAverage( realData );
			float sigma = 0.0f;
			
			for (int i=0; i<realData.Length; i++)
			{
				sigma += Mathf.Pow( realData[ i ] - mu, 2 );
			}
			sigma /= realData.Length - 1;
			
			return sigma;
		}
	}
}