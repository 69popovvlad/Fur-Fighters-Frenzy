using Client.GameLogic.Inputs;
using Client.GameLogic.Inputs.Commands.Movement;
using Core.Ioc;
using UnityEngine;

namespace Client.GameLogic.Movement
{
    public class MovementControl : InputListenerNetworkComponentBase
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody _rigidbody;

        [Header("Speeds")]
        [SerializeField] private float _forwardSpeed = 0.05f;
        [SerializeField] private float _backwardSpeed = 0.025f;
        [SerializeField] private float _sideSpeed = 0.025f;
        [SerializeField] private float _maxUpwardForce = 1f;

        [Header("Animation")]
        [SerializeField] private float _animationStoppingSpeed = 1;

        [Header("Kick")]
        [SerializeField] private float _kickImpulseMultiplier = 15;


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
            UnsubscribeFromInputs();
        }

        private void Update()
        {
            var delta = Time.deltaTime / _animationStoppingSpeed;
            ResetAnimationFloat(XSpeedHash, delta);
            ResetAnimationFloat(ZSpeedHash, delta);
        }

        public override void InputsInitialize(bool isOwner)
        {
            if (isOwner)
            {
                return;
            }

            enabled = false;
            UnsubscribeFromInputs();
        }

        /// <summary>
        /// Server only
        /// </summary>
        public void AddKickImpulse(Vector3 direction, float power)
        {
            _rigidbody.AddForce(direction.normalized * power * _kickImpulseMultiplier, ForceMode.Impulse);
        }

        private void UnsubscribeFromInputs()
        {
            if (_inputBucket == null)
            {
                return;
            }

            _inputBucket.Unsubscribe<MovementCommand>(OnMovementCommand);
        }

        private void OnMovementCommand(MovementCommand command)
        {
            _animator.SetFloat(XSpeedHash, command.XSpeed);
            _animator.SetFloat(ZSpeedHash, command.ZSpeed);

            var vector = new Vector3(command.XSpeed, 0, command.ZSpeed).normalized;
            vector.z *= vector.z > 0 ? _forwardSpeed : _backwardSpeed;
            vector.x *= _sideSpeed;

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f))
            {
                var surfaceNormal = hit.normal;
                var upwardForce = Mathf.Lerp(0, _maxUpwardForce, 1 - Vector3.Dot(Vector3.up, surfaceNormal));
                vector.y += upwardForce;
            }

            _rigidbody.AddForce(transform.TransformDirection(vector) * Time.deltaTime);
        }

        private void Foo(MovementCommand command)
        {
            _animator.SetFloat(XSpeedHash, command.XSpeed);
            _animator.SetFloat(ZSpeedHash, command.ZSpeed);

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f))
            {
                var surfaceNormal = hit.normal;
                var upDirection = Vector3.Cross(transform.right, surfaceNormal);

                var moveDirectionAlongSurface = Vector3.Cross(surfaceNormal, -transform.forward);

                var force = (moveDirectionAlongSurface * _forwardSpeed * command.ZSpeed) + (transform.right * _sideSpeed * command.XSpeed);
                _rigidbody.AddForce(force * Time.deltaTime, ForceMode.VelocityChange);

                if (surfaceNormal != Vector3.up)
                {
                    var upwardsForce = upDirection * _forwardSpeed * command.ZSpeed;
                    _rigidbody.AddForce(upwardsForce * Time.deltaTime, ForceMode.VelocityChange);
                }
            }
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