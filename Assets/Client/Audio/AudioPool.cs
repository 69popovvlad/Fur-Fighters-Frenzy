using System.Collections.Generic;
using UnityEngine;

namespace Client.Audio
{
    public class AudioPool: MonoBehaviour
    {
        private readonly Queue<AudioPlayerView> _readyToPlay = new Queue<AudioPlayerView>();
        private readonly List<AudioPlayerView> _inPlaying = new List<AudioPlayerView>();

        private AudioPlayerView _audioPlayerPrefab;
        private Transform _poolRoot;

        private void Update()
        {
            for(int i = _inPlaying.Count - 1; i >= 0; --i)
            {
                var player = _inPlaying[i];
                player.RecalculateDurationLeftTime(Time.deltaTime);

                if(player.DurationLeft > 0)
                {
                    return;
                }

                _inPlaying.RemoveAt(i);
            }
        }

        internal void Initialize(AudioPlayerView audioPlayerPrefab)
        {
            _audioPlayerPrefab = audioPlayerPrefab;

            _poolRoot = new GameObject("Disabled audios").transform;
            _poolRoot.SetParent(transform);
            _poolRoot.gameObject.SetActive(false);
        }

        internal void PlaySound(AudioClip audioClip, in Vector3 position, float volume)
        {
            AudioPlayerView audioPlayer;

            if (_readyToPlay.Count < 1)
            {
                audioPlayer = CreatePlayer();
                audioPlayer.OnFreed += OnPlayerFreed;
            }
            else
            {
                audioPlayer = _readyToPlay.Dequeue();
            }

            audioPlayer.transform.SetParent(null);
            audioPlayer.transform.position = position;
            audioPlayer.PlayClip(audioClip, volume);
            
            _inPlaying.Add(audioPlayer);
        }

        private AudioPlayerView CreatePlayer()
        {
            return Instantiate(_audioPlayerPrefab, _poolRoot);
        }

        private void OnPlayerFreed(AudioPlayerView audioPlayer)
        {
            audioPlayer.transform.SetParent(_poolRoot);
            _readyToPlay.Enqueue(audioPlayer);
        }
    }
}