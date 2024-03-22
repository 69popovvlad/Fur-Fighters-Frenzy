using Client.GameLogic.Inputs.Commands.Aiming;
using Client.Services.Inputs;
using Core.Ioc;
using UnityEngine;

namespace Client.GameLogic.Inputs.Parts
{
    public class AimingInputPart : IInputPart
    {
        private readonly InputBucket _inputBucket;
        private readonly GridRaycastService _gridRaycastService;

        public AimingInputPart()
        {
            var ioc = Ioc.Instance;
            _inputBucket = ioc.Get<InputBucket>();
            _gridRaycastService = ioc.Get<GridRaycastService>();
        }

        public void Dispose()
        {
            /* Nothing to do */
        }

        public void Update(in InputPartData data, float delta)
        {   
            var command = new AimInputCommand(data.OwnerKey, Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            _inputBucket.Invoke(command);
        }
    }
}