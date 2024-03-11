using Client.GameLogic.Inputs;
using Client.GameLogic.Inputs.Commands.Movement;
using Core.Ioc;
using UnityEngine;

namespace Client.GameLogic.Movement
{
    public class MovementControl : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody _rigidbody;

        [Header("Speeds")]
        [SerializeField] private float _forwardSpeed = 0.05f;
        [SerializeField] private float _backwardSpeed = 0.025f;
        [SerializeField] private float _sideSpeed = 0.025f;

        [Header("Animation")]
        [SerializeField] private float _animationStoppingSpeed = 1;


        private readonly int XSpeedHash = Animator.StringToHash("XSpeed");
        private readonly int ZSpeedHash = Animator.StringToHash("ZSpeed");

        private InputBucket _inputBucket;

        private void Awake()
        {
            _inputBucket = Ioc.Instance.Get<InputBucket>();
            _inputBucket.Subscribe<MovementCommand>(OnMovementCommand);
        }

        private void OnDestroy()
        {
            _inputBucket.Unsubscribe<MovementCommand>(OnMovementCommand);
        }

        private void Update()
        {
            var delta = Time.deltaTime / _animationStoppingSpeed;
            ResetAnimationFloat(XSpeedHash, delta);
            ResetAnimationFloat(ZSpeedHash, delta);
        }

        private void OnMovementCommand(MovementCommand command)
        {
            _animator.SetFloat(XSpeedHash, command.XSpeed);
            _animator.SetFloat(ZSpeedHash, command.ZSpeed);

            var vector = new Vector3(command.XSpeed, 0, command.ZSpeed).normalized;
            vector.z *= vector.z > 0 ? _forwardSpeed : _backwardSpeed;
            vector.x *= _sideSpeed;

            var targetPosition = _rigidbody.position + vector;
            _rigidbody.MovePosition(targetPosition);
        }

        private void ResetAnimationFloat(int id, float delta)
        {
            var value = _animator.GetFloat(id);
            var sign = Mathf.Sign(value);

            value = Mathf.Abs(value) - delta;
            if (value <= 0)
            {
                _animator.SetFloat(id, 0);
                return;
            }

            _animator.SetFloat(id, value * sign);
        }
    }
}