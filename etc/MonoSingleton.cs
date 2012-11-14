using UnityEngine;
using System.Collections;

namespace GenericLib
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
					go.name = "singleton";
				}
				
				return m_instance;
			}
		}
	}
}