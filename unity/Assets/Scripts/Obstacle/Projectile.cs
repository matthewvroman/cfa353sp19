using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class Projectile : MonoBehaviour 
	{
		public float shotTimer = 5;

		private void Update()
		{
			shotTimer -= Time.deltaTime;
			if (shotTimer <= 0)
			{
				Destroy(gameObject);
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			PlayerController player = other.GetComponent<PlayerController>();
			if (player)
			{
				float dir = (player.GetPosition() - transform.position).x;
				dir = (dir > 0 ? 1 : 0);
				player.Died((int)dir);
				Destroy(gameObject);
			}
			if (other.gameObject.CompareTag("Ground"))
			{
				Destroy(gameObject);
			}	
		}

		public void ShootProjectile(Vector2 velocity, bool useGravity = true)
		{
			Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
			rigidbody.gravityScale = useGravity ? 1 : 0;
			rigidbody.velocity = velocity;
		}
	}
}
