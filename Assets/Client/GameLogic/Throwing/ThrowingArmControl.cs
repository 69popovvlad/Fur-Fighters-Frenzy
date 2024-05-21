using System;
using Client.Audio;
using Client.GameLogic.Arm;
using Client.GameLogic.Arm.IK;
using Client.GameLogic.Inputs.Commands.Punching;
using Client.GameLogic.Throwing.Taking;
using Core.Ioc;
using FishNet.Object;
using UnityEngine;

namespace Client.GameLogic.Throwing
{
    public class ThrowingArmControl : NetworkBehaviour, ArmStateControlBase<PunchInputCommand>
    {
        public event Action OnThrown;

        [SerializeField] private TakingArmControl _takingArm;

        [Header("Throwing")]
        [SerializeField] private ArmIKControl _armIk;
        [SerializeField] private ThrowingItemPrediction _throwingPredication;
        [SerializeField] private Transform _throwingDirectionAim;

        private Vector3 throwingDirection;
        private AudioPlayerService _audioPlayerService;

        public Transform ThrowingDirectionAim => _throwingDirectionAim;

        public bool HasItem => _takingArm.HasItem;

        public TakingItemViewBase Item => _takingArm.Item;

        public Transform ItemParent => _takingArm.ItemParent;

        public void Enable(bool enabled) =>
            this.enabled = enabled;

        public void Enter() { /* Nothing to do */ }

        public void Exit() { /* Nothing to do */ }

        public void OnInputCommand(PunchInputCommand inputCommand)
        {
            if (inputCommand.ButtonState == 3)
            {
                ThrowToServer();
                return;
            }

            if (inputCommand.ButtonState == 2)
            {
                _throwingPredication.Show(true);
                return;
            }
        }

        private void Awake()
        {
            var ioc = Ioc.Instance;
            _audioPlayerService = ioc.Get<AudioPlayerService>();
        }

        [ServerRpc]
        public void ThrowToServer() =>
            ThrowToAllClients();

        [ObserversRpc(RunLocally = true)]
        private void ThrowToAllClients()
        {
            _armIk.OnStarted += OnThrowingStarted;
            _armIk.OnTargetReached += OnThrowingAimReached;
            _armIk.Play();
        }

        private void OnThrowingStarted()
        {
            _armIk.OnStarted -= OnThrowingStarted;

            throwingDirection = _throwingDirectionAim.position - ItemParent.position;
            _throwingPredication.Show(false);
        }

        private void OnThrowingAimReached()
        {
            _armIk.OnTargetReached -= OnThrowingAimReached;
            
            _throwingPredication.Show(false);

            if (Item == null)
            {
                OnThrown?.Invoke();
                return;
            }

            if (_armIk.InAction)
            {
                _armIk.ForceReturn();
            }

            _takingArm.DropItem(throwingDirection);

            OnThrown?.Invoke();

            _audioPlayerService.PlayClip(transform.position, "throwing");
        }
    }
}