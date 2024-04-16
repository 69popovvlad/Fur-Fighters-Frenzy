using Client.Audio;
using Client.GameLogic.Punching;
using Core.Ioc;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Client.GameLogic.Throwing.Taking
{
    public class TakingArmControl : NetworkBehaviour
    {
        [SerializeField] protected ArmPunchingControl _armPunchingControl;
        [SerializeField] private ChainIKConstraint _armIK;
        [SerializeField] private Transform _takingItemAim;
        [SerializeField] private Transform _itemParent;
        [SerializeField] private Transform _throwingDirectionAim;

        [Header("Animation")]
        [SerializeField] private AnimationCurve _takingCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [SerializeField] private float _takingDuration = 0.2f;
        [SerializeField] private float _comebackDuration = 0.3f;

        private AudioPlayerService _audioPlayerService;
        private TakingItemViewBase _item;
        private float _punchT;
        private bool _isTaking;

        public bool HasItem => _item != null;

        private void Awake()
        {
            _audioPlayerService = Ioc.Instance.Get<AudioPlayerService>();
        }

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

        public override void OnStopClient()
        {
            base.OnStopClient();

            if (HasItem)
            {
                _item.Drop(Vector3.zero);

                // If client will reconect, this object should be without owner locally
                _item.DropToAllClients(Vector3.zero);
            }
        }

        protected virtual void SetItemOnClientInternal(TakingItemViewBase item)
        {
            /* Nothing to do */
        }

        protected virtual void OnPunchedInternal()
        {
            /* Nothing to do */
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetParent()
        {
            SetParentToAllClients();
        }

        [ObserversRpc(RunLocally = true)]
        private void SetParentToAllClients()
        {
            if (_item == null)
            {
                return;
            }

            var itemTransform = _item.transform;
            itemTransform.SetParent(_itemParent);
            itemTransform.localPosition = _item.TakingItemOffset;
            itemTransform.localRotation = Quaternion.Euler(_item.TakingItemRotation);
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
        public void SetItem(TakingItemViewBase item)
        {
            SetItemToAllClients(item);
        }

        [ObserversRpc(RunLocally = true)]
        private void SetItemToAllClients(TakingItemViewBase item)
        {
            _armPunchingControl.OnPunched += OnPunched;
            _armPunchingControl.SetDontEnableColliderToggle();

            _item = item;
            _takingItemAim.position = item.transform.position;
            _isTaking = true;
            _punchT = 0;

            SetItemOnClientInternal(item);
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
            _item.Drop(direction);
            _item = null;

            _audioPlayerService.PlayClip(transform.position, "throwing");

            OnPunchedInternal();
        }
    }
}