using Client.GameLogic.Aiming;
using Client.GameLogic.Inputs;
using Client.GameLogic.Inputs.Commands.Aiming;
using Core.Ioc;
using UnityEngine;

namespace Client.GameLogic.CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private AimingControl _aimingControl;
        [SerializeField] private float _smoothSpeedX = 1;
        [SerializeField] private float _smoothSpeedY = 0.5f;
        [SerializeField] private float _distance = 3.5f;
        [SerializeField] private float _maxVerticalAngle = 75;
        [SerializeField] private float _minVerticalAngle = 35;

        private Vector3 _offset;
        private float _currentAngleX;
        private float _currentAngleY;
        private InputBucket _inputBucket;

        private void Awake()
        {
            _inputBucket = Ioc.Instance.Get<InputBucket>();
            _inputBucket.Subscribe<AimInputCommand>(OnAimingCommand);
        }

        private void Start()
        {
            _offset = new Vector3(0f, _distance, 0f);
            _currentAngleX = Mathf.Clamp(_currentAngleX, _minVerticalAngle, _maxVerticalAngle);
        }

        private void OnDestroy()
        {
            _inputBucket.Unsubscribe<AimInputCommand>(OnAimingCommand);
        }

        public void SetTarget(Transform target, AimingControl aimingControl)
        {
            _target = target;
            _aimingControl = aimingControl;
            transform.position = target.position + _offset;
        }

        private void OnAimingCommand(AimInputCommand command)
        {
            if (_target == null)
            {
                return;
            }

            var inputX = command.AxisY;
            _currentAngleX += inputX * _smoothSpeedX;
            _currentAngleX = Mathf.Clamp(_currentAngleX, _minVerticalAngle, _maxVerticalAngle);

            var inputY = command.AxisX;
            _currentAngleY += inputY * _smoothSpeedY;
            _currentAngleY %= 360;

            var desiredPositionX = _target.position + Quaternion.Euler(_currentAngleX, _currentAngleY, 0) * _offset;
            transform.position = Vector3.Lerp(transform.position, desiredPositionX, 1);

            transform.LookAt(_target);

            if (_aimingControl == null)
            {
                return;
            }

            _aimingControl.SetAimAngle(_currentAngleX);
            _aimingControl.SetLookDirection(transform.forward);
        }
    }
}