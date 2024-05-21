using Client.GameLogic.Arm.States;
using Client.GameLogic.Eating;
using Client.GameLogic.Inputs;
using Client.GameLogic.Punching;
using Client.GameLogic.Throwing;
using Client.GameLogic.Throwing.Taking;
using UnityEngine;

namespace Client.GameLogic.Arm
{
    public class ArmControl : InputListenerNetworkComponentBase
    {
        [SerializeField] private ArmType _armType;
        [SerializeField] private MonoBehaviour[] _armComponents;
        private ArmStateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine = new ArmStateMachine(_armType);

            for (int i = 0, iLen = _armComponents.Length; i < iLen; ++i)
            {
                switch (_armComponents[i])
                {
                    case ArmPunchingControl armControl:
                        _stateMachine.AddState(armControl);
                        break;

                    case TakingArmControl armControl:
                        _stateMachine.AddState(armControl);
                        break;

                    case ThrowingArmControl armControl:
                        _stateMachine.AddState(armControl);
                        break;

                    case EatingArmControl armControl:
                        _stateMachine.AddState(armControl);
                        break;
                }
            }
        }

        private void Start()
        {
            _stateMachine.Run();
        }

        private void OnDestroy()
        {
            _stateMachine.Stop();
        }

        private void Update() =>
            _stateMachine.Update(Time.deltaTime);

        public override void InputsInitialize(bool isOwner)
        {
            if (isOwner)
            {
                return;
            }

            Destroy(this);
        }
    }
}