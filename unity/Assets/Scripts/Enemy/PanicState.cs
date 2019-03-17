using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class PanicState : State<Enemy> 
	{
		Vector3 target;
		float panicTimer = 10, alarmRange = 10;
		int moveDir = 1;
		public PanicState(StateMachine<Enemy> machine, Vector2 playerPos) : base (machine)
		{
			target = playerPos;
		}

		public override void OnEnter()
		{
			m_stateMachine.controller.CreateStateIndicator("Chase");
			m_stateMachine.controller.stateIndicator.GetComponent<StateIndicator>().SetIndicator(m_stateMachine.controller);
			moveDir = -m_stateMachine.controller.GetMoveDirection(target);
			Collider2D[] colliders = Physics2D.OverlapCircleAll(m_stateMachine.controller.transform.position, alarmRange, LayerMask.GetMask("Enemy"));
			foreach (Collider2D col in colliders)
			{
				Enemy enemy = col.GetComponent<Enemy>();
				if (enemy)
				{
					enemy.AlertEnemy(target);
				}
			}
		}

		public override void OnExit()
		{
			GameObject.Destroy(m_stateMachine.controller.stateIndicator);
			m_stateMachine.controller.stateIndicator = null;
		}

		public override void OnUpdate()
		{
			panicTimer -= Time.deltaTime;
			if (panicTimer <= 0)
			{
				m_stateMachine.SetState(new PatrolState(m_stateMachine));
			}
			else
			{
				if (!m_stateMachine.controller.IsReachable(m_stateMachine.controller.transform.position + (m_stateMachine.controller.transform.right * moveDir)))
				{
					moveDir *= -1;
				}
				else
				{
					m_stateMachine.controller.Move(moveDir, true);
				}
			}
		}

		public override void TriggerEntered(Collider2D collider)
		{
			Enemy enemy = collider.GetComponent<Enemy>();
			PlayerController player = collider.GetComponent<PlayerController>();
			if (enemy)
			{
				enemy.AlertEnemy(target);
			}
		}
	}
}
