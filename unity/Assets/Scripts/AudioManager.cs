using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
    public class AudioManager : MonoBehaviour
    {
        AudioManager m_instance;
        [SerializeField]
        Sound[] sounds;
        public Sound[] soundLibrary
        {
            get
            {
                return sounds;
            }
        }

        // Use this for initialization
        void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }

        public void PlaySound(string soundName)
        {
            foreach (Sound s in sounds)
            {
                if (s.name == soundName)
                {
                    s.source.Play();
                    return;
                }
            }
            Debug.Log("WARNING: Sound you tried to play was not found!");
        }
    }
}
