using System;
using UnityEngine;

namespace Client.Bootstrappers
{
    public abstract class BootstrapperBase
    {
        public abstract string Name { get; }
        public bool _finished;
        
        internal event Action OnEnd;

        public virtual void Run()
        {
            Debug.Log($"{GetType()} was started");
            End();
        }

        public virtual void Update(float delta)
        {
            // no op
        }

        public virtual void End()
        {
            _finished = true;
            OnEnd?.Invoke();
        }

        public bool IsFinished() => 
            _finished;
    }
}
