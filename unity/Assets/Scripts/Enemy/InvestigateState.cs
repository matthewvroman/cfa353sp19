using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class InvestigateState : State<Enemy>
	{
		Vector3 searchPoint;
		int moveDir = 1;
		public InvestigateState(StateMachine<Enemy> machine, Vector3 point) : base(machine) 
		{
			searchPoint = point;
		}
		public override void OnEnter()
		{
			moveDir = m_stateMachine.controller.GetMoveDirection(searchPoint);
		}

		public override void OnExit()
		{
			
		}

		public override void OnUpdate()
		{
			if (m_stateMachine.controller.IsNearPoint(searchPoint))
			{
				m_stateMachine.SetState(new SearchState(m_stateMachine, searchPoint));
			}
			else
			{
				if (m_stateMachine.controller.IsNextToCliff(moveDir) || !m_stateMachine.controller.IsReachable(searchPoint))
				{
					m_stateMachine.SetState(new PatrolState(m_stateMachine));
				}
				else
				{
					m_stateMachine.controller.Move(Mathf.Sign((searchPoint - m_stateMachine.controller.transform.position).x),true);
				}
			}
		}
	}
}
