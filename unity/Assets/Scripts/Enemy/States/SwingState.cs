using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class SwingState : State<Enemy> 
	{
		Transform attackPoint;
		Vector3 searchPoint;
		float attackSize = 0.6f;
		float attackTime = 1;
		bool initialized = false;

		public SwingState(StateMachine<Enemy> machine) : base(machine)
		{
			attackPoint = m_stateMachine.controller.transform.GetChild(1);
		}

		public override void OnEnter()
		{
			m_stateMachine.controller.CreateStateIndicator("Attack");
			m_stateMachine.controller.stateIndicator.GetComponent<StateIndicator>().SetIndicator(m_stateMachine.controller);
			if (m_stateMachine.controller.BoxCollider.IsTouching(m_stateMachine.controller.target.GetComponent<Collider2D>()))
			{
				PlayerController.PlayerDied();
			}

			if (!initialized)
			{
				m_stateMachine.SetState(new WaitState(m_stateMachine, this, 0.25f));
			}
			else
			{
				searchPoint = m_stateMachine.controller.target.position;
				Animator anim = m_stateMachine.controller.GetComponent<Animator>();
				anim.SetTrigger("attack");
				attackTime = anim.GetCurrentAnimatorClipInfo(0).Length;
			}
		}

		public override void OnExit()
		{
			if (initialized)
			{
				GameObject.Destroy(m_stateMachine.controller.stateIndicator);
			}
			else
			{
				initialized = true;
			}
		}

		public override void OnUpdate()
		{
			if (Physics2D.OverlapBox(attackPoint.position, new Vector2(attackSize, attackSize), 0f, LayerMask.GetMask("Player")) != null)
			{
				PlayerController.PlayerDied();
			}
			attackTime -= Time.deltaTime;
			if (attackTime <= 0)
			{
				Transform target = m_stateMachine.controller.sight.CheckForTargets();
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

		public override void CollisionEntered(Collision2D collision)
		{
			m_stateMachine.controller.KillPlayer(collision.gameObject);
		}
	}
}
