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
			projectile = Resources.Load<GameObject>("Spawnables/Projectile");
		}

		public override void OnEnter()
		{
			m_stateMachine.controller.CreateStateIndicator("Attack");
			m_stateMachine.controller.stateIndicator.GetComponent<StateIndicator>().SetIndicator(m_stateMachine.controller);
			m_stateMachine.controller.PlayAnimation("Attack");
			m_stateMachine.controller.m_sound.Play("Cry");
			m_stateMachine.controller.m_killBox.enabled = true;
		}

		public override void OnExit()
		{
			GameObject.Destroy(m_stateMachine.controller.stateIndicator);
			m_stateMachine.controller.m_killBox.enabled = false;
		}

		public override void OnUpdate()
		{
			if (initialized && CheckShots())
			{
				Transform target = m_stateMachine.controller.m_sight.CheckForTargets();
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

		public void Bombard(Vector2 spawnPoint)
		{
			Vector2 dir = m_stateMachine.controller.GetTargetDirection();
			dir.x -= (spawnPoint - ((Vector2)m_stateMachine.controller.transform.position + m_stateMachine.controller.m_boxCollider.offset)).x;

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

				shots[i] = GameObject.Instantiate(projectile, spawnPoint, Quaternion.identity, null);
				shots[i].GetComponent<Projectile>().ShootProjectile(velocity);
			}
			m_stateMachine.controller.m_killBox.enabled = true;
			m_stateMachine.controller.m_sound.Play("Shoot");
			initialized = true;
		}
	}
}
