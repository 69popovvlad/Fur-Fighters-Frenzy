using System;
using Client.GameLogic.Inputs;
using Client.GameLogic.Inputs.Commands;

namespace Client.GameLogic.Arm
{
    public class ArmBlackboard : IArmBlackboard, IDisposable
    {
        public InputBucket InputBucket { get; private set; }
        public ArmType ArmType { get; private set; }

        public IInputCommand IInputCommand { get; private set; }

        public ArmBlackboard(ArmType armType, InputBucket inputBucket)
        {
            ArmType = armType;
            InputBucket = inputBucket;
        }

        public void Dispose() { /* Nothing to do */ }

        public void SetInputCommand(IInputCommand inputCommand)
        {
            IInputCommand = inputCommand;
        }
    }
}