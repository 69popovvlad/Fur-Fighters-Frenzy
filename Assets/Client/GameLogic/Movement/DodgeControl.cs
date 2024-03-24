using Client.GameLogic.Inputs;
using Client.GameLogic.Inputs.Commands.Movement;
using Core.Ioc;
using FishNet.Object;
using UnityEngine;

namespace Client.GameLogic.Movement
{
    public class DodgeControl : NetworkBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _dodgePower = 10;
        [SerializeField] private float _dodgeCooldown = 2;
        
        private readonly int DodgeHash = Animator.StringToHash("Dodge");

        private InputBucket _inputBucket;
        private float _dodgeCooldownLeft;

        private void Awake()
        {
            _inputBucket = Ioc.Instance.Get<InputBucket>();
            _inputBucket.Subscribe<DodgeInputCommand>(OnDodgeInputCommand);
        }

        private void OnDestroy()
        {
            _inputBucket.Unsubscribe<DodgeInputCommand>(OnDodgeInputCommand);
        }

        private void Update()
        {
            if (_dodgeCooldownLeft <= 0)
            {
                return;
            }
            
            _dodgeCooldownLeft -= Time.deltaTime;
        }

        private void OnDodgeInputCommand(DodgeInputCommand command)
        {
            if (_dodgeCooldownLeft > 0)
            {
                return;
            }
            
            _dodgeCooldownLeft = _dodgeCooldown;
            DodgeJump(command.XSpeed, command.ZSpeed);
        }

        [ServerRpc(RequireOwnership = true)]
        private void DodgeJump(float x, float z)
        {
            DodgeToAllClients(x, z);
        }

        [ObserversRpc(RunLocally = true)]
        private void DodgeToAllClients(float x, float z)
        {
            var direction = transform.TransformDirection(new Vector3(x, 0, z));
            _rigidbody.AddForce(direction.normalized * _dodgePower, ForceMode.Impulse);
            _animator.SetTrigger(DodgeHash);
        }
    }
}