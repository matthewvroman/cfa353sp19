using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class SpawnerObstacle : Obstacle 
	{
		public GameObject[] obstacles;
		public Transform spawningPoint;
		public float spawnTime = 5;
		ParticleSystem[] particleSystems;
		float spawnTimer;
		int levelNum;

		public override void init()
		{
			base.init();
			levelNum = GameManager.instance.GetLevelNum();
			spawnTimer = Random.Range(0.1f, spawnTime);
			particleSystems = new ParticleSystem[transform.childCount];
			for (int i = 0; i < transform.childCount; i++)
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
				SpawnObstacle();
				spawnTimer = spawnTime;
				Play(false);
			}
			else if (spawnTimer <= 2 && !particleSystems[0].isPlaying)
			{
				Play(true);
			}
		}

		void Play(bool value)
		{
			foreach (ParticleSystem p in particleSystems)
			{
				if (value) p.Play();
				else p.Stop();
			}
		}

		void SpawnObstacle()
		{
			int random = Random.Range(0,obstacles.Length);
			Instantiate(obstacles[random], spawningPoint.position, Quaternion.Euler(0,0, Random.Range(0, 360)), null);
		}
	}
}
