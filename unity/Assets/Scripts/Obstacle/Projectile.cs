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
			if (other.gameObject.GetComponent<PlayerController>())
			{
				PlayerController.PlayerDied();
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
