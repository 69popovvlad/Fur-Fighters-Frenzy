using System;
using Client.GameLogic.Throwing.Taking;
using UnityEngine;

namespace Client.GameLogic.Throwing
{
    public class ThrowingItemPrediction : MonoBehaviour
    {
        [SerializeField] private float _predictionDurationLimit = 1;
        [SerializeField] private float _predictionTimeStepInterval = 1;
        [SerializeField] private int _predictionStepsLimit = 20;
        [SerializeField] private Transform _origin;
        [SerializeField] private TakingArmControl _throwingArm;
        [SerializeField] private LineRenderer _lineRenderer;

        [Header("Arm length")]
        [SerializeField] private Transform[] _armBones;
        [SerializeField] private float _armLength = 20;

        [Header("Debug")]
        [SerializeField] private Transform _throwingStartPoint;

        private void Start()
        {
            enabled = false;
            _throwingArm.ItemTaken += OnItemTaken;

            CalculateArmLenght();
        }

        private void OnDestroy()
        {
            _throwingArm.ItemTaken -= OnItemTaken;
            _throwingArm.ItemDropped -= ItemDropped;
        }

        private void OnItemTaken()
        {
            _throwingArm.ItemDropped += ItemDropped;
            enabled = true;
        }

        private void ItemDropped()
        {
            _throwingArm.ItemDropped -= ItemDropped;
            enabled = false;
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

        private Vector3[] CalculatePrediction()
        {
            if (_throwingArm.Item is not ThrowingItemView throwingItemView)
            {
                return Array.Empty<Vector3>();
            }

            var throwingAimPosition = _throwingArm.ThrowingDirectionAim.position;

            var spineBonePosition = _armBones[0].position;
            var predictedArmPosition = spineBonePosition + (_origin.position - spineBonePosition).normalized * _armLength;

            var toItemParentDirection = (_throwingArm.ItemParent.position - _origin.position).normalized;
            predictedArmPosition += throwingItemView.transform.localPosition;
            predictedArmPosition = _origin.position + toItemParentDirection * Vector3.Distance(predictedArmPosition, _origin.position);
            _throwingStartPoint.position = predictedArmPosition;

            var direction = (throwingAimPosition - predictedArmPosition).normalized;
            var velocity = throwingItemView.ThrowPower / throwingItemView.Rigidbody.mass;

            var maxSteps = Mathf.Min(_predictionStepsLimit, Mathf.RoundToInt(_predictionDurationLimit / Time.fixedDeltaTime / _predictionTimeStepInterval));
            var predictionPoints = new Vector3[maxSteps];
            for (int i = 0; i < maxSteps; ++i)
            {
                var calculatedPosition = ComputePosition(predictedArmPosition, direction, velocity, i * Time.fixedDeltaTime);
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

        private float CalculateBoneLength(Transform currentBone, Transform endBone)
        {
            float boneLength = 0;

            foreach (Transform child in currentBone)
            {
                if (child != endBone)
                {
                    boneLength += CalculateBoneLength(child, endBone);
                }
                else
                {
                    boneLength += Vector3.Distance(currentBone.position, child.position);
                    Debug.Log(child.name);
                }
            }

            return boneLength;
        }
    }
}