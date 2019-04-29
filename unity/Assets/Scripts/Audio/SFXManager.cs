using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	public class SFXManager : MonoBehaviour 
	{
        [Range(1, 25f)]
        public float maxDistance = 15f;
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
				s.source.playOnAwake = s.playOnAwake;
                s.source.maxDistance = maxDistance;
                s.source.rolloffMode = AudioRolloffMode.Linear;
                s.source.spatialBlend = 1;
                if (s.playOnAwake) s.source.Play();
            }
		}

		public void Play(string soundName)
        {
            foreach (Sound s in sounds)
            {
                if (s.name == soundName)
                {
                    if (!s.source.isPlaying || !s.loop) s.source.Play();
                    return;
                }
            }
            Debug.LogWarning("WARNING: Sound you tried to play was not found!");
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
            Debug.LogWarning("WARNING: Sound you tried to stop was not found!");
        }

        public void StopAll()
        {
            foreach (Sound s in sounds)
            {
                if (s.source.isPlaying) s.source.Stop();
            }
        }

        public bool IsPlaying(string name)
        {
            foreach (Sound s in sounds)
            {
                if (s.name == name) return s.source.isPlaying;
            }
            return false;
        }

        public bool IsPlaying()
        {
            return sounds[0].source.isPlaying;
        }
	}
}
