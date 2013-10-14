using UnityEngine;
using UnityEditor;

using BaseFramework.EditorUtils;

namespace BaseFramework.AI
{
	public class BehaviourTreeEditorWindow : BaseEditorWindow
	{
		private static BehaviourTreeEditorWindow ThisWindow;
		
		[MenuItem( "BaseFramework/AI/Behaviour Tree Editor" )]
		private static void OpenWindow()
		{
			ThisWindow = EditorWindow.GetWindow<BehaviourTreeEditorWindow>();
			ThisWindow.name = "Behaviour Tree Editor";
		}
		
		public override void LoadView()
		{
			m_menuView = new BehaviourTreeMenuView( this );
			m_menuView.CreateNewTree += CreateNewTree;
			AddSubview( m_menuView );
			
			float fMenuHeight = m_menuView.ViewBounds.height;
			float fWindowWidth  = this.maxSize.x;
			float fWindowHeight = this.maxSize.y;
			m_editorView = new NodeEditorView( this );
			m_editorView.ViewBounds = new Rect( 0, fMenuHeight, fWindowWidth, fWindowHeight );
			
			AddSubview( m_editorView );
		}
		
		private void CreateNewTree()
		{
//			BehaviourTree xNewTree = new BehaviourTree();
//			m_editorView.SetTree( xNewTree );
		}
		
		private BehaviourTreeMenuView m_menuView;
		private NodeEditorView m_editorView;
	}
}
