using Client.GameLogic.Health;
using Core.Entities.Views;
using UnityEngine;

namespace Client.GameLogic.Characters
{
    public class CharacterView : EntityView
    {
        [SerializeField] private HealthControl _health;
        [SerializeField, Tooltip("Start and max health value at the same time")]
        private int _maxHealth = 10;

        private CharacterEntity _entity;

        public HealthControl Health => _health;

        private void Awake()
        {
            _entity = new CharacterEntity(); // TODO: set guid from network (need update CORE for it)
            Initialize(_entity);

            _health.Initialize(_maxHealth);
        }
    }
}