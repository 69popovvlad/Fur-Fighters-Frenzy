using UnityEngine;
using Random = UnityEngine.Random;

namespace Client.GameLogic.Fans
{
    public class Fan3DControl : MonoBehaviour
    {
        private readonly int CheeringIndexHash = Animator.StringToHash("CheeringIndex");

        [SerializeField] private Animator _animator;
        [SerializeField] private int _animationCount;
        [SerializeField] private float _minDelay = 1;
        [SerializeField] private float _maxDelay = 5;

        private void Awake()
        {
            var randomDelay = Random.Range(_minDelay, _maxDelay);
            Invoke(nameof(StartAnimation), randomDelay);
        }

        private void StartAnimation()
        {
            var randomClip = Random.Range(0, _animationCount);
            _animator.SetInteger(CheeringIndexHash, randomClip);
            _animator.enabled = true;
        }
    }
}