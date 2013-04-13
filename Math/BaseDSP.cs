using System;
using UnityEngine;

namespace BaseFramework.Math
{
	public static class BaseDSP
	{
		public static ComplexNumber[] FastFourierTransform( float[] realData )
		{
			ComplexNumber[] x = new ComplexNumber[ realData.Length ];
			for (int i=0; i<x.Length; i++)
			{
				x[i] = new ComplexNumber( realData[ i ] );
			}
			return FastFourierTransform( x );
		}
		
		public static ComplexNumber[] FastFourierTransform( ComplexNumber[] x )
		{
			int N = x.Length; // The total number of samples
			if (N == 1)
			{
				// If we have only one sample, it is our result
				return x;
			}
			
			// Split data set in half:
			ComplexNumber[] E = new ComplexNumber[ N/2 ];
			ComplexNumber[] D = new ComplexNumber[ N/2 ];
			for (int i=0; i<N/2; i++)
			{
				//E[ i ] = x[ i ];
				//D[ i ] = x[ i + N / 2 ];
				E[ i ] = x[ 2 * i ];
				D[ i ] = x[ 2 * i + 1 ];
			}
			
			// Recurse.
			E = FastFourierTransform( E );
			D = FastFourierTransform( D );
			
			// Multiply D by complex number (for reasons)
			for (int k=0; k<N/2; k++)
			{
				ComplexNumber temp = ComplexNumber.FromPolar( 1, -2 * Mathf.PI * k / N );
				D[k] *= temp;
			}
			
			// Define the result array:
			ComplexNumber[] X = new ComplexNumber[ N ];
			
			// Add Complex Numbers
			for (int k=0; k<N/2; k++)
			{
				X[ k ] = E[ k ] + D[ k ];
				X[ k + N/2 ] = E[ k ] - D[ k ];
			}
			
			// Return result
			return X;
		}
		
		/// <summary>
		/// Returns the signal to noise ratio of a siganl from provided samples..
		/// </summary>
		/// <returns>
		/// The to noise ratio.
		/// </returns>
		/// <param name='sampleData'>
		/// Data.
		/// </param>
		public static float SignalToNoiseRatio( float[] sampleData )
		{
			return BaseMath.MeanAverage( sampleData ) / BaseMath.StandardDeviation( sampleData );
		}
		
		/// <summary>
		/// The typical error of a signal as calculated from provided samples.
		/// </summary>
		/// <returns>
		/// The error.
		/// </returns>
		/// <param name='sampleData'>
		/// Data.
		/// </param>
		public static float TypicalError( float[] sampleData )
		{
			return BaseMath.StandardDeviation( sampleData ) / Mathf.Pow( sampleData.Length, 0.5f );
		}
	}
}