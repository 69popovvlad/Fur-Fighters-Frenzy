using System.Collections.Generic;
using Client.GameLogic.Inputs.Parts;
using Core.FSM;
using UnityEngine;

namespace Client.GameLogic.Inputs
{
    public class FightingPlayerInputState : StateBase<PlayerInputStateType>
    {
        private readonly string _ownerKey;
        private readonly bool _entityKey;
        private readonly string _currentEntityKey;
        private readonly List<IInputPart> _inputParts = new List<IInputPart>();

        public FightingPlayerInputState(string ownerKey, bool entityKey, string currentEntityKey, PlayerInputStateType stateKey) : base(stateKey)
        {
            _ownerKey = ownerKey;
            _entityKey = entityKey;
            _currentEntityKey = currentEntityKey;

            _inputParts.Add(new PunchingInputPart());
            _inputParts.Add(new MovementInputPart());
            _inputParts.Add(new AimingInputPart());
            _inputParts.Add(new ZoomingInputPart());
            _inputParts.Add(new DodgeInputPart());
            _inputParts.Add(new TakingItemInputPart());
            _inputParts.Add(new LegKickInputPart());
        }

        public override void EnterState()
        {
            
        }

        // private void OnDestroy()
        // {
        //     for (int i = 0, length = _inputParts.Count; i < length; ++i)
        //     {
        //         _inputParts[i].Dispose();
        //     }
        // }

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
            return PlayerInputStateType.Fighting;
        }
    }
}
