using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Client.GameLogic.Slipping
{
    public class SlippingAnimationTriggerHandler: MonoBehaviour
    {
        public event Action<bool> OnSlipTrigger;

        [UsedImplicitly]
        public void OnSlipStartAnimationTrigger()
        {
            OnSlipTrigger?.Invoke(true);
        }

        [UsedImplicitly]
        public void OnSlipStopAnimationTrigger()
        {
            OnSlipTrigger?.Invoke(false);
        }
    }
}