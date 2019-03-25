using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class Shockwave : MonoBehaviour 
	{
		public float speed = 3;
		public float duration = 3;
		int dir = 1;
		
		// Update is called once per frame
		void Update () 
		{
			transform.position += Vector3.right*dir*speed*Time.deltaTime;
			duration -= Time.deltaTime;
			if (duration <= 0)
			{
				Destroy(gameObject);
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			Debug.Log("Hit Something");
			PlayerController player = other.GetComponent<PlayerController>();
			if (player != null)
			{
				Vector2 direction = new Vector2(dir,0);
				player.Knockback(direction*speed);
				Destroy(gameObject);
			}
			if (other.CompareTag("Ground"))
			{
				Debug.Log("Hit Ground");
				Destroy(gameObject);
			}
		}

		public void setDirection(int moveDir)
		{
			dir = moveDir;
			if (dir < 0)
			{
				GetComponent<SpriteRenderer>().flipX = true;
			}
		}
	}
}
