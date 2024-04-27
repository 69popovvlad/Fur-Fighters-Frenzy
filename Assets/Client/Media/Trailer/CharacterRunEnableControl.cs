using System.Linq;
using Client.GameLogic.Inputs;
using Client.GameLogic.Inputs.Commands.Movement;
using Core.Ioc;
using DG.Tweening;
using FishNet.Object;
using JetBrains.Annotations;
using UnityEngine;

namespace Client.Media.Trailer
{
    public class CharacterRunEnableControl : NetworkBehaviour
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private float _startDelay = 1;
        [SerializeField] private float _duration = 5;
        [SerializeField] private PathType _pathType = PathType.CatmullRom;
        [SerializeField] private AnimationCurve _animationEase;
        [SerializeField] private Transform[] _lookAtPoints;
        [SerializeField] private Transform[] _directionPoint;

        private InputBucket _inputBucket;
        private Transform _lookAtPoint;
        private bool _isRunningStage;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            if (IsServerInitialized)
            {
                Invoke(nameof(StartAnimation), _startDelay);
            }
        }

        private void StartAnimation()
        {
            _inputBucket = Ioc.Instance.Get<InputBucket>();
            var positions = _directionPoint.Select(p => p.position).ToArray();
            transform.DOPath(positions, _duration, _pathType).SetEase(_animationEase);
        }

        private void Update()
        {
            _camera.LookAt(_lookAtPoint);

            if (!_isRunningStage)
            {
                return;
            }

            var command = new MovementCommand(null, 0, 1);
            _inputBucket.Invoke(command);
        }

        [UsedImplicitly]
        public void SetRunCommand()
        {
            _isRunningStage = true;
        }

        [UsedImplicitly]
        public void LookAtPoint(int lookAtPointIndex)
        {
            _lookAtPoint = _lookAtPoints[lookAtPointIndex];
        }
    }
}