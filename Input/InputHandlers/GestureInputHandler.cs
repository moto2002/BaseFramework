using UnityEngine;
using BaseFramework.Core;
using System.Collections.Generic;


namespace BaseFramework.InputManager
{
	public class GestureInputHandler : InputHandler, INotifier<Vector2>
	{
#region InputHandler Implementation
		protected override void InitialiseInputHandler ()
		{
			registeredGestures = new List<Gesture>();
			registeredGestures.Add( new SwipeGesture() );
			
			m_touches = new List<InputData>();
			m_subscriptions = new List<IObserver<Vector2>>();
		}
		
		protected override InputMethod ValidInputMethods()
		{
			return InputMethod.TouchInput;
		}
		
		public override void OnInputStart( InputData data )
		{
			m_touches.Add( data );
			NotifySubscribers( );
		}
		
		public override void OnInputTick( InputData data )
		{
			NotifySubscribers( );
		}
		
		public override void OnInputEnd( InputData data )
		{
			NotifySubscribers( );
			m_touches.Remove( data );
		}
#endregion
		
#region INotifier Implementation
		public bool RegisterSubscription( IObserver<Vector2> subscriber )
		{
			if ( m_subscriptions.Contains( subscriber ) )
			{
				return false;
			}
			m_subscriptions.Add( subscriber );
			return true;
		}
		
		public bool UnregisterSubscription( IObserver<Vector2> subscriber )
		{
			if ( m_subscriptions.Contains( subscriber ) )
			{
				m_subscriptions.Remove( subscriber );
				return true;
			}
			return false;
		}
		
		public void NotifySubscribers ()
		{
			InputData[] data = m_touches.ToArray();
			for ( int gIndex = 0; gIndex < registeredGestures.Count; gIndex++ )
			{
				Gesture g = registeredGestures[ gIndex ];
				if ( g.NumberOfTouchesRequiredForGesture() == data.Length )
				{
					//TODO: Functionality such that subscribers choose which Gestures to listen to.
					//		Could be implemented by making the Gesture implement INotifier instead.
					Vector2 gestureResult = g.UpdateGesture( data ); //TODO: Notify Observers of result
					for ( int sIndex = 0; sIndex < m_subscriptions.Count; sIndex++ )
					{
						IObserver<Vector2> sub = m_subscriptions[ sIndex ];
						sub.OnNotify( gestureResult );
					}
				}
			}
		}
#endregion
		
		private List<InputData> m_touches;
		private List<Gesture> registeredGestures;
		private List<IObserver<Vector2>> m_subscriptions;
	}
}