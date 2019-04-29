using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class KillBox : MonoBehaviour 
	{
		Enemy enemy;

		private void Start()
		{
			enemy = GetComponentInParent<Enemy>();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			PlayerController player = other.GetComponent<PlayerController>();
			if (player && !player.IsDead())
			{
				float dir = (player.GetPosition() - enemy.transform.position).x;
				dir = (dir > 0 ? 1 : 0);
				player.Died((int) dir);
			}
		}
	}
}
