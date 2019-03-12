using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class WaitState : State<Enemy> 
	{
		State<Enemy> previousState;
		float waitTimer;
		public WaitState(StateMachine<Enemy> machine, State<Enemy> state, float waitTime = 1) : base(machine)
		{
			previousState = state;
			waitTimer = waitTime;
			m_stateMachine.controller.Stop();
		}

		public override void OnUpdate()
		{
			waitTimer -= Time.deltaTime;
			if (waitTimer <= 0)
			{
				m_stateMachine.SetState(previousState);
			}
		}

		public override void CollisionEntered(Collision2D collision)
		{
			if (collision.gameObject.GetComponent<PlayerController>() && !previousState.CompareState("Chase") && !previousState.CompareState("Charge"))
			{
				m_stateMachine.controller.AlertEnemy(collision.contacts[0].point);
			}
		}
	}
}
