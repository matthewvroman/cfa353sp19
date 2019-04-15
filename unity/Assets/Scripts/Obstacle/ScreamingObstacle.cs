using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class ScreamingObstacle : AlarmObstacle 
	{
		SpriteRenderer renderer;
		public float screamInterval = 3;
		public string spriteName = "Screamer";
		AudioSource sound;
		float screamTimer;

		void Start () 
		{
			init();
			renderer = GetComponent<SpriteRenderer>();
			renderer.sprite = Resources.Load<Sprite>("Sprites/Obstacles/" + spriteName + " 1");
			sound = GetComponentInChildren<AudioSource>();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			PlayerController player = other.GetComponent<PlayerController>();
			if (player != null && !player.IsCrouching())
			{
				StartCoroutine("Scream");
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
						StartCoroutine("Scream");
					}
				}
				else
				{
					screamTimer = screamInterval;
				}
			}
		}

		IEnumerator Scream()
		{
			renderer.sprite = Resources.Load<Sprite>("Sprites/Obstacles/" + spriteName + " 2");
			sound.Play();
			AlertEnemies(alarmRange, transform.position);
			screamTimer = screamInterval;
			yield return new WaitForSeconds(1f);
			renderer.sprite = Resources.Load<Sprite>("Sprites/Obstacles/" + spriteName + " 1");
		}
	}
}
