using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class SpawnerObstacle : Obstacle 
	{
		public GameObject obstacle;
		SpriteRenderer obstacleSprite;
		public Transform spawningPoint;
		public float spawnTime = 5;
		ParticleSystem[] particleSystems;
		float spawnTimer;

		public override void init()
		{
			base.init();
			m_collider.isTrigger = true;
			spawnTimer = spawnTime;
			obstacleSprite = obstacle.GetComponent<SpriteRenderer>();
			particleSystems = new ParticleSystem[transform.childCount];
			for(int i = 0; i < transform.childCount; i++)
			{
				particleSystems[i] = transform.GetChild(i).GetComponent<ParticleSystem>();
			}
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
				obstacleSprite.sprite = Resources.Load<Sprite>("Sprites/Obstacles/S Rock " + Random.Range(1, 4));
				Instantiate(obstacle, spawningPoint.position, Quaternion.identity, null);
				spawnTimer = spawnTime;
				Stop();
			}
			else if (spawnTimer <= 2 && !particleSystems[0].isPlaying)
			{
				Play();
			}
		}

		public void Play()
		{
			foreach(ParticleSystem p in particleSystems)
			{
				p.Play();
			}
		}

		public void Stop()
		{
			foreach(ParticleSystem p in particleSystems)
			{
				p.Stop();
			}
		}
	}
}
