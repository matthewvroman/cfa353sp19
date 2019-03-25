using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class AlarmObstacle : Obstacle
	{
		public float alarmRange = 10;
		protected PlayerController player;
		protected string[] layers = {"Enemy","Juvenile"};

		public override void init()
		{
			base.init();
			m_collider.isTrigger = true;
		}

		protected void AlertEnemies(float radius, Vector2 position)
		{
			Collider2D[] results = Physics2D.OverlapCircleAll(position, radius, LayerMask.GetMask(layers));
			for (int i = 0; i < results.Length; i++)
			{
				results[i].GetComponent<Enemy>().AlertEnemy(position);
			}
		}
	}
}
