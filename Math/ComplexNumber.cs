using UnityEngine;

namespace BaseFramework.Math
{
	public class ComplexNumber
	{
		#region Operator Overloads
		
		public static ComplexNumber operator+( ComplexNumber a, ComplexNumber b )
		{
			return new ComplexNumber(
				a.m_real + b.m_real,
				a.m_imaginary + b.m_imaginary
			);
		}
		
		public static ComplexNumber operator-( ComplexNumber a, ComplexNumber b )
		{
			return new ComplexNumber(
				a.m_real - b.m_real,
				a.m_imaginary - b.m_imaginary
			);
		}
		
		public static ComplexNumber operator*( ComplexNumber a, ComplexNumber b )
		{
			return new ComplexNumber(
				a.m_real * b.m_real - a.m_imaginary * b.m_imaginary,
				a.m_real * b.m_imaginary + b.m_real * a.m_imaginary
			);
		}
		
		public static ComplexNumber operator/( ComplexNumber a, ComplexNumber b )
		{
			float denominator = b.m_real * b.m_real + b.m_imaginary * b.m_imaginary;
			return new ComplexNumber(
				( a.m_real * b.m_real + a.m_imaginary * b.m_imaginary ) / denominator,
				( a.m_imaginary * b.m_real - a.m_real * b.m_imaginary ) / denominator
			);
		}
		
		#endregion
		
		#region Properties
		
		public float RealNumber
		{
			get { return m_real; }
			set { m_real = value; }
		}
		
		public float ImaginaryScale
		{
			get { return m_imaginary; }
			set { m_imaginary = value; }
		}
		
		public float Magnitude
		{
			get
			{
				return Mathf.Sqrt( m_real * m_real + m_imaginary * m_imaginary );
			}
		}
		
		public float Phase
		{
			get
			{
				if ( m_real != 0 )
				{
					return Mathf.Atan( m_imaginary / m_real );
				}
				else if ( m_imaginary > 0 )
				{
					return 90;
				}
				else
				{
					return -90;
				}
			}
		}
		
		#endregion
		
		#region Constructors
		
		public ComplexNumber() : this( 0.0f, 0.0f )
		{
		}
		
		public ComplexNumber( float realValue ) : this( realValue, 0.0f )
		{
		}
		
		public ComplexNumber( float realValue, float imaginaryValue )
		{
			m_real = realValue;
			m_imaginary = imaginaryValue;
		}
		
		#endregion
		
		private float m_real;
		private float m_imaginary;
	}
}