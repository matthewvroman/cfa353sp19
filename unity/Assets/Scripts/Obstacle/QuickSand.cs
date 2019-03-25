using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	[RequireComponent(typeof(EdgeCollider2D))]
	public class QuickSand : Obstacle 
	{

		public float fallGravity = 0.5f;
		float fallVelocity = 0.1f;
		float initOffset;
		EdgeCollider2D edge;

		private void Start()
		{
			init();
			m_collider.isTrigger = true;
			edge = GetComponent<EdgeCollider2D>();
			initOffset = edge.offset.x;
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			PlayerController player = other.gameObject.GetComponent<PlayerController>();
			if (player)
			{
				player.SetSpeedModifier(0.5f);
			}
		}

		private void OnCollisionStay2D(Collision2D other)
		{
			if (other.gameObject.GetComponent<PlayerController>())
			{
				Debug.Log("Moving Down");
				edge.offset -= new Vector2(0, fallVelocity * Time.deltaTime);
				fallVelocity += fallGravity*Time.deltaTime;
			}
		}

		private void OnCollisionExit2D(Collision2D other)
		{
			PlayerController player = other.gameObject.GetComponent<PlayerController>();
			if (player)
			{
				player.SetSpeedModifier(1);
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.GetComponent<PlayerController>())
			{
				PlayerController.PlayerDied();
			}
		}

		public void ResetGround()
		{
			edge.offset = new Vector2(edge.offset.x, initOffset);
		}

		public void UpdateGround(Vector2 playerFeet)
		{
			float y = playerFeet.y - transform.position.y;
			Mathf.Clamp(y, Mathf.NegativeInfinity, initOffset);
			edge.offset = new Vector2(edge.offset.x,y);
		}
	}
}
