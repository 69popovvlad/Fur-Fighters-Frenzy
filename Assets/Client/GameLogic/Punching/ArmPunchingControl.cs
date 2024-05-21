using Client.Audio;
using Client.GameLogic.Arm;
using Client.GameLogic.Arm.IK;
using Client.GameLogic.Collision;
using Client.GameLogic.Inputs.Commands.Punching;
using Core.Ioc;
using FishNet.Object;
using UnityEngine;

namespace Client.GameLogic.Punching
{
    public class ArmPunchingControl : NetworkBehaviour, ArmStateControlBase<PunchInputCommand>
    {
        [SerializeField] private ArmIKControl _armIk;
        [SerializeField] private CollisionProxy _armCollision;

        private AudioPlayerService _audioPlayerService;

        private void Awake()
        {
            var ioc = Ioc.Instance;
            _audioPlayerService = ioc.Get<AudioPlayerService>();

            _armIk.OnTargetReached += PlayPunchSound;
        }

        private void OnDestroy()
        {
            _armIk.OnTargetReached -= PlayPunchSound;
        }

        public void Enable(bool enabled) =>
            this.enabled = enabled;

        public void Enter() { /* Nothing to do */ }

        public void Exit() { /* Nothing to do */ }

        public void OnInputCommand(PunchInputCommand inputCommand)
        {
            if (!IsOwner || inputCommand.ButtonState != 1)
            {
                return;
            }

            SetWeightToServer();
        }

        [ServerRpc(RequireOwnership = true)]
        private void SetWeightToServer() =>
            SetWeightOnClients();

        [ObserversRpc(RunLocally = true)]
        private void SetWeightOnClients()
        {
            if (_armIk.AnimationT > 0)
            {
                return;
            }

            _armIk.OnReturned += OnPunchReturned;
            _armIk.Play();

            _armCollision.Enable(true);
            _armCollision.OnCollided += OnPunchCollision;
        }

        private void OnPunchCollision(string entityKey, string partKey, ColliderDataControl control, UnityEngine.Collision collision)
        {
            _armCollision.OnCollided -= OnPunchCollision;

            _armIk.ForceReturn();
            PlayPunchSound();
        }

        private void OnPunchReturned()
        {
            _armIk.OnReturned -= OnPunchReturned;
            _armCollision.Enable(false);
        }

        private void PlayPunchSound()
        {
            _audioPlayerService.PlayClip(transform.position, "punch_swing");
        }
    }
}