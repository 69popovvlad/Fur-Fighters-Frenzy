using Client.GameLogic.Inputs.Commands.Kicking;
using Core.Ioc;
using UnityEngine;

namespace Client.GameLogic.Inputs.Parts
{
    public class LegKickInputPart : IInputPart
    {
         private readonly InputBucket _inputBucket;

        public LegKickInputPart()
        {
            var ioc = Ioc.Instance;
            _inputBucket = ioc.Get<InputBucket>();
        }

        public void Dispose()
        {
            /* Nothing to do */
        }

        public void Update(in InputPartData data, float delta)
        {   
            if(!Input.GetKeyDown(KeyCode.Q))
            {
                return;
            }

            var command = new LegKickInputCommand(data.OwnerKey);
            _inputBucket.Invoke(command);
        }
    }
}