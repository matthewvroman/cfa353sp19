using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class ScreamingObstacle : AlarmObstacle 
	{
		public float screamInterval = 3;
		float screamTimer;

		void Start () 
		{
			init();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			PlayerController player = other.GetComponent<PlayerController>();
			if (player != null && !player.IsCrouching())
			{
				AlertEnemies(alarmRange, transform.position);
				screamTimer = screamInterval;
			}
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			PlayerController player = other.GetComponent<PlayerController>();
			if (player != null)
			{
				if (!player.IsCrouching())
				{
					screamTimer -= Time.deltaTime;
					if (screamTimer <= 0)
					{
						AlertEnemies(alarmRange, transform.position);
						screamTimer = screamInterval;
					}
				}
				else
				{
					screamTimer = screamInterval;
				}
			}
		}
	}
}
