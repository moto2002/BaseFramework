using UnityEngine;
using System.Collections;


namespace BaseFramework
{
	[RequireComponent (typeof (CharacterController))]
	public class BaseActor : MonoBehaviour
	{
		public float MoveSpeed;
		public float JumpPower;
		
		private CharacterController m_controller;
		
		void Awake ()
		{
			m_controller = GetComponent<CharacterController> ();
		}
		
		void Start ()
		{
			
		}
		
		public virtual void Spawn ()
		{
			
		}
		
		void Update ()
		{
			float forward = Input.GetAxis ("Vertical");
			float strafe = Input.GetAxis ("Horizontal");
			float jump = 0.0f;
			
			forward *= MoveSpeed;
			strafe *= MoveSpeed;
			
			if (m_controller.isGrounded)
			{
				// todo : jumping
				 //float jump = Input.GetAxis ("Jump");
			}
			
			
			m_controller.SimpleMove (forward * transform.forward + strafe * transform.right);
		}
	}
}