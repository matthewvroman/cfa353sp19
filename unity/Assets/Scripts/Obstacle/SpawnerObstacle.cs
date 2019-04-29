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
		SFXManager audio;
		float spawnTimer;

		public override void init()
		{
			base.init();
			spawnTimer = Random.Range(0.1f, spawnTime);
			audio = GetComponent<SFXManager>();
			particleSystems = new ParticleSystem[transform.childCount];
			for (int i = 0; i < transform.childCount; i++)
			{
				particleSystems[i] = transform.GetChild(i).GetComponent<ParticleSystem>();
			}
		}

		private void OnEnable()
		{
			GameScreenManager.RestartLevel += ResetSpawner;
		}

		private void OnDisable()
		{
			GameScreenManager.RestartLevel -= ResetSpawner;
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
				audio.Stop("Rumbling");
			}
			else if (spawnTimer <= 2 && !particleSystems[0].isPlaying)
			{
				Play(true);
				audio.Play("Rumbling");
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

		private void ResetSpawner()
		{
			spawnTimer = Random.Range(0.1f, spawnTime);
		}
	}
}
