using UnityEngine;
using System.Collections;

namespace BaseFramework
{
	public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T m_instance;
		public static T Instance
		{
			get
			{
				if (m_instance == null)
				{
					GameObject go = new GameObject();
					m_instance = go.AddComponent<T>();
					go.name = "Singleton."+typeof(T).ToString();
				}
				
				return m_instance;
			}
		}
	}
}