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
			BehaviourTreeMenuView xMenuView = new BehaviourTreeMenuView( this );
			AddSubview( xMenuView );
			
			float fMenuHeight = xMenuView.ViewBounds.height;
			NodeEditorView xEditorView = new NodeEditorView( this, fMenuHeight );
			AddSubview( xEditorView );
		}
	}
}
