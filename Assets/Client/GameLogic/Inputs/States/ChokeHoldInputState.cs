using System.Collections.Generic;
using Client.GameLogic.Inputs.Parts;
using Core.FSM;
using UnityEngine;

namespace Client.GameLogic.Inputs
{
    public class ChokeHoldInputState : StateBase<PlayerInputStateType>
    {
        private readonly string _ownerKey;
        private readonly string _currentEntityKey;
        private readonly List<IInputPart> _inputParts = new List<IInputPart>();

        public ChokeHoldInputState(string ownerKey, string currentEntityKey, PlayerInputStateType stateKey) : base(stateKey)
        {
            _ownerKey = ownerKey;
            _currentEntityKey = currentEntityKey;

            _inputParts.Add(new ChokeHoldInputPart());
        }

        public override void EnterState()
        {
        }

        public override void UpdateState(float deltaTime)
        {
            var inputData = new InputPartData()
            {
                OwnerKey = _ownerKey,
                EntityKey = _currentEntityKey,
            };

            var delta = Time.deltaTime;
            for (int i = 0, length = _inputParts.Count; i < length; ++i)
            {
                _inputParts[i].Update(inputData, delta);
            }
        }

        public override void ExitState()
        {
        }

        public override PlayerInputStateType GetNextStateKey()
        {
            return PlayerInputStateType.ChokeHold;
        }
    }

}
