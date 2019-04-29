using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class Checkpoint : MonoBehaviour 
	{
		public static System.Action Transfer;
		public Transform respawnPoint;
		Transform hud;
		GameObject popup;

		// Use this for initialization
		void Start () 
		{
			popup = Resources.Load<GameObject>("UI/CheckpointPopup");
			hud = GameObject.Find("Canvas").transform.GetChild(0);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			PlayerController player = other.GetComponent<PlayerController>();
			if (player && player.collectedEgg)
			{
				if (respawnPoint == null) player.SetRespawnPoint(transform.position);
				else player.SetRespawnPoint(respawnPoint.position);
				player.collectedEgg = false;
				Transfer();
				Instantiate(popup, hud);
				GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Teleporter Activated");
				Destroy(this);
			}
		}
	}
}
