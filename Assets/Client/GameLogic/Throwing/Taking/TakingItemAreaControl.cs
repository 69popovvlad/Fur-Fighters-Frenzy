using System.Collections.Generic;
using Client.GameLogic.Characters;
using UnityEngine;

namespace Client.GameLogic.Throwing.Taking
{
    public class TakingItemAreaControl : MonoBehaviour
    {
        [SerializeField] private CharacterView _characterView;

        private readonly HashSet<TakingItemViewBase> _itemsNearby = new HashSet<TakingItemViewBase>();

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<TakingItemViewBase>(out var item))
            {
                return;
            }

            _itemsNearby.Add(item);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<TakingItemViewBase>(out var item))
            {
                return;
            }

            _itemsNearby.Remove(item);
        }

        internal TakingItemViewBase GetNearestItem()
        {
            if (_itemsNearby.Count < 1)
            {
                return null;
            }

            var nearestItem = default(TakingItemViewBase);
            var minDistance = float.MaxValue;
            var myPosition = transform.position;

            foreach (var item in _itemsNearby)
            {
                if (item == null)
                {
                    continue;
                }

                var distance = Vector3.Distance(myPosition, item.transform.position);
                if (distance >= minDistance)
                {
                    continue;
                }

                minDistance = distance;
                nearestItem = item;
            }

            return nearestItem;
        }

        internal void TakeItem(TakingItemViewBase item)
        {
            _itemsNearby.Remove(item);
            item.Take(_characterView.Guid);
        }
    }
}