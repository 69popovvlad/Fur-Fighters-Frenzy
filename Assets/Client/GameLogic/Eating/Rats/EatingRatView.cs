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
        [SerializeField] private EatingItemView _eatingItemView;
        [SerializeField] private Rigidbody _rigidbody;

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

        private void OnTaken()
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        private void OnDropped()
        {
            if (_pathTween == null)
            {
                return;
            }

            _pathTween.Kill();
            _pathTween = null;

            _rigidbody.constraints = RigidbodyConstraints.None;
        }

        public void InitializePath(Transform[] points, float duration)
        {
            if(_pathTween != null)
            {
                return;
            }

            var pointsPositions = points.Select(p => p.position).ToArray();
            _pathTween = transform.DOPath(pointsPositions, duration)
            .SetLookAt(0.01f)
            .OnComplete(() =>
            {
                _pathTween = null;
                Debug.Log("Rat finished");
            });
        }
    }
}