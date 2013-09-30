using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace BaseFramework.EditorUtils
{
	public abstract class BaseEditorWindow : EditorWindow
	{
		public BaseEditorView[] Subviews
		{
			get
			{
				return m_subviews.ToArray();
			}
		}
		
		public abstract void LoadView();
		
		public void AddSubview( BaseEditorView xView )
		{
			xView.SourceWindow = this;
			m_subviews.Add( xView );
		}
		
		private void OnEnable()
		{
			m_subviews = new List<BaseEditorView>();
			LoadView();
		}
		
		private void OnGUI()
		{
			foreach ( BaseEditorView xView in m_subviews )
			{
				GUI.BeginGroup( xView.ViewBounds );
				{
					xView.Draw();
				}
				GUI.EndGroup();
			}
		}
		
		private void Update()
		{
			foreach ( BaseEditorView xView in m_subviews )
			{
				xView.Update();
			}
		}
		
		private List<BaseEditorView> m_subviews;
	}
}