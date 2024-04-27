using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using FishNet.Object;
using UnityEngine;

namespace Client.GameLogic.Eating.Rats
{
    public class EatingRatView : NetworkBehaviour
    {
        private readonly int IdleHash = Animator.StringToHash("Idle");

        [SerializeField] private EatingItemView _eatingItemView;
        [SerializeField] private Animator _animator;

        private TweenerCore<Vector3, Path, PathOptions> _pathTween;

        private void Awake()
        {
            _eatingItemView.OnTaken += OnTaken;
            _eatingItemView.OnDropped += OnDropped;
        }

        private void OnDestroy()
        {
            _eatingItemView.OnTaken -= OnTaken;
            _eatingItemView.OnDropped -= OnDropped;
        }

        public void InitializePath(Transform[] points, float duration, PathType pathType)
        {
            if (_pathTween != null)
            {
                return;
            }

            var pointsPositions = points.Select(p => p.position).ToArray();
            _pathTween = transform.DOPath(pointsPositions, duration, pathType)
            .SetLookAt(0.01f)
            .OnComplete(() =>
            {
                Despawn();
            });
        }

        private void OnTaken()
        {
            _animator.SetBool(IdleHash, true);
            StopPathFollowing();
        }

        private void OnDropped()
        {
            StopPathFollowing();
        }

        private void StopPathFollowing()
        {
            if (_pathTween == null)
            {
                return;
            }

            _pathTween.Kill();
            _pathTween = null;
        }
    }
}