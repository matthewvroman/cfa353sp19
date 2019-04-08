using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class Bait : MonoBehaviour 
	{
		float bait = 1;

		private void Start()
		{
			GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Bait/Berry" + GameManager.instance.GetLevelNum());
		}

		public void EatBait(float eaten)
		{
			Debug.Log("Bait being Eaten");
			bait -= eaten;
			if (bait <= 0)
			{
				Destroy(gameObject);
			}
		}
	}
}
