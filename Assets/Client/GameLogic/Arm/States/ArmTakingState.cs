using Client.GameLogic.Inputs.Commands.Taking;
using Client.GameLogic.Throwing.Taking;

namespace Client.GameLogic.Arm.States
{
    public class ArmTakingState : ArmStateBase<TakingInputCommand, TakingArmControl>
    {
        public ArmTakingState(ArmBlackboard blackboard, TakingArmControl armControl)
            : base(blackboard, armControl, ArmStateType.Taking)
        {
            /* Nothing to do */
        }

        public override void EnterState()
        {
            ArmComponent.OnItemTaken += OnItemTaken;
            ArmComponent.OnNoItem += OnNoItem;

            base.EnterState();
        }

        public override void ExitState()
        {
            ArmComponent.OnItemTaken -= OnItemTaken;
            ArmComponent.OnNoItem -= OnNoItem;

            base.ExitState();
        }

        public override void UpdateState(float delta) { /* Nothing to do */ }

        private void OnItemTaken()
        {
            ArmComponent.OnItemTaken -= OnItemTaken;

            Blackboard.SetInputCommand(default);
            NextStateKey = Blackboard.ArmType == ArmType.Right ? ArmStateType.Throwing : ArmStateType.Eating;
        }

        private void OnNoItem()
        {
            ArmComponent.OnNoItem -= OnNoItem;

            Blackboard.SetInputCommand(default);
            NextStateKey = ArmStateType.Punching;
        }
    }
}