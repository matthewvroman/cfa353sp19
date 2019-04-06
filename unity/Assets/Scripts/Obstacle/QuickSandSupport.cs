using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class QuickSandSupport : Obstacle
	{
		QuickSand quickSand;
		CircleCollider2D circle;
		Rigidbody2D body;

		private void Start()
		{
			init();
			quickSand = transform.parent.GetComponent<QuickSand>();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.GetComponent<PlayerController>())
			{
				circle = other.GetComponent<CircleCollider2D>();
				body = other.GetComponent<Rigidbody2D>();
			}
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			PlayerController player = other.GetComponent<PlayerController>();
			if (player)
			{
				if (!body)
				{
					body = other.GetComponent<Rigidbody2D>();
				}
				if (!circle)
				{
					circle = other.GetComponent<CircleCollider2D>();
				}

				if (player.IsJumping() && body.velocity.y <= 0)
				{
					Vector2 pos = (circle.offset + (Vector2)circle.transform.position) - new Vector2(0,circle.radius);
					quickSand.UpdateGround(pos);
				}
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			PlayerController player = other.GetComponent<PlayerController>();
			if (player)
			{
				player.SetSpeedModifier(1);
				quickSand.ResetGround();
				circle = null;
				body = null;
			}
		}
	}
}
