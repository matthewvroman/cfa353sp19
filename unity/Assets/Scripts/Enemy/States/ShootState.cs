using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class ShootState : State<Enemy> 
	{
		GameObject projectile, shot;
		Vector2 searchPoint;
		float shootSpeed = 5;
		bool initialized = false;

		public ShootState(StateMachine<Enemy> machine) : base(machine)
		{
			projectile = Resources.Load<GameObject>("Prefabs/Spawnables/Projectile");
		}

		public override void OnEnter()
		{
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
			else
			{
				Vector2 dir = m_stateMachine.controller.GetTargetDirection();
				searchPoint = m_stateMachine.controller.target.position;
				m_stateMachine.controller.CheckOrientation(dir.x);
				shot = GameObject.Instantiate(projectile, m_stateMachine.controller.GetForwardPosition(), Quaternion.identity, null);
				shot.GetComponent<Projectile>().ShootProjectile((dir.x >= 0 ? Vector2.right : Vector2.left)*shootSpeed, false);
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
			if (shot == null)
			{
				Transform target = m_stateMachine.controller.sight.CheckForTargets();
				if(target != null)
				{
					m_stateMachine.SetState(new ChaseState(m_stateMachine, target));
				}
				else
				{
					m_stateMachine.SetState(new InvestigateState(m_stateMachine, searchPoint));
				}
			}
		}

		public override void CollisionEntered(Collision2D collision)
		{
			m_stateMachine.controller.KillPlayer(collision.gameObject);
		}
	}
}
