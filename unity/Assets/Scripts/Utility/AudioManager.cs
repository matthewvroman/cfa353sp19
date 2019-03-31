using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
    public class AudioManager : MonoBehaviour
    {
        static AudioManager m_instance;
        public static AudioManager Instance
        {
            get 
            {
                if (m_instance == null)
                {
                    GameObject prefabs = Resources.Load<GameObject>("Prefabs/Utility/AudioManager");
                    GameObject gameObject = Instantiate(prefabs, null);
                    m_instance = gameObject.GetComponent<AudioManager>();
                }
                return m_instance;
            }
        }
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
                return;
            }

            DontDestroyOnLoad(gameObject);

            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.spatialBlend = 0;
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.mute = s.muted;
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
