using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class Egg : MonoBehaviour 
	{
		public static System.Action<int> EggCollected;

		void OnTriggerEnter2D(Collider2D other)
		{
			PlayerController player = other.GetComponent<PlayerController>();
			if (player)
			{
				if (EggCollected != null) EggCollected(gameObject.GetInstanceID());
				player.collectedEgg = true;
				gameObject.SetActive(false);
			}
		}
	}
}
