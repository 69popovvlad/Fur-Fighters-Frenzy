using System.Collections.Generic;
using Client.GameLogic.Inputs.Parts;
using UnityEngine;

namespace Client.GameLogic.Inputs
{
    public class PlayerInputHandler: InputHandlerBase
    {
        private readonly List<IInputPart> _inputParts = new List<IInputPart>();

        public override void Initialize()
        {
            // CurrentEntityKey
        }

        protected override void AwakeInternal()
        {
            base.AwakeInternal();
            
            _inputParts.Add(new PunchingInputPart());
            _inputParts.Add(new MovementInputPart());
            _inputParts.Add(new AimingInputPart());
        }

        private void OnDestroy()
        {
            for (int i = 0, length = _inputParts.Count; i < length; ++i)
            {
                _inputParts[i].Dispose();
            }
        }

        private void Update()
        {
            var inputData = new InputPartData()
            {
                OwnerKey = OwnerKey,
                EntityKey = CurrentEntityKey,
            };

            var delta = Time.deltaTime;
            for (int i = 0, length = _inputParts.Count; i < length; ++i)
            {
                _inputParts[i].Update(inputData, delta);
            }
        }
    }
}