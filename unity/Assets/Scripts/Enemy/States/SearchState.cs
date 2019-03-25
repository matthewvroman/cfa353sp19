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
			if (m_stateMachine.controller.IsNearPoint(patrolPoint) || m_stateMachine.controller.IsNextToCliff(moveDir))
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
			float range = moveDir*paceDistance;
			float y = m_stateMachine.controller.transform.position.y;
			RaycastHit2D hit = m_stateMachine.controller.CheckDistance(new Vector2(searchPoint.x, y));
			if (hit)
			{
				range = (hit.point - (Vector2)searchPoint).x;
			}
			patrolPoint = searchPoint + new Vector3(Random.Range((range > 1) ? 1 : 0, range),0,0);
		}

		public bool IsAlertable(Vector2 newSearchPoint)
		{
			if (Vector2.Distance(newSearchPoint, searchPoint) > 1)
			{
				return true;
			}
			return false;
		}

		public override void CollisionEntered(Collision2D collision)
		{
			PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player)
            {
                m_stateMachine.controller.Knockback(collision, player);
            }
		}

		public override void TriggerEntered(Collider2D collider)
        {
            if (collider.GetComponentInParent<PlayerController>())
            {
                m_stateMachine.controller.AlertEnemy(collider.transform.position);
            }
        }
	}
}
