using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class ChargeState : State<Enemy> 
	{
		Vector2 targetPos;
		float chargeDuration = 3, chargeSpeed = 8;
		int chargeDir;
		bool initialized = false;

		public ChargeState(StateMachine<Enemy> machine) : base(machine) 
		{
			m_stateMachine.SetState(new WaitState(m_stateMachine, this));
		}

		public override void OnEnter()
		{
			targetPos = m_stateMachine.controller.target.position;
			chargeDir = (int)Mathf.Sign(m_stateMachine.controller.GetTargetDirection().x);
			m_stateMachine.controller.CreateStateIndicator("Attack");
			m_stateMachine.controller.stateIndicator.GetComponent<StateIndicator>().SetIndicator(m_stateMachine.controller);
			if (m_stateMachine.controller.BoxCollider.IsTouching(m_stateMachine.controller.target.GetComponent<Collider2D>()))
			{
				PlayerController.PlayerDied();
			}
			if (!initialized)
			{
				m_stateMachine.SetState(new WaitState(m_stateMachine, this));
			}
		}

		public override void OnExit() 
		{
			if (initialized)
			{
				GameObject.Destroy(m_stateMachine.controller.stateIndicator);
			}
			else
			{
				initialized = true;
			}
		}

		public override void OnUpdate()
		{
			chargeDuration -= Time.deltaTime;
			if (chargeDuration <= 0)
			{
				m_stateMachine.SetState(new InvestigateState(m_stateMachine, targetPos));
			}
			else
			{
				if (m_stateMachine.controller.IsNextToCliff(chargeDir))
				{
					m_stateMachine.controller.ApplyDrag();
					if (m_stateMachine.controller.Rigidbody.velocity.x == 0)
					{
						m_stateMachine.SetState(new ChaseState(m_stateMachine, m_stateMachine.controller.target));
					}
				}
				else
				{
					m_stateMachine.controller.ApplyMovement(chargeDir, chargeSpeed);	
				}
			}
		}

		public override void CollisionEntered(Collision2D collision)
		{
			if (collision.gameObject.GetComponent<PlayerController>())
			{
				PlayerController.PlayerDied();
			}
			else if (collision.gameObject.CompareTag("Ground") && m_stateMachine.controller.HeadOnCollision(collision))
			{
				chargeDuration = 0;
			}
		}
	}
}
