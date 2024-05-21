using Client.GameLogic.Eating;
using Client.GameLogic.Inputs.Commands.Punching;

namespace Client.GameLogic.Arm.States
{
    public class ArmEatingState : ArmStateBase<PunchInputCommand, EatingArmControl>
    {
        public ArmEatingState(ArmBlackboard blackboard, EatingArmControl armControl)
            : base(blackboard, armControl, ArmStateType.Eating)
        {
            /* Nothing to do */
        }

        public override void EnterState()
        {
            InputBucket.Subscribe<PunchInputCommand>(OnThrowingCommind);
            ArmComponent.OnEaten += OnEaten;

            base.EnterState();
        }

        public override void ExitState()
        {
            InputBucket.Unsubscribe<PunchInputCommand>(OnThrowingCommind);
            ArmComponent.OnEaten -= OnEaten;

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

        private void OnEaten()
        {
            ArmComponent.OnEaten -= OnEaten;

            Blackboard.SetInputCommand(default);
            NextStateKey = ArmStateType.Punching;
        }
    }
}