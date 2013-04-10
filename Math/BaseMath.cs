using System;
using UnityEngine;

namespace BaseFramework.Math
{
	public class BaseMath
	{
		public static ComplexNumber[] FastFourierTransform_CooleyTukey( float[] realData )
		{
			ComplexNumber[] x = new ComplexNumber[ realData.Length ];
			for (int i=0; i<x.Length; i++)
			{
				x[i] = new ComplexNumber( realData[ i ] );
			}
			return FastFourierTransform_CooleyTukey( x );
		}
		
		public static ComplexNumber[] FastFourierTransform_CooleyTukey( ComplexNumber[] x )
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
			E = FastFourierTransform_CooleyTukey( E );
			D = FastFourierTransform_CooleyTukey( D );
			
			// Multiply half_2 by complex number (for reasons)
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
	}
}