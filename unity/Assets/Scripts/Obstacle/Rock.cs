using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class Rock : DeadlyObstacle 
	{
		float timer = 5;
		GameObject rockBreakAudio;
		// Use this for initialization
		void Start () 
		{
			init();
			rockBreakAudio = Resources.Load<GameObject>("Audio/Rock");
		}
		
		// Update is called once per frame
		void Update () 
		{
			transform.rotation = Quaternion.Euler(0,0, transform.rotation.eulerAngles.z + 30*Time.deltaTime);
			timer -= Time.deltaTime;
			if (timer <= 0)
			{
				Break();
			}
		}

		void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.CompareTag("Ground"))
			{
				Break();
				return;
			}
			PlayerController player = other.gameObject.GetComponent<PlayerController>();
			if (player && !player.IsDead())
			{
				float dir = (player.GetPosition() - transform.position).x;
				dir = (dir > 0 ? 1 : 0);
				player.Died((int) dir);
				Break();
			}
		}

		private void Break()
		{
			Instantiate(rockBreakAudio, transform.position, Quaternion.identity, null);
			Destroy(gameObject);
		}
	}
}
