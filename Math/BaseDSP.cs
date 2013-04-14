using System;
using UnityEngine;

namespace BaseFramework.Math
{
	public static class BaseDSP
	{
		#region Utility Functions
		
		/// <summary>
		/// Returns the signal to noise ratio of a signal from provided samples..
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
		/// The typical error is defined as ?
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
		
		/// <summary>
		/// Convolute the floating point arrays x and h, using the Input Side Algorithm.
		/// </summary>
		/// <returns>
		/// The convoluted product of x and h.
		/// </returns>
		/// <param name='x'>
		/// The input signal, X.
		/// </param>
		/// <param name='h'>
		/// The impulse response, H.
		/// </param>
		public static float[] ConvolutionSum_InputSide( float[] x, float[] h )
		{
			float[] y = new float[ x.Length + h.Length ];
			for (int i=0; i<x.Length; i++)
			{
				for (int j=0; j<h.Length; j++)
				{
					y[ i + j ] += x[ i ] * h[ j ];
				}
			}
			return y;
		}
		
		/// <summary>
		/// Convolute the floating point arrays x and h, using the Output Side Algorithm.
		/// </summary>
		/// <returns>
		/// The convoluted product of x and h.
		/// </returns>
		/// <param name='x'>
		/// The input signal, X.
		/// </param>
		/// <param name='h'>
		/// The impulse response, H.
		/// </param>
		public static float[] ConvolutionSum_OutputSide( float[] x, float[] h )
		{
			int N = x.Length + h.Length;
			float[] y = new float[ N ];
			for (int i=0; i<N; i++)
			{
				for (int j=0; j<h.Length; j++)
				{
					if ( i-j >= 0 && i-j < x.Length)
						y[ i ] += h[ j ] * x[ i - j ];
				}
			}
			return y;
		}
		
		#endregion
		
		#region Window Functions
		
//		public static HammingWindow
		
		#endregion
		
		#region Decomposition Methods
		
		/// <summary>
		/// Decomposes a signal x into its corresponding even symmetry.
		/// An even symmetry is defined as an N point signal that forms a mirror image of itself at N/2.
		/// </summary>
		/// <returns>
		/// The even symmetry decomposition of signal x.
		/// </returns>
		/// <param name='x'>
		/// A signal, x.
		/// </param>
		public static float[] EvenDecomposition( float[] x )
		{
			int N = x.Length;
			float[] E = new float[ N ];
			for (int n = 0; n < N; n++)
			{
				E[ n ] = ( x[ n ] + x[ N - n ] ) / 2;
			}
			return E;
		}
		
		/// <summary>
		/// Decomposes a signal x into its corresponding odd symetry form.
		/// An odd symmetry is defined as an N point signal that forms a mirror image of opposite magnitudes at N/2.
		/// </summary>
		/// <returns>
		/// The odd symmetry decomposition of signal x.
		/// </returns>
		/// <param name='x'>
		/// A signal, x.
		/// </param>
		public static float[] OddDecomposition( float[] x )
		{
			int N = x.Length;
			float[] O = new float[ N ];
			for (int n = 0; n < N; n++)
			{
				O[ n ] = ( x[ n ] - x[ N - n ] ) / 2;
			}
			return O;
		}
		
		/// <summary>
		/// Fasts the fourier transform.
		/// </summary>
		/// <returns>
		/// The fourier transform.
		/// </returns>
		/// <param name='realData'>
		/// Real data.
		/// </param>
		public static ComplexNumber[] FastFourierTransform( float[] realData )
		{
			ComplexNumber[] x = new ComplexNumber[ realData.Length ];
			for (int i=0; i<x.Length; i++)
			{
				x[i] = new ComplexNumber( realData[ i ] );
			}
			return FastFourierTransform( x );
		}
		
		/// <summary>
		/// Fasts the fourier transform.
		/// </summary>
		/// <returns>
		/// The fourier transform.
		/// </returns>
		/// <param name='x'>
		/// X.
		/// </param>
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
		
		#endregion
	}
}