﻿using System;
using UnityEngine;

namespace _10_AudioManager.Scripts.Runtime
{
    [Serializable]
    public class Sound
    {
        public string name;

        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 0.5f;

        [Range(.1f, 3f)]
        public float pitch = 1f;

        [Range(0f, 1f)]
        public float spatialBlend = 0f;

        public bool loop = false;
        public bool playOnAwake = false;

        [HideInInspector]
        public AudioSource source;
    }
}