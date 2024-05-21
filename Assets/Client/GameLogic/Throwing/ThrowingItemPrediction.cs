using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Client.GameLogic.Throwing
{
    public class ThrowingItemPrediction : MonoBehaviour
    {
        [SerializeField] private float _predictionDurationLimit = 1;
        [SerializeField] private float _predictionTimeStepInterval = 1;
        [SerializeField] private int _predictionStepsLimit = 20;
        [SerializeField] private Transform _origin;
        [SerializeField] private ThrowingArmControl _throwingArm;
        [SerializeField] private LineRenderer _lineRenderer;

        [Header("Arm length")]
        [SerializeField] private Transform[] _armBones;
        [SerializeField] private float _armLength = 20;

        [Header("Spine bending correction")]
        [SerializeField] private ChainIKConstraint _spineChainIK;
        [SerializeField] private Transform[] _spineBones;

        private void Start()
        {
            enabled = false;

            CalculateArmLenght();
        }

        private void FixedUpdate()
        {
            if (!_throwingArm.HasItem || _throwingArm.Item.IsThrowing)
            {
                return;
            }

            var predictionPoints = CalculatePrediction();
            _lineRenderer.positionCount = predictionPoints.Length;
            _lineRenderer.SetPositions(predictionPoints);
        }

        public void Show(bool enabled)
        {
            this.enabled = enabled;
            gameObject.SetActive(enabled);
        }

        private Vector3[] CalculatePrediction()
        {
            if (_throwingArm.Item is not ThrowingItemView throwingItemView)
            {
                return Array.Empty<Vector3>();
            }

            var spineBonePosition = _armBones[0].position;
            var predictedArmPosition = spineBonePosition + (_origin.position - spineBonePosition).normalized * _armLength;

            var itemParentPosition = _throwingArm.ItemParent.position;
            var toItemParentDirection = (itemParentPosition - _origin.position).normalized;
            predictedArmPosition = _origin.position + toItemParentDirection * Vector3.Distance(predictedArmPosition, _origin.position);

            var spineCorrection = CalculateSpineCorrection();
            predictedArmPosition -= spineCorrection;

            var startPosition = predictedArmPosition - throwingItemView.transform.localPosition;
            var direction = (_throwingArm.ThrowingDirectionAim.position - itemParentPosition).normalized;
            var velocity = throwingItemView.ThrowPower / throwingItemView.Rigidbody.mass;

            var maxSteps = Mathf.Min(_predictionStepsLimit, Mathf.RoundToInt(_predictionDurationLimit / Time.fixedDeltaTime / _predictionTimeStepInterval));
            var predictionPoints = new Vector3[maxSteps];
            for (int i = 0; i < maxSteps; ++i)
            {
                var calculatedPosition = ComputePosition(startPosition, direction, velocity, i * Time.fixedDeltaTime);
                predictionPoints[i] = calculatedPosition;
            }

            return predictionPoints;
        }

        private Vector3 ComputePosition(in Vector3 origin, in Vector3 direction, float velocity, float t)
        {
            var calculatedPosition = origin + t * velocity * direction;
            calculatedPosition.y += 0.5f * Physics.gravity.y * t * t;
            return calculatedPosition;
        }

        private void CalculateArmLenght()
        {
            _armLength = 0;

            var bonesLenght = _armBones.Length;
            if (bonesLenght < 2)
            {
                return;
            }

            var previousBone = _armBones[0];
            for (int i = 1, iLen = _armBones.Length; i < iLen; ++i)
            {
                var currentBone = _armBones[i];
                _armLength += Vector3.Distance(previousBone.position, currentBone.position);
                previousBone = currentBone;
            }
        }

        private Vector3 CalculateSpineCorrection()
        {
            var totalCorrection = Vector3.zero;

            foreach (Transform spineBone in _spineBones)
            {
                var localOffset = spineBone.localPosition;
                localOffset *= _spineChainIK.weight;
                var globalOffset = spineBone.parent.TransformVector(localOffset);

                totalCorrection += globalOffset;
            }

            return totalCorrection;
        }
    }
}