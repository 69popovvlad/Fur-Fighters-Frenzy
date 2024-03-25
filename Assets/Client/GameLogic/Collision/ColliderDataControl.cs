using Client.Network.Entities;
using UnityEngine;

namespace Client.GameLogic.Collision
{
    public class ColliderDataControl: MonoBehaviour
    {
        [SerializeField] private NetworkEntityView _networkEntityView;
        [SerializeField] private string _onCollisionEnterKey;
        
        private int _id;

        public int Id => _id;

        public string OnCollisionEnterKey => _onCollisionEnterKey;

        public string CharacterEntityKey => _networkEntityView.Guid;

        internal void SetId(int id)
        {
            _id = id;
        }
    }
}