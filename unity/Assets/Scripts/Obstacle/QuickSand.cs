using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	[RequireComponent(typeof(EdgeCollider2D))]
	public class QuickSand : Obstacle 
	{

		public float fallGravity = 1;
		float fallVelocity;
		static float initfallVelocity = 0.1f;
		float speedModifier;
		static float initSpeedMod = 0.6f;
		float initOffset;
		EdgeCollider2D edge;

		private void Start()
		{
			init();
			m_collider.isTrigger = true;
			edge = GetComponent<EdgeCollider2D>();
			initOffset = edge.offset.y;
			fallVelocity = initfallVelocity;
			speedModifier = initSpeedMod;
		}

		private void OnCollisionStay2D(Collision2D other)
		{
			PlayerController player = other.gameObject.GetComponent<PlayerController>();
			if (player)
			{
				edge.offset -= new Vector2(0, fallVelocity * Time.deltaTime);
				fallVelocity += fallGravity*Time.deltaTime;

				player.SetSpeedModifier(speedModifier);
				speedModifier = Mathf.Clamp(speedModifier - (fallGravity/3*Time.deltaTime), 0.2f, 1);
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
			fallVelocity = initfallVelocity;
			speedModifier = initSpeedMod;
		}

		public void UpdateGround(Vector2 playerFeet)
		{
			float y = Mathf.Clamp(playerFeet.y - transform.position.y, Mathf.NegativeInfinity, initOffset);
			Mathf.Clamp(y, Mathf.NegativeInfinity, initOffset);
			edge.offset = new Vector2(edge.offset.x,y);
		}
	}
}
