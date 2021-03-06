﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class CrouchState : State<PlayerController>
	{
		public CrouchState(StateMachine<PlayerController> machine) : base(machine)
		{
			//Do nothing
		}

		public override void OnEnter()
		{
			m_stateMachine.controller.SetupCrouching(true);

			if (m_stateMachine.controller.canHide && !m_stateMachine.controller.IsSpotted())
			{
				m_stateMachine.controller.Hide(true);
			}
		}

		public override void OnExit()
		{
			m_stateMachine.controller.SetupCrouching(false);
			m_stateMachine.controller.Hide(false);
			m_stateMachine.controller.m_sound.Stop("Sneak");
			m_stateMachine.controller.m_sound.Stop("Hidden");
		}

		public override void OnUpdate() 
		{
			m_stateMachine.controller.Move(Input.GetAxis("Horizontal"), false);
			if (m_stateMachine.controller.CanUncrouch())
			{
				if (Input.GetKeyDown(KeyCode.Space))
				{
					m_stateMachine.SetState(new JumpState(m_stateMachine, true));
				}
				if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
				{
					m_stateMachine.SetState(new BaseState(m_stateMachine));
				}
				if (m_stateMachine.controller.canClimb && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
				{
					m_stateMachine.SetState(new ClimbState(m_stateMachine));
				}
			}
		}

		public override void TriggerEntered(Collider2D collider) 
		{
			if (collider.CompareTag("Climbable"))
            {
                m_stateMachine.controller.canClimb = true;
            }
            else if (collider.CompareTag("Hideable"))
            {
                m_stateMachine.controller.canHide = true;
				if (!m_stateMachine.controller.IsSpotted())
				{
					m_stateMachine.controller.Hide(true);
				}
            }
		}

		public override void TriggerExited(Collider2D collider) 
		{
			if (collider.CompareTag("Climbable"))
			{
				m_stateMachine.controller.canClimb = false;
			}
			else if (collider.CompareTag("Hideable") && !m_stateMachine.controller.circleCollider.IsTouching(collider))
			{
				m_stateMachine.controller.canHide = false;
				m_stateMachine.controller.Hide(false);
			}
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
