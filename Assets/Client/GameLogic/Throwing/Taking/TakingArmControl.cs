using Client.GameLogic.Punching;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Client.GameLogic.Throwing.Taking
{
    public class TakingArmControl : NetworkBehaviour
    {
        [SerializeField] private ArmPunchingControl _armPunchingControl;
        [SerializeField] private ChainIKConstraint _armIK;
        [SerializeField] private Transform _takingItemAim;
        [SerializeField] private Transform _itemParent;
        [SerializeField] private Transform _throwingDirectionAim;
        
        [Header("Offset")]
        [SerializeField] private Vector3 _takingItemOffset;
        [SerializeField] private Vector3 _takingItemRotation;

        [Header("Animation")]
        [SerializeField] private AnimationCurve _takingCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [SerializeField] private float _takingDuration = 0.2f;
        [SerializeField] private float _comebackDuration = 0.3f;

        private ThrowingItemView _item;
        private float _punchT;
        private bool _isTaking;

        private void Update()
        {
            if (!_isTaking)
            {
                CalculateTakingReturn();
                return;
            }

            CalculateTaking();
        }

        private void CalculateTaking()
        {
            if (_punchT >= 1)
            {
                _isTaking = false;
                SetParent();
                return;
            }

            _punchT += Time.deltaTime / _takingDuration;
            _armIK.weight = _takingCurve.Evaluate(_punchT);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetParent()
        {
            SetParentToAllClients();
        }

        [ObserversRpc(RunLocally = true)]
        private void SetParentToAllClients()
        {
            var itemTransform = _item.transform;
            itemTransform.SetParent(_itemParent);
            itemTransform.localPosition = _takingItemOffset;
            itemTransform.localRotation = Quaternion.Euler(_takingItemRotation);
        }

        private void CalculateTakingReturn()
        {
            if (_punchT <= 0)
            {
                return;
            }

            _punchT -= Time.deltaTime / _comebackDuration;
            _armIK.weight = _takingCurve.Evaluate(_punchT);
        }

        [ServerRpc]
        public void SetItem(ThrowingItemView item)
        {
            SetItemToAllClients(item);
        }

        [ObserversRpc(RunLocally = true)]
        private void SetItemToAllClients(ThrowingItemView item)
        {
            _armPunchingControl.OnPunched += OnPunched;
            _armPunchingControl.SetDontEnableColliderToggle();

            _item = item;
            _takingItemAim.position = item.transform.position;
            _isTaking = true;
            _punchT = 0;
        }

        private void OnPunched()
        {
            _armPunchingControl.OnPunched -= OnPunched;

            if (_item == null)
            {
                return;
            }

            if (_isTaking)
            {
                _isTaking = false;
                _punchT = 0;
            }

            var direction = _throwingDirectionAim.position - _itemParent.position;
            _item.Throw(direction);
            _item = null;
        }
    }
}