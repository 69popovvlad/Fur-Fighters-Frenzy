using Client.GameLogic.Inputs;
using Client.GameLogic.Inputs.Commands.Punching;
using Core.Ioc;
using UnityEngine;

namespace Client.GameLogic.Punching
{
    public class PunchingControl : MonoBehaviour
    {
        [Header("Left arm")]
        [SerializeField] private ArmPunchingControl _leftArm;

        [Header("Right arm")]
        [SerializeField] private ArmPunchingControl _rightArm;

        private InputBucket _inputBucket;
        
        private void Awake()
        {
            var ioc = Ioc.Instance;
            
            _inputBucket = ioc.Get<InputBucket>();
            _inputBucket.Subscribe<PunchInputCommand>(OnPunchCommand);
        }

        private void OnDestroy()
        {
            _inputBucket.Unsubscribe<PunchInputCommand>(OnPunchCommand);
        }

        private void OnPunchCommand(PunchInputCommand command)
        {
            if (command.isLeftHand)
            {
                _leftArm.Punch(command);
            }
            else
            {
                _rightArm.Punch(command);
            }
        }
    }
}