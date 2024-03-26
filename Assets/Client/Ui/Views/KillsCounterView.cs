using Client.GameLogic.Characters;
using Client.GameLogic.Health;
using Client.GameLogic.Health.Commands;
using Client.Network.GameLogic.Characters;
using Client.Network.GameLogic.Characters.Commands;
using Core.Entities.Views;
using Core.Ioc;
using TMPro;
using UnityEngine;

namespace Client.Ui.Views
{
    public class KillsCounterView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _killsCount;
        [SerializeField] private TextMeshProUGUI _diesCount;

        private HealthBucket _healthBucket;
        private CharacterOwnerBucket _characterOwnerBucket;
        private string _myCharacterEntityKey;
        private int _kills;
        private int _dies;

        private void Awake()
        {
            var ioc = Ioc.Instance;
            _healthBucket = ioc.Get<HealthBucket>();
            _healthBucket.Subscribe<DeadHealthCommand>(OnDeadHealthCommand);

            _characterOwnerBucket = ioc.Get<CharacterOwnerBucket>();
            _characterOwnerBucket.Subscribe<SetCharacterOwnerCommand>(OnSetCharacterOwnerCommand);
        }

        private void OnDestroy()
        {
            _healthBucket?.Unsubscribe<DeadHealthCommand>(OnDeadHealthCommand);
            _characterOwnerBucket?.Unsubscribe<SetCharacterOwnerCommand>(OnSetCharacterOwnerCommand);
        }

        private void OnSetCharacterOwnerCommand(SetCharacterOwnerCommand command)
        {
            var characterView = ViewsContainer.GetView<CharacterView>(command.EntityKey);
            if (characterView.IsOwner)
            {
                _myCharacterEntityKey = command.EntityKey;
            }
        }

        private void OnDeadHealthCommand(DeadHealthCommand command)
        {
            if (string.IsNullOrEmpty(_myCharacterEntityKey))
            {
                return;
            }
            
            if (_myCharacterEntityKey.Equals(command.FromEntityKey))
            {
                _killsCount.text = $"{++_kills}";
                return;
            }

            if (_myCharacterEntityKey.Equals(command.ToEntityKey))
            {
                _diesCount.text = $"{++_dies}";
            }
        }
    }
}