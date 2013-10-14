using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace BaseFramework.EditorUtils
{
	public abstract class BaseEditorView
	{
		public BaseEditorWindow SourceWindow
		{
			get { return m_pxSourceWindow;  }
			set { m_pxSourceWindow = value; }
		}
		
		public BaseEditorView Superview
		{
			get { return m_pxSuperview;  }
			set { m_pxSuperview = value; }
		}
		
		public List<BaseEditorView> Subviews
		{
			get { return m_pxSubviews; }
		}
		
		public Vector2 Centre
		{
			get { return ViewBounds.center;    }
			set { m_xViewBounds.center = value; }
		}
		
		public Rect ViewBounds
		{
			get { return m_xViewBounds;  }
			set { m_xViewBounds = value; }
		}
		
		public BaseEditorView( BaseEditorWindow xSourceWindow )
		{
			m_pxSubviews = new List<BaseEditorView>();
			m_pxSourceWindow = xSourceWindow;
		}
		
		public virtual void Draw()
		{
			foreach ( BaseEditorView xSubview in m_pxSubviews )
			{
				GUI.BeginGroup( ViewBounds );
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
			
			m_pxSubviews.Add( xView );
			xView.Superview = this;
		}
		
		public void RemoveAllSubviews()
		{
			m_pxSubviews = new List<BaseEditorView>();
		}
		
		public void RemoveFromSuperview()
		{
			if ( m_pxSuperview != null )
			{
				m_pxSuperview.Subviews.Remove( this );
			}
		}
		
		private Rect m_xViewBounds;
		private List<BaseEditorView> m_pxSubviews;
		
		private BaseEditorView m_pxSuperview;
		private BaseEditorWindow m_pxSourceWindow;
	}
}