using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class DeadlyObstacle : Obstacle
	{
		protected Rigidbody2D m_rigidbody;

		public override void init()
		{
			base.init();
			m_rigidbody = GetComponent<Rigidbody2D>();
		}

		protected virtual void KillPlayer(GameObject target)
		{
			PlayerController player = target.GetComponent<PlayerController>();
			if (player)
			{
				float dir = (player.GetPosition() - transform.position).x;
				dir = (dir > 0 ? 1 : 0);
				player.Died((int) dir);
			}
		}
		
	}
}
