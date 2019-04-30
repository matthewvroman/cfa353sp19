using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(SpriteRenderer))]
	public class Goal : MonoBehaviour 
	{

		public EggManager eggManager;
		public GameScreenManager screenManager;

		void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Player"))
			{
				eggManager.TransferEggs();
				screenManager.LevelComplete();
			}
		}
	}
}
