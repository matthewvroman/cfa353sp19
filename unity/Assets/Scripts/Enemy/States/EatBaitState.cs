using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class EatBaitState : State<Enemy> 
	{
		public EatBaitState(StateMachine<Enemy> machine) : base(machine) {}

		public override void OnUpdate()
		{
			if (m_stateMachine.controller.target == null)
			{
				Transform target = m_stateMachine.controller.m_sight.CheckForTargets();
				if (target != null)
				{
					m_stateMachine.controller.TargetSpotted(target);
				}
				else
				{
					m_stateMachine.SetState(new PatrolState(m_stateMachine));
				}
			}
			else if (!m_stateMachine.controller.IsNearPoint(m_stateMachine.controller.target.position))
			{
				m_stateMachine.SetState(new ChaseState(m_stateMachine, m_stateMachine.controller.target));
			}
			else
			{
				GetTarget().GetComponent<Bait>().EatBait(m_stateMachine.controller.baitEatingSpeed * Time.deltaTime);
			}
		}

		private Collider2D GetTarget()
		{
			string[] layers = {"Bait", "Player"};
			return Physics2D.OverlapCircle(m_stateMachine.controller.transform.position, m_stateMachine.controller.NEAR_PATROL_POINT, LayerMask.GetMask(layers));
		}
	}
}
