using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Client.Media
{
    public class AnimationEnableControl : MonoBehaviour
    {
        private readonly int StartHash = Animator.StringToHash("Start");

        [SerializeField] private Animator[] _animators;
        [SerializeField] private float _animationsDelay = 0.5f;

        [UsedImplicitly]
        public void EnableAllAnimators()
        {
            StartCoroutine(nameof(AnimatorEnableCoroutine));
        }

        private IEnumerator AnimatorEnableCoroutine() {
            for (int i = 0, iLen = _animators.Length; i < iLen; ++i)
            {
                _animators[i].SetTrigger(StartHash);
                yield return new WaitForSeconds(_animationsDelay);
            }
        }
    }
}