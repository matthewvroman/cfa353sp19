using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class CheckpointPopup : MonoBehaviour 
	{
		public float duration = 5;

		private void Update()
		{
			duration -= Time.deltaTime;
			if (duration <= 0) Destroy(gameObject);
		}
	}
}
