using UnityEngine;
using Random = UnityEngine.Random;

namespace Client.GameLogic.Fans
{
    public class FanControl : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite[] _fansSprites;

        [Header("Animation")]
        [SerializeField] private Animator _animator;
        [SerializeField] private float _minDelay = 1;
        [SerializeField] private float _maxDelay = 5;
        [SerializeField] private AnimationClip[] _clips;

        private void Awake()
        {
            var randomSprite = _fansSprites[Random.Range(0, _fansSprites.Length)];
            _spriteRenderer.sprite = randomSprite;

            var randomDelay = Random.Range(_minDelay, _maxDelay);
            Invoke(nameof(StartAnimation), randomDelay);
        }

        private void StartAnimation()
        {
            var randomClip = _clips[Random.Range(0, _clips.Length)];
            _animator.Play(randomClip.name);
        }
    }
}