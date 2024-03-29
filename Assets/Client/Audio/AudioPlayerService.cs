using System.Collections.Generic;
using Core.Application;
using Core.Application.Events;
using UnityEngine;

namespace Client.Audio
{
    public class AudioPlayerService : IApplicationResource
    {
        private readonly Dictionary<string, List<AudioClip>> _clips = new Dictionary<string, List<AudioClip>>();

        private AudioPool _audioPool;

        public void Initialize(AudioPlayerView audioPlayerPrefab)
        {
            var poolInstance = new GameObject("Audio pool");
            _audioPool = poolInstance.AddComponent<AudioPool>();
            _audioPool.Initialize(audioPlayerPrefab);
        }

        /// <summary>
        /// Play clip by key
        /// </summary>
        /// <param name="position">Audio spawn position</param>
        /// <param name="key">Clip key</param>
        /// <param name="index">Specified clip index or -1 to play random clip</param>
        /// <param name="volume">Audio clip playing volume</param>
        public void PlayClip(in Vector3 position, string key, int index = -1, float volume = 0.5f)
        {
            if (!_clips.TryGetValue(key, out var list))
            {
                Debug.LogWarning($"Clip {key} doesn't exist");
                return;
            }

            var clipIndex = index;
            if (index < 0)
            {
                clipIndex = Random.Range(0, list.Count);
            }
            else
            {
                clipIndex = Mathf.Clamp(clipIndex, 0, list.Count - 1);
            }

            _audioPool.PlaySound(list[clipIndex], position, volume);
        }

        internal void RegisterClip(AudioClipInfo clipInfo)
        {
            if (!_clips.TryGetValue(clipInfo.Key, out var list))
            {
                list = _clips[clipInfo.Key] = new List<AudioClip>();
            }

            list.Add(clipInfo.Clip);
        }

        internal void UnregisterClip(AudioClipInfo clipInfo)
        {
            if (!_clips.TryGetValue(clipInfo.Key, out var list))
            {
                return;
            }

            list.Remove(clipInfo.Clip);
        }
    }
}