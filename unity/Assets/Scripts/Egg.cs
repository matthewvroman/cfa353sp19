using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class Egg : MonoBehaviour 
	{
		public static System.Action EggCollected;

		void OnTriggerEnter2D(Collider2D other)
		{
			if (other.GetComponent<PlayerController>())
			{
				EggCollected();
				Destroy(gameObject);
			}
		}
	}
}
