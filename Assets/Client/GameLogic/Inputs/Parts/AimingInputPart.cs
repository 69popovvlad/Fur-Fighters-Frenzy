using Client.GameLogic.Inputs.Commands.Aiming;
using Client.Services.Inputs;
using Core.Ioc;

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
            if (!_gridRaycastService.TryGetWorldTouchPosition(out var worldPosition))
            {
                return;
            }
            
            var command = new AimInputCommand(data.OwnerKey, worldPosition.x, .5f, worldPosition.z); // TODO: Replace 0.5f with some data
            _inputBucket.Invoke(command);
        }
    }
}