using UnityEngine;
using System.Collections;

public static class BaseHelper
{
	public static Transform GetRootParent (Transform childTransform)
	{
		Transform root = childTransform;
		
		while (root.parent != null)
		{
			root = root.parent;
		}
		
		return root;
	}
}
