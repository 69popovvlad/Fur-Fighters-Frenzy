using Client.GameLogic.Inputs.Commands.Punching;
using Core.Ioc;
using UnityEngine;

namespace Client.GameLogic.Inputs.Parts
{
    public class PunchingInputPart: IInputPart
    {
        private readonly InputBucket _inputBucket;
        
        public PunchingInputPart()
        {
            _inputBucket = Ioc.Instance.Get<InputBucket>();
        }
        public void Dispose()
        {
            /* Nothing to do */
        }

        public void Update(in InputPartData data, float delta)
        {
            LeftPunch(data, delta);
            RightPunch(data, delta);
        }

        private void LeftPunch(in InputPartData data, float delta) =>
            SendPunch(data, delta, true, 0);
        
        private void RightPunch(in InputPartData data, float delta) =>
            SendPunch(data, delta, false, 1);
        
        private void SendPunch(in InputPartData data, float delta, bool isLeftHand, int mouseKeyIndex)
        {
            if (!Input.GetMouseButtonDown(mouseKeyIndex))
            {
                return;   
            }

            var command = new PunchInputCommand(data.OwnerKey, isLeftHand);
            _inputBucket.Invoke(command);
        }
    }
}