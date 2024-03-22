using Client.GameLogic.Inputs;
using UnityEngine;

namespace Client.GameLogic.Aiming
{
    public class AimingControl : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _aim;
        [SerializeField] private float _rotationSpeed = 1;
        [SerializeField] private float _radius = 1.5f;
        [SerializeField] private int _angleCorrection = 270;
        [SerializeField] private float _heightCorrection = 0.5f;

        private InputBucket _inputBucket;
        private Quaternion _lookRotation;
        private Vector3 _offset;

        private void Start()
        {
            _offset = new Vector3(0, _radius, 0);
            SetLookDirection(transform.forward);
        }

        private void Update()
        {
            _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, _lookRotation,
                Time.deltaTime * _rotationSpeed));
        }

        public void SetAimAngle(float angle)
        {
            Debug.Log(angle);
            var desiredPosition = transform.position -
                                  Quaternion.Euler(_angleCorrection - angle, _lookRotation.eulerAngles.y, 0) * _offset;
            desiredPosition.y += _heightCorrection;
            _aim.position = desiredPosition;
        }

        public void SetLookDirection(Vector3 direction)
        {
            var horizontalDirection = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
            var angleInHorizontal = Vector3.SignedAngle(Vector3.forward, horizontalDirection, Vector3.up);
            _lookRotation = Quaternion.Euler(0, angleInHorizontal, 0);
        }
    }
}