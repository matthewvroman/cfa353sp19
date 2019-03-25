using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class JumpState : State<PlayerController>
	{
		public JumpState(StateMachine<PlayerController> machine, bool jumped) : base(machine)
		{
			if (jumped)
			{
				m_stateMachine.controller.Rigidbody.velocity += Vector2.up * (m_stateMachine.controller.jumpForce * m_stateMachine.controller.GetSpeedModifier());
			}
		}

		public override void OnEnter()
		{
			m_stateMachine.controller.Rigidbody.gravityScale = 1;
		}

		public override void OnUpdate() 
		{
            if (m_stateMachine.controller.Rigidbody.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                m_stateMachine.controller.Rigidbody.velocity += Vector2.up * Physics.gravity.y * (m_stateMachine.controller.lowJumpMultiplyer - 1) * Time.deltaTime;
            }

			m_stateMachine.controller.Move(Input.GetAxis("Horizontal"), true);
			if (m_stateMachine.controller.canClimb && Input.GetAxis("Climb") > 0)
			{
				m_stateMachine.SetState(new ClimbState(m_stateMachine));
			}
			m_stateMachine.controller.DropBait();
		}

		public override void TriggerEntered(Collider2D collider) 
		{
			if (collider.CompareTag("Climbable"))
			{
				m_stateMachine.controller.canClimb = true;
			}
			m_stateMachine.controller.Hideable(collider, true);
		}

		public override void TriggerExited(Collider2D collider) 
		{
			if (collider.CompareTag("Climbable"))
			{
				m_stateMachine.controller.canClimb = false;
			}
			m_stateMachine.controller.Hideable(collider, false);
		}

		public override void CollisionEntered(Collision2D collision)
		{
			if (m_stateMachine.controller.IsGrounded(m_stateMachine.controller.circleCollider))
            {
                m_stateMachine.SetState(new BaseState(m_stateMachine));
            }
		}
	}
}
