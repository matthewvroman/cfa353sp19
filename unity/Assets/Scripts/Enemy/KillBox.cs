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
			if (other.GetComponent<PlayerController>())
			{
				PlayerController.PlayerDied();
			}
		}
	}
}
