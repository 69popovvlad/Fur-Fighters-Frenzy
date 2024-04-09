using Client.GameLogic.CameraLogic;
using Client.GameLogic.Characters;
using Client.GameLogic.Health.Commands;
using Client.GameLogic.Inputs;
using Client.GameLogic.Inputs.Commands;
using Client.Network.GameLogic.Characters.Commands;
using Core.Entities.Views;
using UnityEngine;

namespace Client.Network.GameLogic
{
    public partial class CharacterSpawner
    {
        [SerializeField] private PlayerInputHandler _inputPrefab;

        private PlayerInputHandler _input;
        private CharacterView _characterViewClientSide;

        private void OnClientSideInitialized()
        {
            if (!IsOwner)
            {
                return;
            }

            _input = Instantiate(_inputPrefab);

            _characterOwnerBucket.Subscribe<SetCharacterOwnerCommand>(OnSetCharacterOwnerCommand);
            _healthBucket.Subscribe<DeadHealthCommand>(OnDeadHealthCommandClient);
            _inputBucket.Subscribe<InputEnableCommand>(OnInputEnableCommand);
        }

        private void OnClientSideDeinitialized()
        {
            if (!IsOwner)
            {
                return;
            }

            _characterOwnerBucket?.Unsubscribe<SetCharacterOwnerCommand>(OnSetCharacterOwnerCommand);
            _healthBucket?.Unsubscribe<DeadHealthCommand>(OnDeadHealthCommandClient);
            _inputBucket?.Unsubscribe<InputEnableCommand>(OnInputEnableCommand);

            if (_input != null)
            {
                Destroy(_input.gameObject);
                _input = null;
            }

            _characterViewClientSide = null;
        }

        private void OnDeadHealthCommandClient(DeadHealthCommand command)
        {
            if (!_characterViewClientSide.Guid.Equals(command.ToEntityKey)
                || _input == null)
            {
                return;
            }

            _input.SetEnable(false);
        }

        private void OnSetCharacterOwnerCommand(SetCharacterOwnerCommand command)
        {
            if (OwnerId != command.OwnerId)
            {
                return;
            }

            _characterViewClientSide = ViewsContainer.GetView<CharacterView>(command.EntityKey);
            FindObjectOfType<CameraFollow>().SetTarget(_characterViewClientSide.transform, _characterViewClientSide.AimingControl);
            _input.SetEnable(true);
        }

        private void OnInputEnableCommand(InputEnableCommand command)
        {
            if (_characterViewClientSide == null
                || !_characterViewClientSide.Guid.Equals(command.OwnerKey)
                || _input == null)
            {
                return;
            }

            _input.SetEnable(command.Enable);
        }
    }
}