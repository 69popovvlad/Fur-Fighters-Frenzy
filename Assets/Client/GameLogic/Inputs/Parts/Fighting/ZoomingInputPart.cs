using Client.GameLogic.Inputs.Commands.Zooming;
using Core.Ioc;
using UnityEngine;

namespace Client.GameLogic.Inputs.Parts
{
    public class ZoomingInputPart: IInputPart
    {
        private readonly InputBucket _inputBucket;
        
        public ZoomingInputPart()
        {
            _inputBucket = Ioc.Instance.Get<InputBucket>();
        }
        
        public void Dispose()
        {
            /* Nothing to do */
        }

        public void Update(in InputPartData data, float delta)
        {
            var scrollDelta = Input.mouseScrollDelta;
            var command = new ZoomingInputCommand(data.OwnerKey, scrollDelta);
            _inputBucket.Invoke(command);
        }
    }
}