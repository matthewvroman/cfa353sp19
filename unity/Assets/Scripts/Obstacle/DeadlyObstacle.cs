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

		protected virtual void KillPlayer(GameObject player)
		{
			if (player.CompareTag("Player"))
			{
				PlayerController.PlayerDied();
			}
		}
		
	}
}
