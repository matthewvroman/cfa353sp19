using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class KnockbackState : State<PlayerController> 
	{
		Vector2 dir;
		float m_duration = 1;
		public KnockbackState(StateMachine<PlayerController> machine, Vector2 direction, float duration = 1) : base(machine)
		{
			dir = direction;
			m_duration = duration;
		}

		public override void OnEnter()
		{
			m_stateMachine.controller.Rigidbody.velocity = dir;
		}

		public override void OnUpdate()
		{
			m_duration -= Time.deltaTime;
			if (m_duration <= 0)
			{
				m_stateMachine.SetState(new BaseState(m_stateMachine));
			}
		}

		public override void CollisionExited(Collision2D collision)
		{
			ContactPoint2D[] contacts = new ContactPoint2D[1];
			if (collision.gameObject.CompareTag("Ground") && collision.GetContacts(contacts) > 0)
			{
				float angle = Vector2.Angle(contacts[0].point - (Vector2)m_stateMachine.controller.transform.position, Vector2.right);
				if (angle <= 30 || angle >= 150)
				{
					m_stateMachine.SetState(new BaseState(m_stateMachine));
				}
			}
		}
	}
}
