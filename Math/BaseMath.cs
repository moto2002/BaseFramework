using System;
using UnityEngine;

namespace BaseFramework.Math
{
	public class BaseMath
	{
		public static float[] FastFourierTransform_CooleyTukey( float[] data )
		{
			int N = data.Length; // The total number of samples
			if (N == 1)
			{
				// If we have only one sample, it is our result
				return data;
			}
			
			// Split data set in half:
			float[] half_1 = new float[ N/2 ];
			float[] half_2 = new float[ N/2 ];
			for (int i=0; i<N/2; i++)
			{
				half_1[ i ] = data[ i ];
				half_2[ i ] = data[ i + N / 2 ];
			}
			
			// Recurse.
			half_1 = FastFourierTransform_CooleyTukey( half_1 );
			half_2 = FastFourierTransform_CooleyTukey( half_2 );
			
			// Multiply half_2 by complex number (for reasons)
			for (int k=0; k<N/2; k++)
			{
				half_2[k] *= Mathf.Exp( -2 * Mathf.PI * k / N );
			}
			
			// Define the result array:
			float[] X = new float[ N ];
			
			for (int k=0; k<N/2; k++)
			{
				X[ k ] = half_1[ k ] + half_2[ k ];
				X[ k + N/2 ] = half_1[ k ] - half_2[ k ];
			}
			
			return X;
		}
	}
}