using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class CrystalSpike : DeadlyObstacle
	{

		// Use this for initialization
		void Start () 
		{
			init();
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			KillPlayer(other.gameObject);
		}
	}
}
