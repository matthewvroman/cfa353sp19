using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class ChaseState : State<Enemy> 
	{
		bool bait = false;
		float withinRange;
		public ChaseState(StateMachine<Enemy> machine, Transform Target) : base(machine) 
		{
			m_stateMachine.controller.target = Target;
		}

		public override void OnEnter()
		{
			if (m_stateMachine.controller.target == null)
			{
				m_stateMachine.SetState(new PatrolState(m_stateMachine));
			}

			if (m_stateMachine.controller.target.GetComponent<Bait>())
			{
				withinRange = m_stateMachine.controller.NEAR_PATROL_POINT;
				bait = true;
			}
			else
			{
				withinRange = m_stateMachine.controller.attackRange;
			}
		}

		public override void OnExit()
		{
			
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
					m_stateMachine.controller.rigidbody.velocity = Vector3.zero;
				}
				else
				{
					m_stateMachine.controller.EnterAttackState();
				}
			}
			else if (distance >= m_stateMachine.controller.dectectionRange || m_stateMachine.controller.IsNextToCliff(dir) || !m_stateMachine.controller.IsReachable(targetPos))
			{
				m_stateMachine.SetState(new InvestigateState(m_stateMachine, m_stateMachine.controller.target.position));
			}
			else
			{
				if (!m_stateMachine.controller.IsNextToCliff(dir))
				{
					m_stateMachine.controller.Move(dir, true);
				}
			}
		}


	}
}
