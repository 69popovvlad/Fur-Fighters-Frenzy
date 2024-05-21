using Client.GameLogic.Inputs.Commands;

namespace Client.GameLogic.Arm
{
    public interface ArmStateControlBase<T> where T: IInputCommand
    {
        public void Enable(bool enabled);

        public void Enter();

        public void Exit();
        
        public void OnInputCommand(T inputCommand);
    }
}