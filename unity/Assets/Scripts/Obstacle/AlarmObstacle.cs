using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class AlarmObstacle : Obstacle
	{
		public float alarmRange = 5;

		public override void init()
		{
			base.init();
			m_collider.isTrigger = true;
		}

		// Use this for initialization
		void Start () 
		{
			init();
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			PlayerController player = other.GetComponent<PlayerController>();
			if (player != null)
			{
				float radius = alarmRange;
				if (player.IsCrouching())
				{
					radius /= 2;
				}
				Collider2D[] results = Physics2D.OverlapCircleAll(other.transform.position, radius, LayerMask.GetMask("Enemy"));
				for (int i = 0; i < results.Length; i++)
				{
					results[i].GetComponent<Enemy>().AlertEnemy(other.transform.position);
				}
			}
		}
	}
}
