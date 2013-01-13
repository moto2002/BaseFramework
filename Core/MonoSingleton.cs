using UnityEngine;
using System.Collections;

namespace BaseFramework
{
	[ExecuteInEditMode ()]
	public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T m_instance;
		public static T Instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = (FindObjectOfType (typeof (T)) as T);
					
					if (m_instance == null)
					{
						GameObject go = new GameObject();
						go.name = "Singleton."+typeof(T).ToString();
						m_instance = go.AddComponent<T>();
					}
					
					if (m_instance == null)
					{
						Debug.LogError ("Singleton Instance did not instantiate properly.");
					}
				}
				
				return m_instance;
			}
		}
		
		protected virtual void OnEnable()
		{
			name = "Singleton."+typeof(T).ToString();
		}
	}
}