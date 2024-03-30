using UnityEngine;

namespace Client.GameLogic.Movement
{
    public class MovemoentSoundControl : MonoBehaviour
    {
        [SerializeField] private MovementAnimationTriggerHandler _animationTrigger;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _aqrtSpeeStepsLimit;

        [Header("Clips")]
        [SerializeField] private AudioClip[] _grassSteps;

        [Header("Volume randomization settings")]
        [SerializeField] private float _volumeMin = 0.1f;
        [SerializeField] private float _volumeMax = 0.3f;

        [Header("Pitch randomization settings")]
        [SerializeField] private float _pitchMin = 0.8f;
        [SerializeField] private float _pitchMax = 1.2f;

        private void Awake()
        {
            _animationTrigger.OnStepTrigger += OnStepTrigger;
        }

        private void OnDestroy()
        {
            _animationTrigger.OnStepTrigger -= OnStepTrigger;
        }

        private void OnStepTrigger(int legIndex)
        {
            _audioSource.volume = Random.Range(_volumeMin, _volumeMax);
            _audioSource.pitch = Random.Range(_pitchMin, _pitchMax);
            _audioSource.PlayOneShot(_grassSteps[Random.Range(0, _grassSteps.Length)]);
        }

        // private void OnStepTrigger(int legIndex)
        // {
        //     if (_rigidbody.velocity.sqrMagnitude >= _aqrtSpeeStepsLimit)
        //     {
        //         if (!_audioSource.isPlaying)
        //         {
        //             _audioSource.volume = Random.Range(_volumeMin, _volumeMax);
        //             _audioSource.pitch = Random.Range(_pitchMin, _pitchMax);
        //             _audioSource.PlayOneShot(_grassSteps[Random.Range(0, _grassSteps.Length)]);
        //         }
        //     }
        //     else
        //     {
        //         _audioSource.Pause();
        //     }
        // }
    }
}