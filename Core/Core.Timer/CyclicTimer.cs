using System;

namespace Core.Timer
{
    public class CyclicTimer: Timer
    {
        private readonly float _duration;
        
        public CyclicTimer(float duration, Action onDone):
            base(duration, onDone)
        {
            _duration = duration;
        }
        
        /// <summary>
        /// Return false everytime, but reset timer when it's gone.
        /// </summary>
        /// <returns>False btw</returns>
        internal override bool DecreaseLefTimer(float delta)
        {
            TimeLeft -= delta;
            if (TimeLeft <= 0)
            {
                OnDone?.Invoke();
                TimeLeft = _duration;
            }
            
            return false;
        }
    }
}