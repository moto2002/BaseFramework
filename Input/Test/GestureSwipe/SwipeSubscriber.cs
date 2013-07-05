using UnityEngine;
using BaseFramework.Core;
using BaseFramework.InputManager;


public class SwipeSubscriber : MonoBehaviour, IObserver<Vector2>
{
	public float MAX_SPEED;
	
	public void OnNotify( Vector2 data )
	{
		Vector3 force = Vector3.zero;
		force.x = data.x;
		force.z = data.y;
		
		// Calculate the impulse
		m_velocity += (force * Time.deltaTime) / rigidbody.mass;
		m_velocity = Vector3.ClampMagnitude( m_velocity, MAX_SPEED );
	}
	
	private void Start()
	{
		gestureHandler = InputManager.Instance.RetrieveObject<GestureInputHandler>();
		gestureHandler.RegisterSubscription( this );
	}
	
	private void FixedUpdate()
	{
		transform.position += m_velocity * Time.deltaTime;
	}
	
	private void OnCollisionEnter( Collision collisionInfo )
	{
		Vector3 normal = collisionInfo.contacts[0].normal;
		Vector3 newVelocity = m_velocity - ( 2 * Vector3.Dot( m_velocity, normal ) * normal );
		
		m_velocity = newVelocity;
	}
	
	private Vector3 m_velocity;
	private GestureInputHandler gestureHandler;
}