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
			if (other.GetComponent<PlayerController>())
			{
				PlayerController.PlayerDied();
			}
		}
	}
}
