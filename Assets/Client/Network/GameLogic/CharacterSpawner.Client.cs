using Client.GameLogic.CameraLogic;
using Client.GameLogic.Characters;
using Client.Network.GameLogic.Characters.Commands;
using Core.Entities.Views;

namespace Client.Network.GameLogic
{
    public partial class CharacterSpawner
    {
        private void OnClientSideInitialized()
        {
            if (!IsOwner)
            {
                return;
            }
            
            _input = Instantiate(_inputPrefab);

            _characterOwnerBucket.Subscribe<SetCharacterOwnerCommand>(OnSetCharacterOwnerCommand);
        }
        
        private void OnClientSideDeinitialized()
        {
            if (!IsOwner)
            {
                return;
            }
            
            _characterOwnerBucket?.Unsubscribe<SetCharacterOwnerCommand>(OnSetCharacterOwnerCommand);
            Destroy(_input.gameObject);   
        }
        
        private void OnSetCharacterOwnerCommand(SetCharacterOwnerCommand command)
        {
            if (OwnerId != command.OwnerId)
            {
                return;
            }

            var characterView = ViewsContainer.GetView<CharacterView>(command.EntityKey);
            FindObjectOfType<CameraFollow>().SetTarget(characterView.transform, characterView.AimingControl);
        }
    }
}