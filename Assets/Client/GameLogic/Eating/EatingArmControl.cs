using Client.GameLogic.Throwing.Taking;
using UnityEngine;

namespace Client.GameLogic.Eating
{
    public class EatingArmControl : TakingArmControl
    {
        [SerializeField] private Transform _eatingAimParent;
        [SerializeField] private Transform _punchAim;

        private Transform _lastPunchAimParent;
        private Vector3 _lastPunchAimPosition;

        protected override void SetItemOnClientInternal(TakingItemViewBase item)
        {
            base.SetItemOnClientInternal(item);

            _lastPunchAimParent = _punchAim.parent;
            _lastPunchAimPosition = _punchAim.localPosition;

            _punchAim.SetParent(_eatingAimParent);
            _punchAim.localPosition = Vector3.zero;
        }

        protected override void OnPunchedInternal()
        {
            base.OnPunchedInternal();

            _armPunchingControl.OnPunchReturned += OnPunchReturned;
        }

        private void OnPunchReturned()
        {
            _armPunchingControl.OnPunchReturned -= OnPunchReturned;

            _punchAim.SetParent(_lastPunchAimParent);
            _punchAim.localPosition = _lastPunchAimPosition;
        }
    }
}