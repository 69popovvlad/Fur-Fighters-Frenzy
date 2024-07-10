using System;

namespace Client.GameLogic.Inputs.Parts
{
    public interface IInputPart: IDisposable
    {
        void Update(in InputPartData data, float delta);
    }
}