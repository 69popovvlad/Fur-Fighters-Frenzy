using Client.GameLogic.Punching;
using Client.GameLogic.Inputs.Commands.Punching;
using Client.GameLogic.Inputs.Commands.Taking;

namespace Client.GameLogic.Arm.States
{
    public class ArmPunchingState : ArmStateBase<PunchInputCommand, ArmPunchingControl>
    {
        public ArmPunchingState(ArmBlackboard blackboard, ArmPunchingControl armControl)
            : base(blackboard, armControl, ArmStateType.Punching)
        {
            /* Nothing to do */
        }

        public override void EnterState()
        {
            InputBucket.Subscribe<PunchInputCommand>(OnPunchingCommand);
            InputBucket.Subscribe<TakingInputCommand>(OnTaken);

            base.EnterState();
        }

        public override void ExitState()
        {
            InputBucket.Unsubscribe<PunchInputCommand>(OnPunchingCommand);
            InputBucket.Unsubscribe<TakingInputCommand>(OnTaken);

            base.ExitState();
        }

        public override void UpdateState(float delta) { /* Nothing to do */ }

        private void OnPunchingCommand(PunchInputCommand command)
        {
            if (command.IsLeftHand && Blackboard.ArmType == ArmType.Right
                || !command.IsLeftHand && Blackboard.ArmType == ArmType.Left)
            {
                return;
            }

            ArmComponent.OnInputCommand(command);
        }

        private void OnTaken(TakingInputCommand command)
        {
            InputBucket.Unsubscribe<TakingInputCommand>(OnTaken);

            Blackboard.SetInputCommand(command);
            NextStateKey = ArmStateType.Taking;
        }
    }
}