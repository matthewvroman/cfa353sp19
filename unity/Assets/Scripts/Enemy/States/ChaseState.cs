using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class ChaseState : State<Enemy> 
	{
		bool bait = false, initialized = false;
		float withinRange;


		public ChaseState(StateMachine<Enemy> machine, Transform target) : base(machine) 
		{
			m_stateMachine.controller.target = target;
		}

		public override void OnEnter()
		{
			if (m_stateMachine.controller.target == null)
			{
				m_stateMachine.SetState(new PatrolState(m_stateMachine));
				return;
			}

			m_stateMachine.controller.CreateStateIndicator("Chase");
			m_stateMachine.controller.stateIndicator.GetComponent<StateIndicator>().SetIndicator(m_stateMachine.controller);
			if (!initialized)
			{
				m_stateMachine.SetState(new WaitState(m_stateMachine, this));
			}
			else
			{
				if (m_stateMachine.controller.target.GetComponent<Bait>())
				{
					withinRange = m_stateMachine.controller.NEAR_PATROL_POINT;
					bait = true;
				}
				else
				{
					withinRange = m_stateMachine.controller.attackRange;
				}
				PlayerController.PlayerHidden += PlayerLost;
			}
		}

		public override void OnExit()
		{
			if (initialized)
			{
				GameObject.Destroy(m_stateMachine.controller.stateIndicator);
				PlayerController.PlayerHidden -= PlayerLost;
			}
			else
			{
				initialized = true;
			}
		}

		public override void OnUpdate()
		{
			float distance = Vector2.Distance(m_stateMachine.controller.transform.position, m_stateMachine.controller.target.position);
			float dir = Mathf.Clamp((m_stateMachine.controller.GetTargetDirection()).x, -1, 1);
			Vector3 targetPos = m_stateMachine.controller.target.position;
			if (distance <= withinRange)
			{
				if (bait)
				{
					m_stateMachine.SetState(new EatBaitState(m_stateMachine));
					m_stateMachine.controller.m_rigidbody.velocity = Vector3.zero;
				}
				else
				{
					m_stateMachine.controller.EnterAttackState();
				}
			}
			else if (distance >= m_stateMachine.controller.dectectionRange || !m_stateMachine.controller.IsReachable(targetPos))
			{
				m_stateMachine.SetState(new InvestigateState(m_stateMachine, m_stateMachine.controller.target.position));
			}
			else
			{
				m_stateMachine.controller.Move(dir, true);
			}
		}

		public override void CollisionEntered(Collision2D collision)
		{
			if (m_stateMachine.controller.HeadOnCollision(collision))
			{
				m_stateMachine.controller.KillPlayer(collision.gameObject);
			}
		}

		private void PlayerLost()
		{
			m_stateMachine.SetState(new InvestigateState(m_stateMachine, m_stateMachine.controller.target.position));
		}
	}
}
