using UnityEngine;
using System.Collections.Generic;

public class TrackedMono<T> : MonoBehaviour where T : MonoBehaviour
{
	private static HashSet<T> m_instances = new HashSet<T>();
	
	public HashSet<T> Instances
	{
		get { return m_instances; }
	}
	
	protected virtual void OnEnable()
	{
		T component = this as T;
		if ( component != null )
		{
			m_instances.Add( component );
		}
	}

	protected virtual void OnDisable()
	{
		T component = this as T;
		if ( component != null )
		{
			m_instances.Remove( component );
		}
	}
}
