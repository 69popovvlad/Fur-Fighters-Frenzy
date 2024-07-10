using System.Collections.Generic;
using Client.GameLogic.Inputs.Parts;
using UnityEngine;

namespace Client.GameLogic.Inputs
{
    public class PlayerInputHandler : InputHandlerBase
    {
        private PlayerInputStateMachine _stateMachine;
        
        protected override void AwakeInternal()
        {
            base.AwakeInternal();
            
            _stateMachine = new PlayerInputStateMachine();

            _stateMachine.AddState(new FightingPlayerInputState(OwnerKey, enabled,CurrentEntityKey, PlayerInputStateType.Fighting));
            _stateMachine.AddState(new ChokeHoldInputState(OwnerKey, CurrentEntityKey, PlayerInputStateType.ChokeHold));
        }
        
        private void Start()
        {
          _stateMachine.Run();
        }

        private void OnDestroy()
        {
            _stateMachine.Stop();
        }

        private void Update() =>
            _stateMachine.Update(Time.deltaTime);

        public override void Initialize()
        {
            // CurrentEntityKey
        }
        
        // public override void InputsInitialize(bool isOwner)
        // {
        //     if (isOwner)
        //     {
        //         return;
        //     }
        //
        //     Destroy(this);
        // }
        

        public override void SetEnable(bool enable)
        {
            enabled = enable;
        }
    }
}