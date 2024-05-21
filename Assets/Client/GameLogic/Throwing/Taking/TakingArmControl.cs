using System;
using Client.GameLogic.Arm;
using Client.GameLogic.Arm.IK;
using Client.GameLogic.Inputs.Commands.Taking;
using FishNet.Object;
using UnityEngine;

namespace Client.GameLogic.Throwing.Taking
{
    public class TakingArmControl : NetworkBehaviour, ArmStateControlBase<TakingInputCommand>
    {
        public event Action OnItemTaken;
        public event Action OnNoItem;

        [SerializeField] private ArmType _armType;
        [SerializeField] private ArmIKControl _armTakingIk;
        [SerializeField] private Transform _takingItemAim;
        [SerializeField] private Transform _itemParent;

        [SerializeField] private TakingItemAreaControl _takingItemArea;

        protected TakingItemViewBase _item;

        public bool HasItem => _item != null;

        public TakingItemViewBase Item => _item;

        public Transform ItemParent => _itemParent;

        public override void OnStopClient()
        {
            base.OnStopClient();

            if (HasItem)
            {
                var itemTemp = _item;
                DropItem(Vector3.zero);

                // If client will reconect, this object should be without owner locally
                itemTemp.DropToAllClients(Vector3.zero);
            }
        }

        public void Enable(bool enabled) =>
            this.enabled = enabled;

        public void Enter() { /* Nothing to do */ }

        public void Exit() { /* Nothing to do */ }

        public void OnInputCommand(TakingInputCommand inputCommand)
        {
            if (HasItem)
            {
                OnNoItem?.Invoke();
                return;
            }

            var nearestItem = _takingItemArea.GetNearestItem();
            if (nearestItem == null || nearestItem.HasOwner || nearestItem.TargetArm != _armType)
            {
                OnNoItem?.Invoke();
                return;
            }

            _takingItemArea.TakeItem(nearestItem);
            SetItem(nearestItem);
        }

        public void DropItem(Vector3 direction)
        {
            _item.Drop(direction);
            _item = null;
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetParentToServer() =>
            SetParentToAllClients();

        [ObserversRpc(RunLocally = true)]
        private void SetParentToAllClients()
        {
            if (_item == null)
            {
                OnNoItem?.Invoke();
                return;
            }

            var itemTransform = _item.transform;
            itemTransform.SetParent(_itemParent);
            itemTransform.localPosition = _item.TakingItemOffset;
            itemTransform.localRotation = Quaternion.Euler(_item.TakingItemRotation);

            OnItemTaken?.Invoke();
        }

        [ServerRpc]
        public void SetItem(TakingItemViewBase item)
        {
            SetItemToAllClients(item);
        }

        [ObserversRpc(RunLocally = true)]
        private void SetItemToAllClients(TakingItemViewBase item)
        {
            _item = item;
            _takingItemAim.position = item.transform.position;
            _armTakingIk.OnTargetReached += OnTargetItemReached;
            _armTakingIk.Play();
        }

        private void OnTargetItemReached()
        {
            _armTakingIk.OnTargetReached -= OnTargetItemReached;
            SetParentToServer();
        }
    }
}