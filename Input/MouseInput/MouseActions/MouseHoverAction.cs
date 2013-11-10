using UnityEngine;

namespace BaseFramework.MouseInput
{
	public class MouseHoverAction : MouseAction
	{
		public MouseHoverAction( Collider xCollider, MouseActionDelegate xDelegate ) : base( xCollider, xDelegate )
		{
		}
		
		protected override void OnHover (Vector3 xCursorPosition)
		{
			base.OnHover (xCursorPosition);
			state = MouseActionState.MouseActionChanged;
		}
	}
}
