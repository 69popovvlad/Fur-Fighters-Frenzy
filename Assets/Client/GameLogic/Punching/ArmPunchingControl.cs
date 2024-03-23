using Client.GameLogic.Collision;
using Client.GameLogic.Inputs.Commands.Punching;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Client.GameLogic.Punching
{
    public class ArmPunchingControl : NetworkBehaviour
    {
        [SerializeField] private ChainIKConstraint _armIK;
        [SerializeField] private CollisionProxy _armCollision;
        [SerializeField] private AnimationCurve _punchCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private float _punchDuration = 0.2f;
        [SerializeField] private float _comebackDuration = 0.3f;

        private bool _inPunching;
        private float _punchT;

        private void Update()
        {
            if (!_inPunching)
            {
                CalculatePunchReturn();
                return;
            }

            CalculatePunch();
        }

        private void CalculatePunch()
        {
            if (_punchT >= 1)
            {
                _inPunching = false;
                return;
            }

            _punchT += Time.deltaTime / _punchDuration;
            _armIK.weight = _punchCurve.Evaluate(_punchT);
        }

        private void CalculatePunchReturn()
        {
            if (_punchT <= 0)
            {
                return;
            }

            _punchT -= Time.deltaTime / _comebackDuration;
            _armIK.weight = _punchCurve.Evaluate(_punchT);

            if (_punchT > 0)
            {
                return;
            }

            _armCollision.Enable(false);
        }

        internal void Punch(in PunchInputCommand command)
        {
            if (!IsOwner)
            {
                return;
            }

            SetWeightToServer();
        }

        [ServerRpc(RequireOwnership = true)]
        private void SetWeightToServer()
        {
            SetWeightOnClients();
        }

        [ObserversRpc(RunLocally = true)]
        private void SetWeightOnClients()
        {
            if (_punchT > 0)
            {
                return;
            }
            
            _inPunching = true;
            _armCollision.Enable(true);
            _punchT = 0;
        }
    }
}