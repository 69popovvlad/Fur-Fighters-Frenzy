using System;
using UnityEngine;

namespace Client.Audio
{
    public class AudioPlayerView: MonoBehaviour
    {
        public event Action<AudioPlayerView> OnFreed;

        [SerializeField] private AudioSource _audioSource;
        
        private float _durationLeft;

        public float DurationLeft => _durationLeft;

        internal void RecalculateDurationLeftTime(float delta)
        {
            _durationLeft -= delta;
            if(_durationLeft <= 0)
            {
                StopPlay();
            }
        }

        internal void PlayClip(AudioClip audioClip, float volume)
        {
            _audioSource.clip = audioClip;
            _audioSource.volume = volume;
            _audioSource.Play();

            _durationLeft = audioClip.length;
        }

        private void StopPlay()
        {
            _audioSource.Stop();
            _audioSource.clip = null;
            OnFreed?.Invoke(this);
        }
    }
}