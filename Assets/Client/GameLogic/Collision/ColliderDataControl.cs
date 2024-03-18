using Client.GameLogic.Characters;
using UnityEngine;

namespace Client.GameLogic.Collision
{
    public class ColliderDataControl: MonoBehaviour
    {
        [SerializeField] private CharacterView _characterView;
        [SerializeField] private string _onCollisionEnterKey;
        
        private int _id;

        public int Id => _id;

        public string OnCollisionEnterKey => _onCollisionEnterKey;

        public string CharacterEntityKey => _characterView.Guid;

        internal void SetId(int id)
        {
            _id = id;
        }
    }
}