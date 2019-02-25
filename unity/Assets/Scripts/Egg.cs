using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	[RequireComponent(typeof(CircleCollider2D))]
	[RequireComponent(typeof(SpriteRenderer))]
	public class Egg : MonoBehaviour 
	{
		public static System.Action<bool, Vector3> UpdateMapPosition;
		public static System.Action EggCollected;
		
		void Start () 
		{
			UpdateMapPosition(true, transform.position);
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Player"))
			{
				UpdateMapPosition(false, transform.position);
				EggCollected();
				Destroy(gameObject);
			}
		}
	}
}
