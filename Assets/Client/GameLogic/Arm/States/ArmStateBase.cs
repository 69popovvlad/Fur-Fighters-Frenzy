using Client.GameLogic.Inputs;
using Client.GameLogic.Inputs.Commands;
using Core.FSM;
using UnityEngine;

namespace Client.GameLogic.Arm.States
{
    public abstract class ArmStateBase<TCommand, TControl> : StateBase<ArmStateType>
        where TCommand : IInputCommand
        where TControl : ArmStateControlBase<TCommand>
    {
        protected TControl ArmComponent;
        protected ArmBlackboard Blackboard;
        protected InputBucket InputBucket => Blackboard.InputBucket;
        protected ArmStateType NextStateKey;
        private ArmStateType _startStateKey;

        public ArmStateBase(ArmBlackboard blackboard, TControl armComponent, ArmStateType stateKey) : base(stateKey)
        {
            Blackboard = blackboard;
            ArmComponent = armComponent;
            NextStateKey = _startStateKey = stateKey;
        }

        public override ArmStateType GetNextStateKey()
        {
            return NextStateKey;
        }

        public override void EnterState()
        {
            Debug.Log($"Start State {this.GetType().Name}");
            ArmComponent.Enable(true);
            ArmComponent.Enter();

            if (Blackboard.IInputCommand == default)
            {
                return;
            }

            ArmComponent.OnInputCommand((TCommand)Blackboard.IInputCommand);
        }

        public override void ExitState()
        {
            Debug.Log($"End State {this.GetType().Name}");
            ArmComponent.Enable(false);
            ArmComponent.Exit();

            NextStateKey = _startStateKey;
        }
    }
}