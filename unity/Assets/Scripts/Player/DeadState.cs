using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class DeadState : State<PlayerController>
	{
		int direction;
		public DeadState(StateMachine<PlayerController> machine, int dir): base(machine)
		{
			direction = dir;
		}

		public override void OnEnter()
		{
			m_stateMachine.controller.detectionBox.enabled = false;
            Physics2D.IgnoreLayerCollision(9, 12, true);
            m_stateMachine.controller.m_animator.speed = 1;
			string name = "Death ";
			name += (direction < 0 ? "Right" : "Left");
			m_stateMachine.controller.m_animator.Play(name);
		}

		public override void OnExit()
		{
			m_stateMachine.controller.transform.rotation = Quaternion.Euler(Vector3.zero);
			m_stateMachine.controller.detectionBox.enabled = true;
			Physics2D.IgnoreLayerCollision(9, 12, false);
		}

		public override void CollisionStayed(Collision2D collision)
		{
			if (collision.gameObject.CompareTag("Ground"))
			{
				m_stateMachine.controller.ApplyDrag();
			}
		}
	}
}
