using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class SearchState : State<Enemy>
	{
		float paceDistance;
		Vector3 searchPoint;
		Vector3 patrolPoint;
		int moveDir = 1;


		public SearchState(StateMachine<Enemy> machine, Vector3 target): base (machine) 
		{
			searchPoint = target;
			paceDistance = 6;
		}

		public override void OnEnter()
		{
			if (m_stateMachine.controller.stateIndicator == null)
			{
				m_stateMachine.controller.CreateStateIndicator("Search");
				m_stateMachine.controller.stateIndicator.GetComponent<SearchIndicator>().StartSearch(m_stateMachine.controller, 10);
			}
			RandomSearchPoint();
		}

		public override void OnUpdate()
		{
			if (m_stateMachine.controller.IsNearPoint(patrolPoint) || !m_stateMachine.controller.IsReachable(patrolPoint))
			{
				m_stateMachine.SetState(new WaitState(m_stateMachine, this));
			}
			else
			{
				m_stateMachine.controller.Move(moveDir, false);
			}
		}

		private void RandomSearchPoint()
		{
			moveDir *= -1;
			patrolPoint = searchPoint + new Vector3(Random.Range(1,moveDir*paceDistance),0,0);
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
