using UnityEngine;
using BaseFramework.Core;
using BaseFramework.InputManager;


public class SwipeSubscriber : MonoBehaviour, IObserver<Vector2>
{
	public void OnNotify( Vector2 data )
	{
		rigidbody.AddForce( data );
	}
	
	private void Start()
	{
		gestureHandler = (GestureInputHandler)GestureInputHandler.Instance;
		gestureHandler.RegisterSubscription( this );
	}
	
	private GestureInputHandler gestureHandler;
}