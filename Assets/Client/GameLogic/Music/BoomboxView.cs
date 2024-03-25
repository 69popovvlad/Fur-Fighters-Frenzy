using Client.Network.Entities;
using DG.Tweening;
using FishNet.Object;
using UnityEngine;

namespace Client.GameLogic.Music
{
    public class BoomboxView : NetworkEntityView
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private ParticleSystem[] _particles;
        [SerializeField] private AudioClip[] _clips;

        [Header("Shake animation")]
        [SerializeField] private float _shakeDuration = 0.3f;
        [SerializeField] private float _shakeStrength = 10;
        [SerializeField] private int _shakeVibrato = 5;
        [SerializeField] private float _shakeRandomness = 10;

        private int _currentClipIndex = -1;
        private BoomboxEntity _entity;
        private bool _inShaking;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            if (!IsServerInitialized)
            {
                return;
            }

            NextStation();

            _entity = new BoomboxEntity(ObjectId.ToString());
            Initialize(_entity);
        }

        private void OnCollisionEnter(UnityEngine.Collision other)
        {
            if (_inShaking)
            {
                return;
            }

            _inShaking = true;
            
            NextStation();
        }

        private void NextStation()
        {
            if (++_currentClipIndex >= _clips.Length)
            {
                _currentClipIndex = -1;
            }

            NextStationToAllClients(_currentClipIndex);
        }

        [ObserversRpc(RunLocally = true, BufferLast = true)]
        private void NextStationToAllClients(int clipIndex)
        {
             transform.DOShakeRotation(_shakeDuration, _shakeStrength, _shakeVibrato, _shakeRandomness)
                .OnComplete(() =>
                {
                    _inShaking = false;
                    if (clipIndex < 0)
                    {
                        _audioSource.Stop();

                        for (int i = 0, iLen = _particles.Length; i < iLen; ++i)
                        {
                            _particles[i].Stop();
                        }
                        return;
                    }

                    _audioSource.Stop();
                    _audioSource.clip = _clips[clipIndex];
                    _audioSource.Play();

                    if (clipIndex > 0)
                    {
                        return;
                    }
                    
                    // Enable only for the first clip
                    for (int i = 0, iLen = _particles.Length; i < iLen; ++i)
                    {
                        _particles[i].Play();
                    }
                });
        }
    }
}