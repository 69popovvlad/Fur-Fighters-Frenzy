using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Client.GameLogic.Kicking
{
    public class LegKickAnimationTriggerHandler : MonoBehaviour
    {
        public event Action<int, bool> OnKickTrigger;

        [UsedImplicitly]
        public void OnKickStartAnimationTrigger(int legIndex)
        {
            OnKickTrigger?.Invoke(legIndex, true);
        }

        [UsedImplicitly]
        public void OnKickStopAnimationTrigger(int legIndex)
        {
            OnKickTrigger?.Invoke(legIndex, false);
        }
    }
}