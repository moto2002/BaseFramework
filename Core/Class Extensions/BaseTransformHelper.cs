using UnityEngine;
using System.Collections;

namespace BaseFramework.Core
{
	public static class BaseTransformHelper
	{
		public static Transform GetRootParent( Transform childTransform ) // todo : make extensdion method
		{
			Transform root = childTransform;
			
			while (root.parent != null)
			{
				root = root.parent;
			}
			
			return root;
		}
	}
}