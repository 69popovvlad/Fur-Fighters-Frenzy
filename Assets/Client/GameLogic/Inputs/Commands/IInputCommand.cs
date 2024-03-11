using Core.Buckets;

namespace Client.GameLogic.Inputs.Commands
{
    public interface IInputCommand: IBroadcast
    {
        string OwnerKey { get; }
    }
}