using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class AlarmObstacle : Obstacle
	{
		public float alarmRange = 10;
		private PlayerController player;
		string[] layers = {"Enemy","Juvenile"};

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
			PlayerController temp = other.GetComponent<PlayerController>();
			if (temp != null && !temp.IsCrouching())
			{
				player = temp;
				AlertEnemies(alarmRange, other.transform.position);
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			PlayerController temp = other.GetComponent<PlayerController>();
			if (temp != null)
			{
				player = null;
			}
		}

		private void OnTriggerStay2D(Collider2D other) 
		{
			if (player != null && other.CompareTag(player.tag) && !player.IsCrouching() && Mathf.Abs(player.Rigidbody.velocity.x) > 0)
			{
				AlertEnemies(alarmRange, other.transform.position);
			}
		}

		private void AlertEnemies(float radius, Vector2 position)
		{
			Collider2D[] results = Physics2D.OverlapCircleAll(position, radius, LayerMask.GetMask(layers));
			for (int i = 0; i < results.Length; i++)
			{
				results[i].GetComponent<Enemy>().AlertEnemy(position);
			}
		}
	}
}
