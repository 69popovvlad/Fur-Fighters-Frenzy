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
        [SerializeField] private float _upLimit = 45;
        [SerializeField] private float _downLimit = 45;

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
            // Get the character's position and the mouse pointer position
            var transformPosition = transform.position;
            var worldPosition = new Vector3(command.XPosition, command.YPosition, command.ZPosition);
            var direction = worldPosition - transformPosition;

            // We are looking for the projection of the direction onto a plane parallel to the ground
            var horizontalDirection = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
            // Determine the angle between the horizontal direction and the direction to the pointer
            var angleInHorizontal = Vector3.SignedAngle(Vector3.forward, horizontalDirection, Vector3.up);
            _lookRotation = Quaternion.Euler(0, angleInHorizontal, 0);

            var distance = Vector3.Distance(worldPosition, transformPosition);
            var height = _downLimit + (_upLimit - _downLimit) * distance / _radius;

            var aimPosition = _aim.position;
            aimPosition.y = height;
            _aim.position = aimPosition;
            
            var _aimRotation = Quaternion.Euler(0, _lookRotation.eulerAngles.y, 0);
            _aim.rotation = _aimRotation;
        }
    }
}