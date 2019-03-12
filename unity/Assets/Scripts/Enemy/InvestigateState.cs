using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class InvestigateState : State<Enemy>
	{
		Vector3 searchPoint;
		int moveDir = 1;
		bool initialized = false;

		public InvestigateState(StateMachine<Enemy> machine, Vector3 point) : base(machine) 
		{
			searchPoint = point;
		}
		public override void OnEnter()
		{
			moveDir = m_stateMachine.controller.GetMoveDirection(searchPoint);
			m_stateMachine.controller.CheckOrientation(moveDir);
			m_stateMachine.controller.target = null;
			m_stateMachine.controller.CreateStateIndicator("Investigate");
			m_stateMachine.controller.stateIndicator.GetComponent<StateIndicator>().SetIndicator(m_stateMachine.controller);
			if (!initialized)
			{
				m_stateMachine.SetState(new WaitState(m_stateMachine, this));
			}
		}

		public override void OnExit() 
		{
			if (initialized)
			{
				GameObject.Destroy(m_stateMachine.controller.stateIndicator);
				m_stateMachine.controller.stateIndicator = null;
			}
			else
			{
				initialized = true;
			}
		}

		public override void OnUpdate()
		{
			if (m_stateMachine.controller.IsNearPoint(searchPoint))
			{
				m_stateMachine.SetState(new SearchState(m_stateMachine, searchPoint));
			}
			else
			{
				moveDir = m_stateMachine.controller.GetMoveDirection(searchPoint);
				if (!m_stateMachine.controller.IsReachable(searchPoint))
				{
					m_stateMachine.SetState(new PatrolState(m_stateMachine));
				}
				else
				{
					m_stateMachine.controller.Move(moveDir, true);
				}
			}
		}

		public override void CollisionEntered(Collision2D collision)
		{
			if (collision.gameObject.GetComponent<PlayerController>())
			{
				m_stateMachine.controller.AlertEnemy(collision.contacts[0].point);
			}
		}
	}
}
