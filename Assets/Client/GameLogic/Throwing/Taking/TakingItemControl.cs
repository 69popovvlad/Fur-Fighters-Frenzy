using System.Collections.Generic;
using Client.GameLogic.Characters;
using Client.GameLogic.Inputs;
using Client.GameLogic.Inputs.Commands.Taking;
using Core.Ioc;
using UnityEngine;

namespace Client.GameLogic.Throwing.Taking
{
    public class TakingItemControl : MonoBehaviour
    {
        [SerializeField] private CharacterView _characterView;
        [SerializeField] private TakingArmControl _takingArm;

        private readonly HashSet<ThrowingItemView> _itemsNearby = new HashSet<ThrowingItemView>();

        private InputBucket _inputBucket;

        private void Awake()
        {
            _inputBucket = Ioc.Instance.Get<InputBucket>();
            _inputBucket.Subscribe<TakingInputCommand>(OnTakingInputCommand);
        }

        private void OnDestroy()
        {
            _inputBucket.Unsubscribe<TakingInputCommand>(OnTakingInputCommand);
        }

        private void OnTakingInputCommand(TakingInputCommand command)
        {
            var nearestItem = GetNearestItem();
            if (nearestItem == null)
            {
                return;
            }

            nearestItem.Take(_characterView.Guid);
            _takingArm.SetItem(nearestItem);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<ThrowingItemView>(out var item))
            {
                return;
            }

            _itemsNearby.Add(item);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<ThrowingItemView>(out var item))
            {
                return;
            }

            _itemsNearby.Remove(item);
        }

        private ThrowingItemView GetNearestItem()
        {
            if (_itemsNearby.Count < 1)
            {
                return null;
            }

            var nearestItem = default(ThrowingItemView);
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
    }
}