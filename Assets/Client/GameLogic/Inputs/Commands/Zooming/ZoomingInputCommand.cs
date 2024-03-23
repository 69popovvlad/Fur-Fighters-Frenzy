using UnityEngine;

namespace Client.GameLogic.Inputs.Commands.Zooming
{
    public struct ZoomingInputCommand: IInputCommand
    {
        public string OwnerKey { get; private set; }
        public Vector2 ZoomingDelta { get; private set; }

        public ZoomingInputCommand(string ownerKey, Vector2 zoomingDelta)
        {
            OwnerKey = ownerKey;
            ZoomingDelta = zoomingDelta;
        }
    }
}