using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class AcidicWater : DeadlyObstacle
	{

		void Start () 
		{
			init();
			m_rigidbody.isKinematic = true;
			m_collider.isTrigger = true;	
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			PlayerController player = other.GetComponent<PlayerController>();
			if (player)
			{
				float dir = (player.GetPosition() - transform.position).x;
				dir = (dir > 0 ? 1 : 0);
				player.Died((int)dir);
			}
		}
	}
}
