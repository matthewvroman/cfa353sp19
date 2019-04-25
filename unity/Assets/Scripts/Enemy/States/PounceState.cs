using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class PounceState : State<Enemy>
	{
		float forwardForce = 8;
		bool initialized = false;

		public PounceState(StateMachine<Enemy> machine) : base(machine) {}

		public override void OnEnter()
		{
			m_stateMachine.controller.CreateStateIndicator("Attack");
			m_stateMachine.controller.stateIndicator.GetComponent<StateIndicator>().SetIndicator(m_stateMachine.controller);
			m_stateMachine.controller.PlayAnimation("Attack");

			if (!initialized)
			{
				m_stateMachine.SetState(new WaitState(m_stateMachine, this));
				m_stateMachine.controller.m_sound.Play("Cry");
			}
			else
			{
				m_stateMachine.controller.Attack();
				Vector2 dir = m_stateMachine.controller.GetTargetDirection();
				m_stateMachine.controller.CheckOrientation(dir.x);
				float angle = 40;
				float force =  Mathf.Sqrt(((Mathf.Abs(dir.x)+2)*-Physics2D.gravity.y)/Mathf.Sin(Mathf.Deg2Rad*2*angle));
				Vector2 velocity = (Quaternion.Euler(0,0,angle) * (Vector2.right * force));
				velocity.x *= Mathf.Sign(dir.x);
				velocity.y = Mathf.Clamp(velocity.y, 0.5f, 10);
				m_stateMachine.controller.m_rigidbody.velocity = velocity;
				m_stateMachine.controller.m_sound.Play("Pounce");
			}
		}

		public override void OnExit()
		{
			if (initialized)
			{
				GameObject.Destroy(m_stateMachine.controller.stateIndicator);
				m_stateMachine.controller.Attack();
				m_stateMachine.controller.PlaySound("Land");
			}
			else
			{
				initialized = true;
			}
		}

		public override void CollisionEntered(Collision2D collision)
		{
			if (collision.gameObject.CompareTag("Ground") && initialized)
			{
				Transform target = m_stateMachine.controller.m_sight.CheckForTargets();
				if (target != null)
				{
					m_stateMachine.SetState(new ChaseState(m_stateMachine, target));
				}
				else
				{
					m_stateMachine.SetState(new InvestigateState(m_stateMachine, m_stateMachine.controller.transform.position));
				}
			}
		}
	}
}
