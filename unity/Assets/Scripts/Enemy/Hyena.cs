using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class Hyena : Enemy 
	{
		// Use this for initialization
		void Start () 
		{
			init();
		}
		
		// Update is called once per frame
		void Update () 
		{
			m_stateMachine.OnUpdate();
		}

		public override void EnterAttackState()
		{
			m_stateMachine.SetState(new ChargeState(m_stateMachine));
		}

		/// <summary>
		/// Sent when an incoming collider makes contact with this object's
		/// collider (2D physics only).
		/// </summary>
		/// <param name="other">The Collision2D data associated with this collision.</param>
		void OnCollisionEnter2D(Collision2D other)
		{
			if (other.collider.GetComponent<PlayerController>())
			{
				Debug.Log(gameObject.name + " killed the player");
			}
		}
	}
}
