using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class ChaseState : State<Enemy> 
	{
		bool bait = false, initialized = false;
		float withinRange;
		int moveDir = 1;
		bool changeDir = false;


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
			moveDir = (int)Mathf.Clamp((m_stateMachine.controller.GetTargetDirection()).x, -1, 1);
			if (!initialized)
			{
				m_stateMachine.SetState(new WaitState(m_stateMachine, this));
			}
			else
			{
				if (changeDir)
				{
					changeDir = false;
					return;
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
			if (moveDir != (int)Mathf.Clamp((m_stateMachine.controller.GetTargetDirection()).x, -1, 1))
			{
				changeDir = true;
				m_stateMachine.SetState(new WaitState(m_stateMachine, this, 1, false));
			}
			Vector3 targetPos = m_stateMachine.controller.target.position;
			if (distance <= withinRange)
			{
				m_stateMachine.controller.EnterAttackState();
			}
			else if (distance > m_stateMachine.controller.dectectionRange || !m_stateMachine.controller.IsReachable(targetPos))
			{
				m_stateMachine.SetState(new InvestigateState(m_stateMachine, m_stateMachine.controller.target.position));
			}
			else
			{
				m_stateMachine.controller.Move(moveDir, true);
			}
		}

		private void PlayerLost()
		{
			m_stateMachine.SetState(new InvestigateState(m_stateMachine, m_stateMachine.controller.target.position));
		}
	}
}
