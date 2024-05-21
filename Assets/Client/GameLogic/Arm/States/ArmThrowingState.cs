using Client.GameLogic.Inputs.Commands.Punching;
using Client.GameLogic.Throwing;

namespace Client.GameLogic.Arm.States
{
    public class ArmThrowingState : ArmStateBase<PunchInputCommand, ThrowingArmControl>
    {
        public ArmThrowingState(ArmBlackboard blackboard, ThrowingArmControl armControl)
            : base(blackboard, armControl, ArmStateType.Throwing)
        {
            /* Nothing to do */
        }

        public override void EnterState()
        {
            InputBucket.Subscribe<PunchInputCommand>(OnThrowingCommind);
            ArmComponent.OnThrown += OnThrown;

            base.EnterState();
        }

        public override void ExitState()
        {
            InputBucket.Unsubscribe<PunchInputCommand>(OnThrowingCommind);
            ArmComponent.OnThrown -= OnThrown;

            base.ExitState();
        }

        public override void UpdateState(float delta) { /* Nothing to do */ }

        private void OnThrowingCommind(PunchInputCommand command)
        {
            if (command.IsLeftHand && Blackboard.ArmType == ArmType.Right
                || !command.IsLeftHand && Blackboard.ArmType == ArmType.Left)
            {
                return;
            }

            ArmComponent.OnInputCommand(command);
        }

        private void OnThrown()
        {
            ArmComponent.OnThrown -= OnThrown;

            Blackboard.SetInputCommand(default);
            NextStateKey = ArmStateType.Punching;
        }
    }
}