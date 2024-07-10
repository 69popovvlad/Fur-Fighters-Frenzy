using Client.GameLogic.Inputs.Commands.ChokeHold;
using Client.GameLogic.Inputs.Commands.Zooming;
using Client.GameLogic.Inputs.Parts;
using Core.Ioc;
using UnityEngine;

namespace Client.GameLogic.Inputs
{
    public class ChokeHoldInputPart : IInputPart
    {
        private readonly InputBucket _inputBucket;
        
        public ChokeHoldInputPart()
        {
            _inputBucket = Ioc.Instance.Get<InputBucket>();
        }
        
        public void Dispose()
        {
            /* Nothing to do */
        }

        public void Update(in InputPartData data, float delta)
        {
            var command = new ChokeHoldInputCommand(data.OwnerKey);
            _inputBucket.Invoke(command);
        }
    }
}
