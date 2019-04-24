using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(SpriteRenderer))]
	public class Goal : MonoBehaviour 
	{
		public static System.Action LevelComplete;

		void OnEnable()
		{
			Egg.EggCollected += ActivateGoal;
		}

		void OnDisable()
		{
			Egg.EggCollected -= ActivateGoal;
		}

		// Use this for initialization
		void Start () 
		{
			GetComponent<BoxCollider2D>().enabled = false;
			GetComponent<SpriteRenderer>().enabled = false;
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Player"))
			{
				GameManager.instance.CompletedLevel();
				LevelComplete();
			}
		}

		private void ActivateGoal()
		{
			GetComponent<BoxCollider2D>().enabled = true;
			GetComponent<SpriteRenderer>().enabled = true;
		}
	}
}
