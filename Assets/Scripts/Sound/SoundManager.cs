using System;
using UnityEngine;

namespace FiringRange
{
    [Serializable]
    public struct Sound
    {
        public AudioClip Clip;
        [Range(0, 100)]public float Volume;
    }

    public class SoundManager : MonoBehaviour
    {
        [SerializeField] Sound BackgroundMusic;
        public static SoundManager instance;
        public bool MusicIsOn
        {
            get => _musicIsOn;
            set
            {
                _musicIsOn = value;
                if (value)
                {
                    if (!_musicSource.isPlaying)
                    {
                        _musicSource.Play();
                    }
                }
                else
                {
                    if (_musicSource.isPlaying)
                    {
                        _musicSource.Stop();
                    }
                }
            }
        }

        public bool SfxIsOn
        {
            get => _sfxIsOn;
            set => _sfxIsOn = value;
        }


        private AudioSource _musicSource;
        private AudioSource _sfxSource;

        private bool _musicIsOn;
        private bool _sfxIsOn;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(instance);
            }

            AudioSource[] sources = GetComponents<AudioSource>();

            _musicSource = sources[0];
            _musicSource.clip = BackgroundMusic.Clip;
            _musicSource.loop = true;
            _musicSource.playOnAwake = true;
            _musicSource.volume = BackgroundMusic.Volume / 100;
            _musicSource.Play();

            _sfxSource = sources[1];
            _sfxSource.loop = false;
            _sfxSource.playOnAwake = false;
        }

        public void PlaySoundEffect(Sound sound)
        {
            //if (!SfxIsOn) return;

            _sfxSource.clip = sound.Clip;
            _sfxSource.volume = sound.Volume / 100;

            if (_sfxSource.isPlaying)
                _sfxSource.Stop();

            _sfxSource.Play();
        }
    }
}