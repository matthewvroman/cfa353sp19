using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class DeadState : State<PlayerController>
	{
		public DeadState(StateMachine<PlayerController> machine): base(machine)
		{
			
		}

		public override void OnEnter()
		{
			GameObject.Destroy(m_stateMachine.controller.detectionBox);
            Physics2D.IgnoreLayerCollision(9, 12, true);
            m_stateMachine.controller.m_animator.speed = 1;
			m_stateMachine.controller.m_animator.Play("Death");
		}
	}
}
