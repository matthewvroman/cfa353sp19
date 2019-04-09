using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class FallenLeaves : AlarmObstacle
	{

		// Use this for initialization
		void Start () 
		{
			init();
			m_spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Obstacles/leaves " + GameManager.instance.GetLevelNum());
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
			if (player != null && other.CompareTag(player.tag) && !player.IsCrouching() && Mathf.Abs(player.m_rigidbody.velocity.x) > 0)
			{
				AlertEnemies(alarmRange, other.transform.position);
			}
		}
	}
}
