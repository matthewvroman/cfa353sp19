using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class ChargeState : State<Enemy> 
	{
		float chargeDuration = 3;
		float chargeSpeed = 8;
		int chargeDir;

		public ChargeState(StateMachine<Enemy> machine) : base(machine) {}

		public override void OnEnter()
		{
			chargeDir = (int)Mathf.Sign(m_stateMachine.controller.GetTargetDirection().x);
		}

		public override void OnExit()
		{

		}

		public override void OnUpdate()
		{
			chargeDuration -= Time.deltaTime;
			if (chargeDuration <= 0)
			{
				if (m_stateMachine.controller.target != null)
				{
					m_stateMachine.SetState(new ChaseState(m_stateMachine, m_stateMachine.controller.target));
				}
				else
				{
					m_stateMachine.SetState(new PatrolState(m_stateMachine));
				}
			}
			else
			{
				if (m_stateMachine.controller.IsNextToCliff(chargeDir))
				{
					m_stateMachine.controller.ApplyDrag();
					if (m_stateMachine.controller.rigidbody.velocity.x == 0)
					{
						m_stateMachine.SetState(new ChaseState(m_stateMachine, m_stateMachine.controller.target));
					}
				}
				else
				{
					m_stateMachine.controller.ApplyMovement(chargeDir, chargeSpeed);	
				}
			}
		}
	}
}
