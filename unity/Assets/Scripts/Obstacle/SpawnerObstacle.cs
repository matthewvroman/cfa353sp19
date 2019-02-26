using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class SpawnerObstacle : Obstacle 
	{
		public GameObject obstacle;
		public Transform spawningPosition;
		public int numObstaclesToSpawn = 1;
		public bool randomizedSpawns = false;
		bool triggered = false;

		public override void init()
		{
			base.init();
			m_collider.isTrigger = true;
		}

		void Start()
		{
			init();
		}

		private void SpawnObstacles()
		{
			for (int i = 0; i < numObstaclesToSpawn; i++)
			{
				float random = 0;
				if (randomizedSpawns)
				{
					float pos = m_collider.bounds.extents.x;
					random = Random.Range(-pos, pos);
				}
				Instantiate(obstacle, spawningPosition.position + new Vector3(random,0,0), Quaternion.identity, null);
			}
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			if (other.GetComponent<PlayerController>() && !triggered)
			{
				triggered = true;
				SpawnObstacles();
				Destroy(gameObject);
			}
		}
	}
}
