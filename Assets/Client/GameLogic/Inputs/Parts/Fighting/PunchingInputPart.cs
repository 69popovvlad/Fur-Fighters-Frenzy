using Client.GameLogic.Inputs.Commands.Punching;
using Core.Ioc;
using UnityEngine;

namespace Client.GameLogic.Inputs.Parts
{
    public class PunchingInputPart : IInputPart
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
            byte buttonState;
            if (Input.GetMouseButtonDown(mouseKeyIndex))
            {
                buttonState = 1; // Button just pressed
            }
            else if (Input.GetMouseButton(mouseKeyIndex))
            {
                buttonState = 2; // Button held down
            }
            else if (Input.GetMouseButtonUp(mouseKeyIndex))
            {
                buttonState = 3; // Button just released
            }
            else
            {
                return;
            }

            var command = new PunchInputCommand(data.OwnerKey, isLeftHand, buttonState);
            _inputBucket.Invoke(command);
        }
    }
}