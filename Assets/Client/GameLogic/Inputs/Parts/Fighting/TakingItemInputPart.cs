using Client.GameLogic.Inputs.Commands.Taking;
using Core.Ioc;
using UnityEngine;

namespace Client.GameLogic.Inputs.Parts
{
    public class TakingItemInputPart : IInputPart
    {
        private readonly InputBucket _inputBucket;

        public TakingItemInputPart()
        {
            _inputBucket = Ioc.Instance.Get<InputBucket>();
        }

        public void Dispose()
        {
            /* Nothing to do */
        }

        public void Update(in InputPartData data, float delta)
        {
            if (!Input.GetKeyDown(KeyCode.E))
            {
                return;
            }

            var command = new TakingInputCommand(data.OwnerKey);
            Debug.Log(data.OwnerKey);
            Debug.Log(command);
            Debug.Log(_inputBucket);
            _inputBucket.Invoke(command);
        }
    }
}