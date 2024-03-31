using Client.GameLogic.Characters;
using Client.GameLogic.Inputs;
using Client.GameLogic.Inputs.Commands;
using Core.Ioc;
using FishNet.Object;
using UnityEngine;

namespace Client.GameLogic.Slipping
{
    public class SlippingControl : NetworkBehaviour
    {
        private readonly int SlipHash = Animator.StringToHash("Slip");

        [SerializeField] private CharacterView _characterView;
        [SerializeField] private Animator _animator;
        [SerializeField] private SlippingAnimationTriggerHandler _animationTrigger;

        private InputBucket _inputBucket;

        private void Awake()
        {
            var ioc = Ioc.Instance;
            _inputBucket = ioc.Get<InputBucket>();

            _animationTrigger.OnSlipTrigger += OnSlipTrigger;
        }

        private void OnDestroy()
        {
            _animationTrigger.OnSlipTrigger -= OnSlipTrigger;
        }

        [ObserversRpc(RunLocally = true)]
        internal void Slip()
        {
            _animator.SetTrigger(SlipHash);
        }

        private void OnSlipTrigger(bool isStart)
        {
            var command = new InputEnableCommand(_characterView.Guid, !isStart);
            _inputBucket.Invoke(command);
        }
    }
}