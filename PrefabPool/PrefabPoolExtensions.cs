using UnityEngine;
using System.Collections;

namespace BaseFramework
{
	public static class PrefabPoolExtensions
	{
		public static void ReturnToPool (this GameObject go)
		{
			go.SendMessageUpwards ("PoolObject", go, SendMessageOptions.DontRequireReceiver);
		}
		
		public static void IsInPool (this GameObject go)
		{
		}
	}
}