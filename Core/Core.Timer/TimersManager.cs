using System;
using System.Collections.Generic;

namespace Core.Timer
{
    public static class TimersManager
    {
        private static readonly List<Timer> _timers = new List<Timer>();
        
        public static Timer CreateTimer(float duration, Action onDone)
        {
            var timer = new Timer(duration, onDone);
            _timers.Add(timer);
            return timer;
        }

        public static CyclicTimer CreateCyclicTimer(float duration, Action onDone)
        {
            var timer = new CyclicTimer(duration, onDone);
            _timers.Add(timer);
            return timer;
        }

        public static void ReleaseTimer(Timer timer)
        {
            if (timer != null)
            {
                timer.Dispose();
                _timers.Remove(timer);
            }
        }
        
        
        /// <summary>
        /// Calling from ApplicationContext
        /// </summary>
        internal static void OnUpdate(float delta)
        {
            for (var i = _timers.Count - 1; i >= 0; --i)
            {
                var timer = _timers[i];
                if (timer.DecreaseLefTimer(delta))
                {
                    _timers.RemoveAt(i);
                }
            }
        }
    }
}