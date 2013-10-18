using UnityEngine;
using System.Collections;

namespace BaseFramework.Core
{
	public static class BaseVectorHelper
	{
		public static Vector3 Clamp( this Vector3 pxSelf, Vector3 pxMin, Vector3 pxMax )
		{
			Vector3 pxResult = Vector3.zero;
			pxResult.x = Mathf.Clamp( pxSelf.x, pxMin.x, pxMax.x );
			pxResult.y = Mathf.Clamp( pxSelf.y, pxMin.y, pxMax.y );
			pxResult.z = Mathf.Clamp( pxSelf.z, pxMin.z, pxMax.z );
			
			return pxResult;
		}
	}
}