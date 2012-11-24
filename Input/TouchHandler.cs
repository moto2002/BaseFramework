using UnityEngine;
using System.Collections;

namespace GenericLib
{
	public abstract class TouchHandler : MonoBehaviour
	{
		private TouchGestures m_input;
		
		void Awake ()
		{
			m_input = TouchGestures.Instance;
			
			m_input.TouchBegan += TouchStart;
			m_input.TouchChanged += TouchUpdate;
			m_input.TouchEnded += TouchEnd;
		}
		
		void OnDestroy ()
		{
			m_input.TouchBegan -= TouchStart;
			m_input.TouchChanged -= TouchUpdate;
			m_input.TouchEnded -= TouchEnd;
		}
		
		public abstract void TouchStart (TouchGestures.Finger f);
		public abstract void TouchUpdate (TouchGestures.Finger f);
		public abstract void TouchEnd (TouchGestures.Finger f);
	}
}