using UnityEngine;
using System.Collections;

namespace BaseFramework.PrefabPool
{
	public static class PrefabPoolExtensions
	{
		public static void ReturnToPool( this GameObject pxGameObject )
		{
			// To consider- Base class functionality..?
			pxGameObject.SendMessageUpwards( "ReturnObjectToPool", pxGameObject );
		}
	}
}