using Client.GameLogic.Inputs;
using Client.GameLogic.Inputs.Commands;
using Core.FSM;
using Core.Ioc;

namespace Client.GameLogic.Arm.States
{
    public class ArmStateMachine : StateMachine<ArmStateType>
    {
        private ArmType _armType;
        private InputBucket _inputBucket;
        private ArmBlackboard _blackboard;

        public ArmStateMachine(ArmType armType)
        {
            _armType = armType;

            var ioc = Ioc.Instance;
            _inputBucket = ioc.Get<InputBucket>();

            _blackboard = new ArmBlackboard(_armType, _inputBucket);
        }

        internal void Run() =>
            Start();

        internal void Stop()
        {
            CurrentState.ExitState();
            _blackboard?.Dispose();
        }

        internal void AddState<TCommand>(ArmStateControlBase<TCommand> component)
            where TCommand : struct, IInputCommand
        {
            var state = ArmStateFactory.CreateState(component, new object[] { _blackboard, component });
            AddState(state);

            CurrentState ??= state;
        }
    }
}