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
        [SerializeField] private float _comebackDuration = 1;
        
        private void Update()
        {
            if (_armIK.weight <= 0)
            {
                return;
            }

            _armIK.weight -= Time.deltaTime / _comebackDuration;
            
            if (_armIK.weight > 0)
            {
                return;
            }
            
            if (!IsOwner)
            {
                return;
            }
            
            SetWeightToServer(0);
        }

        internal void Punch(in PunchInputCommand command)
        {
            if (!IsOwner)
            {
                return;
            }

            SetWeightToServer(1);
        }

        [ServerRpc]
        private void SetWeightToServer(float weight)
        {
            SetWeightOnClients(weight);
        }

        [ObserversRpc]
        private void SetWeightOnClients(float weight)
        {
            _armIK.weight = weight;
            _armCollision.Enable(weight > 0);
        }
    }
}