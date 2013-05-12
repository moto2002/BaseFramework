using UnityEngine;
using UnityEditor;
using System.Collections;

namespace BaseFramework
{
	[CustomEditor (typeof (Sunlight))]
	public class SunlightEditor : Editor
	{
//		SerializedProperty m_radius;
		
		void OnEnable ()
		{
//			m_radius = serializedObject.FindProperty ("OrbitRadius");
		}
		
		void OnInspecterGUI ()
		{
			//base.OnInspectorGUI ();
			
			serializedObject.Update ();
			//m_radius = 
			serializedObject.ApplyModifiedProperties();
		}
	}
}