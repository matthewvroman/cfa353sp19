using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Bradley.AlienArk
{
    [System.Serializable]
    public class Sound
    {
        public string name;

        public AudioClip clip;

        [Range(0f,1f)]
        public float volume;
        [Range(0f, 3f)]
        public float pitch;

        public bool loop = false;
        public bool muted = false;

        [HideInInspector]
        public AudioSource source;
    }
}
