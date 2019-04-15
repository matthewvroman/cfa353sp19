using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class ClimbState : State<PlayerController>
	{
		bool climbing = false;

		public ClimbState(StateMachine<PlayerController> machine) : base(machine)
		{
			//Do nothing
		}

		public override void OnEnter()
		{
			m_stateMachine.controller.m_rigidbody.gravityScale = 0;
			m_stateMachine.controller.PlayAnimation("Climb");
		}

		public override void OnExit()
		{
			m_stateMachine.controller.m_rigidbody.gravityScale = 1;
			m_stateMachine.controller.m_animator.speed = 1;
			m_stateMachine.controller.m_sound.Stop("Climb");
		}

		public override void OnUpdate() 
		{
			m_stateMachine.controller.Climb(Input.GetAxis("Climb"));
			m_stateMachine.controller.Move(Input.GetAxis("Horizontal"), false);

			if (Input.GetKeyDown(KeyCode.Space))
			{
				m_stateMachine.SetState(new JumpState(m_stateMachine, true));
			}
			m_stateMachine.controller.DropBait();
		}

		public override void TriggerEntered(Collider2D collider) 
		{
			m_stateMachine.controller.Hideable(collider, true);
		}

		public override void TriggerExited(Collider2D collider) 
		{
			if (collider.CompareTag("Climbable"))
			{
				m_stateMachine.controller.canClimb = false;
				if (m_stateMachine.controller.IsGrounded(m_stateMachine.controller.circleCollider)) m_stateMachine.SetState(new BaseState(m_stateMachine));
				else m_stateMachine.SetState(new JumpState(m_stateMachine, false));
			}
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
