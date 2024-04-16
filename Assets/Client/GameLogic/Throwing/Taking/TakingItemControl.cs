using System.Collections.Generic;
using Client.GameLogic.Characters;
using Client.GameLogic.Eating;
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
        [SerializeField] private EatingArmControl _eatngArm;

        private readonly HashSet<TakingItemViewBase> _itemsNearby = new HashSet<TakingItemViewBase>();

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
            if (nearestItem == null || nearestItem.HasOwner)
            {
                return;
            }

            if (nearestItem is EatingItemView eatingItem)
            {
                TakeEatingItem(command, eatingItem);
            }
            else
            {
                TakeThrowingItem(command, nearestItem);
            }
        }

        private void TakeThrowingItem(in TakingInputCommand command, TakingItemViewBase item)
        {
            if (_takingArm.HasItem)
            {
                return;
            }

            _itemsNearby.Remove(item);
            item.Take(_characterView.Guid);
            _takingArm.SetItem(item);
        }

        private void TakeEatingItem(in TakingInputCommand command, EatingItemView item)
        {
            if (_eatngArm.HasItem)
            {
                return;
            }
            
            _itemsNearby.Remove(item);
            item.Take(_characterView.Guid);
            _takingArm.SetItem(item);
        }

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

        private TakingItemViewBase GetNearestItem()
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
    }
}