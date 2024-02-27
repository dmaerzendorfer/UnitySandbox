using System;
using _Generics.Scripts.Runtime;
using UnityEngine;

namespace _10_AudioManager.Scripts.Runtime
{
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        public Sound[] sounds;


        public override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            foreach (var s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();

                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;

                s.source.loop = s.loop;
                s.source.spatialBlend = s.spatialBlend;
                s.source.playOnAwake = s.playOnAwake;

                if (s.playOnAwake) s.source.Play();
            }
        }

        public void PlaySound(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning($"Sound with name {name} not found!");
                return;
            }

            s.source.Play();
        }
    }
}