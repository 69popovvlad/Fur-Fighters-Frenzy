using System;
using Client.GameLogic.Arm;
using Client.GameLogic.Arm.IK;
using Client.GameLogic.Inputs.Commands.Punching;
using Client.GameLogic.Throwing.Taking;
using FishNet.Object;
using UnityEngine;

namespace Client.GameLogic.Eating
{
    public class EatingArmControl : NetworkBehaviour, ArmStateControlBase<PunchInputCommand>
    {
        public event Action OnEaten;

        [SerializeField] private TakingArmControl _takingArm;

        [Header("Eating")]
        [SerializeField] private ArmIKControl _armIk;

        public void Enable(bool enabled) =>
            this.enabled = enabled;

        public void Enter() { /* Nothing to do */ }

        public void Exit() { /* Nothing to do */ }

        public void OnInputCommand(PunchInputCommand inputCommand)
        {
            if (inputCommand.ButtonState == 1)
            {
                EatToServer();
            }
        }

        [ServerRpc]
        public void EatToServer() =>
            EatToAllClients();

        [ObserversRpc(RunLocally = true)]
        private void EatToAllClients()
        {
            _armIk.OnTargetReached += OnMouthAimReached;
            _armIk.Play();
        }

        private void OnMouthAimReached()
        {
            _armIk.OnTargetReached -= OnMouthAimReached;

            _takingArm.DropItem(Vector3.zero);

            OnEaten?.Invoke();
        }
    }
}