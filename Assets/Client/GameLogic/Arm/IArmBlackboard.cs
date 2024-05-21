using Client.GameLogic.Inputs;

namespace Client.GameLogic.Arm
{
    public interface IArmBlackboard
    {
        public InputBucket InputBucket { get; }

        public ArmType ArmType { get; }
    }
}