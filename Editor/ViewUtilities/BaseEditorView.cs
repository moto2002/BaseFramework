using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace BaseFramework.EditorUtils
{
	public abstract class BaseEditorView
	{
		public BaseEditorWindow SourceWindow
		{
			get { return m_sourceWindow;  }
			set { m_sourceWindow = value; }
		}
		
		public BaseEditorView Superview
		{
			get { return m_superview;  }
			set { m_superview = value; }
		}
		
		public List<BaseEditorView> Subviews
		{
			get { return m_subviews; }
		}
		
		public Vector2 Centre
		{
			get { return ViewBounds.center;    }
			set { m_viewBounds.center = value; }
		}
		
		public Rect ViewBounds
		{
			get { return m_viewBounds;  }
			set { m_viewBounds = value; }
		}
		
		public BaseEditorView( BaseEditorWindow xSourceWindow )
		{
			m_subviews = new List<BaseEditorView>();
			m_sourceWindow = xSourceWindow;
		}
		
		public virtual void Draw()
		{
			foreach ( BaseEditorView xSubview in m_subviews )
			{
				GUI.BeginGroup( xSubview.ViewBounds );
				{
					xSubview.Draw();
				}
				GUI.EndGroup();
			}
		}
		
		public virtual void Update()
		{
		}
		
		public void AddSubview( BaseEditorView xView )
		{
			xView.RemoveFromSuperview();
			
			m_subviews.Add( xView );
			xView.Superview = this;
		}
		
		public void RemoveAllSubviews()
		{
			m_subviews = new List<BaseEditorView>();
		}
		
		public void RemoveFromSuperview()
		{
			if ( m_superview != null )
			{
				m_superview.Subviews.Remove( this );
			}
		}
		
		private Rect m_viewBounds;
		private List<BaseEditorView> m_subviews;
		
		private BaseEditorView m_superview;
		private BaseEditorWindow m_sourceWindow;
	}
}