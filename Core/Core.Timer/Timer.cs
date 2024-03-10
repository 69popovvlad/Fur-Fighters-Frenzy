using System;

namespace Core.Timer
{
    public class Timer: IDisposable
    {
        protected Action OnDone;
        protected float TimeLeft;

        public Timer(float duration, Action onDone)
        {
            TimeLeft = duration;
            OnDone = onDone;
        }

        public void Dispose()
        {
            OnDone = null;
        }

        /// <summary>
        /// Return true if time has gone.
        /// </summary>
        /// <returns>The time has gone?</returns>
        internal virtual bool DecreaseLefTimer(float delta)
        {
            TimeLeft -= delta;
            if (TimeLeft > 0)
            {
                return false;
            }
            
            OnDone?.Invoke();
            return true;
        }
    }
}