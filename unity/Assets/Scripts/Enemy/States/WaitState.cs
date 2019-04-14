using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class WaitState : State<Enemy> 
	{
		State<Enemy> previousState;
		float waitTimer;
		bool m_stop = true;
		public WaitState(StateMachine<Enemy> machine, State<Enemy> state, float waitTime = 1, bool stop = true) : base(machine)
		{
			previousState = state;
			waitTimer = waitTime;
			m_stop = stop;
		}

		public override void OnUpdate()
		{
			if (m_stop || !m_stateMachine.controller.IsReachable(m_stateMachine.controller.GetForwardPosition()))
			{
				m_stateMachine.controller.ApplyDrag();
			}
			waitTimer -= Time.deltaTime;
			if (waitTimer <= 0)
			{
				m_stateMachine.SetState(previousState);
			}
		}

		public override void CollisionEntered(Collision2D collision)
		{
			PlayerController player = collision.gameObject.GetComponent<PlayerController>();
			if (player)
			{
				if (!previousState.CompareState("ChaseState") && !CheckAtatckState())
				{
					m_stateMachine.controller.Knockback(collision, player);
				}
			}
		}

		private bool CheckAtatckState()
		{
			return previousState.CompareState(m_stateMachine.controller.attackType.ToString() + "State");
		}

		public override void TriggerEntered(Collider2D collider)
        {
            if (collider.GetComponentInParent<PlayerController>() && !previousState.CompareState("ChaseState"))
            {
                m_stateMachine.controller.AlertEnemy(collider.transform.position);
            }
        }

		public bool IsAlertable(Vector2 newSearchPoint)
		{
			if (previousState.CompareState("ChaseState") || CheckAtatckState())
			{
				return false;
			}
			else if (previousState.CompareState("InvestigateState"))
			{
				return ((InvestigateState)previousState).IsAlertable(newSearchPoint);
			}
			else if (previousState.CompareState("SearchState"))
			{
				return ((SearchState)previousState).IsAlertable(newSearchPoint);
			}
			return true;
		}
	}
}
