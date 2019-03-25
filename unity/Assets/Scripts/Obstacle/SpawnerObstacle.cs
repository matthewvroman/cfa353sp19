using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class SpawnerObstacle : Obstacle 
	{
		public GameObject obstacle;
		public Transform spawningPoint;
		public float spawnTime = 5;
		ParticleSystem particleSystem;
		float spawnTimer;

		public override void init()
		{
			base.init();
			m_collider.isTrigger = true;
			spawnTimer = spawnTime;
			particleSystem = GetComponentInChildren<ParticleSystem>();
		}

		void Start()
		{
			init();
		}

		private void Update()
		{
			spawnTimer -= Time.deltaTime;
			if (spawnTimer <= 0)
			{
				Instantiate(obstacle, spawningPoint.position, Quaternion.identity, null);
				spawnTimer = spawnTime;
				particleSystem.Stop();
			}
			else if (spawnTimer <= 2 && !particleSystem.isPlaying)
			{
				particleSystem.Play();
			}
		}
	}
}
