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
				return m_pxSubviews.ToArray();
			}
		}
		
		public abstract void LoadView();
		
		public void AddSubview( BaseEditorView xView )
		{
			xView.SourceWindow = this;
			m_pxSubviews.Add( xView );
		}
		
		private void OnEnable()
		{
			m_pxSubviews = new List<BaseEditorView>();
			LoadView();
		}
		
		private void OnGUI()
		{
			// TODO: Resize Subviews when window is resized!!
			foreach ( BaseEditorView xView in m_pxSubviews )
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
			foreach ( BaseEditorView xView in m_pxSubviews )
			{
				xView.Update();
			}
		}
		
		private List<BaseEditorView> m_pxSubviews;
	}
}