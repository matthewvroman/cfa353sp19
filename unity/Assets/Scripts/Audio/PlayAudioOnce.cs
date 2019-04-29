using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class PlayAudioOnce : MonoBehaviour 
	{
		SFXManager audio;
		bool initailized = false;

		private void Start()
		{
			audio = GetComponent<SFXManager>();
			Debug.Log("Created Use Once Audio");
		}

		private void Update()
		{
			if (!audio.IsPlaying())
			{
				if (initailized)
				{
					Debug.Log("Destroying Audio Source called " + gameObject.name);
					Destroy(gameObject);
				}
			}
			else if (!initailized) initailized = true;
		}
	}
}
