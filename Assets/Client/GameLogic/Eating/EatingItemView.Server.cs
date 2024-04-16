using Client.GameLogic.Characters;
using Core.Entities.Views;
using FishNet.Object;
using UnityEngine;

namespace Client.GameLogic.Eating
{
    public partial class EatingItemView
    {
        [ServerRpc(RequireOwnership = false)]
        public override void Take(string ownerKey)
        {
            if (_isTaken)
            {
                return;
            }

            TakeToAllClients(ownerKey);
        }

        [ServerRpc(RequireOwnership = false)]
        public override void Drop(Vector3 direction)
        {
            if (!_isTaken || IsThrowing)
            {
                return;
            }

            var characterView = ViewsContainer.GetView<CharacterView>(_ownerKey);
            if (characterView.Health.Dead)
            {
                return;
            }

            characterView.Health.IncreaseMaxHealth(string.Empty, _healPoints);
            DestroyToAllClients();
            
            Despawn();
        }
    }
}