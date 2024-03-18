using Client.GameLogic.Collision;
using Client.GameLogic.Inputs.Commands.Punching;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Client.GameLogic.Punching
{
    public class ArmPunchingControl : MonoBehaviour
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
            if (_armIK.weight <= 0)
            {
                _armCollision.Enable(false);
            }
        }

        internal void Punch(in PunchInputCommand command)
        {
            _armIK.weight = 1;
            _armCollision.Enable(true);
        }
    }
}