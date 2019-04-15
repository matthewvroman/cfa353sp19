using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class BaseState : State<PlayerController>
	{
		public BaseState(StateMachine<PlayerController> machine) : base(machine)
		{
			//Do nothing
		}

		public override void OnExit()
		{
			m_stateMachine.controller.m_sound.Stop("Run");
		}
		
		public override void OnUpdate() 
		{
			m_stateMachine.controller.Move(Input.GetAxis("Horizontal"), true);
			if (Input.GetKeyDown(KeyCode.Space))
			{
				m_stateMachine.SetState(new JumpState(m_stateMachine, true));
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			{
				m_stateMachine.SetState(new CrouchState(m_stateMachine));
			}
			if (m_stateMachine.controller.canClimb && Input.GetAxis("Climb") > 0)
			{
				m_stateMachine.SetState(new ClimbState(m_stateMachine));
			}
			m_stateMachine.controller.DropBait();
		}

		public override void TriggerEntered(Collider2D collider) 
		{
			m_stateMachine.controller.Climbable(collider.CompareTag("Climbable"), true);
			m_stateMachine.controller.Hideable(collider, true);
		}

		public override void TriggerExited(Collider2D collider) 
		{
			m_stateMachine.controller.Climbable(collider.CompareTag("Climbable"), false);
			m_stateMachine.controller.Hideable(collider, false);
		}

		public override void CollisionExited(Collision2D collision) 
		{
			if (!m_stateMachine.controller.IsGrounded(m_stateMachine.controller.circleCollider))
            {
                m_stateMachine.SetState(new JumpState(m_stateMachine, false));
            }
		}
	}
}
