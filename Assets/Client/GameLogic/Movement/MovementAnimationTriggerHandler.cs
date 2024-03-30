using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Client.GameLogic.Movement
{
    public class MovementAnimationTriggerHandler : MonoBehaviour
    {
        public event Action<int> OnStepTrigger;

        [UsedImplicitly]
        public void OnStepAnimationTrigger(int legIndex)
        {
            OnStepTrigger?.Invoke(legIndex);
        }
    }
}