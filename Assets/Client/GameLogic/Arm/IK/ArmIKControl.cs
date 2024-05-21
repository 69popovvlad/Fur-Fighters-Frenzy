using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Client.GameLogic.Arm.IK
{
    public class ArmIKControl : MonoBehaviour
    {
        public event Action OnTargetReached;
        public event Action OnReturned;
        public event Action OnStarted;

        [SerializeField] private ChainIKConstraint _armIK;
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private float _duration = 0.2f;
        [SerializeField] private float _comebackDuration = 0.3f;

        private bool _inAction;
        private float _animationT;

        public bool InAction => _inAction;
        public float AnimationT => _animationT;

        private void Update()
        {
            if (!_inAction)
            {
                CalculateReturnAnimation();
                return;
            }

            CalculateToTargetAnimation();
        }

        public void Play(bool withWeightReset = false)
        {
            _animationT = withWeightReset ? 0 : _animationT;
            _inAction = true;

            OnStarted?.Invoke();
        }

        public void ForceReturn()
        {
            _inAction = false;
        }

        private void CalculateToTargetAnimation()
        {
            if (_animationT >= 1)
            {
                OnTargetReached?.Invoke();
                _inAction = false;
                return;
            }

            _animationT += Time.deltaTime / _duration;
            _armIK.weight = _animationCurve.Evaluate(_animationT);
        }

        private void CalculateReturnAnimation()
        {
            if (_animationT <= 0)
            {
                return;
            }

            _animationT -= Time.deltaTime / _comebackDuration;
            _armIK.weight = _animationCurve.Evaluate(_animationT);

            if (_animationT > 0)
            {
                return;
            }

            OnReturned?.Invoke();
        }
    }
}