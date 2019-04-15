using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class SFXManager : MonoBehaviour 
	{
		[SerializeField]
        Sound[] sounds;

		private void Start()
		{
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.spatialBlend = 0;
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.mute = s.muted;
				s.source.playOnAwake = false;
            }
		}

		public void Play(string soundName)
        {
            foreach (Sound s in sounds)
            {
                if (s.name == soundName)
                {
                    if (!s.source.isPlaying) s.source.Play();
                    return;
                }
            }
            Debug.Log("WARNING: Sound you tried to play was not found!");
        }

        public void Stop(string soundName)
        {
            foreach (Sound s in sounds)
            {
                if (s.name == soundName)
                {
                    if (s.source.isPlaying) s.source.Stop();
                    return;
                }
            }
            Debug.Log("WARNING: Sound you tried to play was not found!");
        }

        public void StopAll()
        {
            foreach (Sound s in sounds)
            {
                if (s.source.isPlaying) s.source.Stop();
            }
        }
	}
}
