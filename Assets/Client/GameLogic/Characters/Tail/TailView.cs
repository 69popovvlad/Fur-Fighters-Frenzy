using UnityEngine;

namespace Client.GameLogic.Characters.Tail
{
    public class TailView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _tailCentre;
        [SerializeField] private Transform _tailAim;
        [SerializeField] private float _threshold = 1;
        [SerializeField] private float _maxDistanceFromCentre = 2f;
        [SerializeField] private float _sensitivity = 0.5f;
        [SerializeField] private float _smoothSpeed = 1;
        [SerializeField] private float _height;

        private Vector3 _lastAimPosition;

        private void Start() {
            _lastAimPosition = _tailAim.position;
        }

        private void FixedUpdate()
        {
            var velocity = _rigidbody.velocity;
            var playerVelocity = new Vector3(velocity.x, 0, velocity.z);

            if (playerVelocity.magnitude > _threshold)
            {
                var desiredPosition = _tailCentre.position - playerVelocity * _sensitivity;
                desiredPosition.y = _height;
                
                desiredPosition = _tailCentre.position + Vector3.ClampMagnitude(desiredPosition - _tailCentre.position, _maxDistanceFromCentre);
                _tailAim.position = _lastAimPosition = Vector3.Lerp(_lastAimPosition, desiredPosition, Time.fixedDeltaTime * _smoothSpeed);
            }
            else
            {
                var desiredPosition = _tailCentre.position;
                desiredPosition.y = _height;
                _tailAim.position = _lastAimPosition = Vector3.Lerp(_lastAimPosition, desiredPosition, Time.fixedDeltaTime * _smoothSpeed);
            }
        }
    }
}