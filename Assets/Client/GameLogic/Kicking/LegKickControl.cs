using Client.Audio;
using Client.GameLogic.Collision;
using Client.GameLogic.Inputs;
using Client.GameLogic.Inputs.Commands.Kicking;
using Core.Ioc;
using FishNet.Object;
using UnityEngine;

namespace Client.GameLogic.Kicking
{
    public class LegKickControl : NetworkBehaviour
    {
        private readonly int LegKickHash = Animator.StringToHash("LegKick");

        [SerializeField] private LegKickAnimationTriggerHandler _animationTrigger;
        [SerializeField] private Animator _animator;
        [SerializeField] private CollisionProxy _legCollision;
        [SerializeField] private float _kickDelay = 2;

        private AudioPlayerService _audioPlayerService;
        private InputBucket _inputBucket;
        private float _currentKickDelay;

        private void Awake()
        {
            var ioc = Ioc.Instance;
            _audioPlayerService = ioc.Get<AudioPlayerService>();

            _inputBucket = ioc.Get<InputBucket>();
            _inputBucket.Subscribe<LegKickInputCommand>(OnLegKickInputCommand);

            _animationTrigger.OnKickTrigger += OnKickTrigger;
        }

        private void OnDestroy()
        {
            _inputBucket.Unsubscribe<LegKickInputCommand>(OnLegKickInputCommand);
            _animationTrigger.OnKickTrigger -= OnKickTrigger;
        }

        private void Update()
        {
            if (_currentKickDelay <= 0)
            {
                return;
            }

            _currentKickDelay -= Time.deltaTime;
        }

        private void OnKickTrigger(int legIndex, bool isStart)
        {
            _legCollision.Enable(isStart);

            if (isStart)
            {
                _audioPlayerService.PlayClip(transform.position, "kick_swing");
            }
        }

        private void OnLegKickInputCommand(LegKickInputCommand command)
        {
            if (!IsOwner || _currentKickDelay > 0)
            {
                return;
            }

            SetTriggerToServer();
        }

        [ServerRpc(RequireOwnership = true)]
        private void SetTriggerToServer()
        {
            SetTriggerToAllClients();
        }

        [ObserversRpc(RunLocally = true)]
        private void SetTriggerToAllClients()
        {
            if (_currentKickDelay > 0)
            {
                return;
            }

            // This shoud be synchronized by Network Animator component
            _animator.SetTrigger(LegKickHash);

            _currentKickDelay = _kickDelay;
        }
    }
}