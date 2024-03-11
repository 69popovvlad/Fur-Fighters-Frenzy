using Client.GameLogic.Inputs;
using Client.GameLogic.Inputs.Commands.Aiming;
using Core.Ioc;
using UnityEngine;

namespace Client.GameLogic.Aiming
{
    public class AimingControl : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _aim;
        [SerializeField] private float _radius = 1.5f;
        [SerializeField] private float _rotationSpeed = 1;

        private InputBucket _inputBucket;
        private Quaternion _lookRotation;

        private void Awake()
        {
            _inputBucket = Ioc.Instance.Get<InputBucket>();
            _inputBucket.Subscribe<AimInputCommand>(OnAimingCommand);
        }

        private void OnDestroy()
        {
            _inputBucket.Unsubscribe<AimInputCommand>(OnAimingCommand);
        }

        private void Update()
        {
            _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, _lookRotation, Time.deltaTime * _rotationSpeed));
        }

        private void OnAimingCommand(AimInputCommand command)
        {
            var transformPosition = transform.position;

            var worldPosition = new Vector3(command.XPosition, command.YPosition, command.ZPosition);
            var direction = worldPosition - transformPosition;

            _aim.position = transformPosition + direction.normalized * _radius;

            direction.y = 0;
            if (direction == Vector3.zero)
            {
                return;
            }

            _lookRotation = Quaternion.LookRotation(direction);
        }
    }
}