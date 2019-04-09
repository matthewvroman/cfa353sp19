using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class BombardState : State<Enemy>
	{
		GameObject projectile;
		GameObject[] shots = new GameObject[3];
		Vector2 searchPoint;
		bool initialized = false;
		public BombardState(StateMachine<Enemy> machine) : base(machine)
		{
			projectile = Resources.Load<GameObject>("Prefabs/Spawnables/Projectile");
		}

		public override void OnEnter()
		{
			m_stateMachine.controller.CreateStateIndicator("Attack");
			m_stateMachine.controller.stateIndicator.GetComponent<StateIndicator>().SetIndicator(m_stateMachine.controller);
			if (m_stateMachine.controller.m_boxCollider.IsTouching(m_stateMachine.controller.target.GetComponent<Collider2D>()))
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
				float offset = m_stateMachine.controller.m_boxCollider.bounds.extents.x;
				dir.x += (dir.x > 0) ? -offset : offset;
				searchPoint = m_stateMachine.controller.target.position;
				m_stateMachine.controller.CheckOrientation(dir.x);
				float angle = 50;
				for (int i = 0; i < shots.Length; i++)
				{
					float multiplier = 2;
					float range = Mathf.Abs(dir.x + (i * multiplier) - multiplier);

					float force =  Mathf.Sqrt(range*-Physics2D.gravity.y/Mathf.Sin(Mathf.Deg2Rad*2*angle));
					Vector2 velocity = (Quaternion.Euler(0,0,angle) * (Vector2.right * force));
					velocity.x *= Mathf.Sign(dir.x);

					shots[i] = GameObject.Instantiate(projectile, m_stateMachine.controller.GetForwardPosition(), Quaternion.identity, null);
					shots[i].GetComponent<Projectile>().ShootProjectile(velocity);
				}
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
			if (CheckShots())
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

		private bool CheckShots()
		{
			for (int i = 0; i < shots.Length; i++)
			{
				if (shots[i] != null)
				{
					return false;
				}
			}
			return true;
		}
	}
}
