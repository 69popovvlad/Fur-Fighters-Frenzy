using Core.FSM;

namespace Client.GameLogic.Inputs
{
    public class PlayerInputStateMachine : StateMachine<PlayerInputStateType>
    {
        private readonly string _ownerKey;
        private readonly string _currentEntityKey;
      
        internal void Run() =>
            Start();

        internal void Stop() =>
            CurrentState.ExitState();

        public new void AddState(StateBase<PlayerInputStateType> state)
        {
            base.AddState(state);
            CurrentState ??= state;
        }
    }
}
