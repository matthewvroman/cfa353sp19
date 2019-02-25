using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class SearchState : State<Enemy>
	{
		static float SearchDuration = 5;
		float searchTimer;
		Vector3 searchPoint;
		Vector3 patrolPoint;
		float paceDistance = 3;
		int moveDir;


		public SearchState(StateMachine<Enemy> machine, Vector3 target): base (machine) 
		{
			searchPoint = target;
		}

		public override void OnEnter()
		{
			searchTimer = SearchDuration;
			RandomSearchPoint();
		}

		public override void OnExit()
		{
			
		}

		public override void OnUpdate()
		{
			searchTimer -= Time.deltaTime;
			if (searchTimer <= 0)
			{
				m_stateMachine.SetState(new PatrolState(m_stateMachine));
			}
			else
			{
				if (m_stateMachine.controller.IsNearPoint(patrolPoint))
				{
					RandomSearchPoint();
				}
				else
				{
					m_stateMachine.controller.Move(moveDir, true);
				}
			}
		}

		private void RandomSearchPoint()
		{
			moveDir *= -1;
			patrolPoint = searchPoint + new Vector3(Random.Range(0,moveDir*paceDistance),0,0);
		}

	}
}
