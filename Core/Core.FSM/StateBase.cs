using System;

namespace Core.FSM
{
    public abstract class StateBase<T> where T: Enum
    {
        public T StateKey { get; private set; }

        protected StateBase(T stateKey)
        {
            StateKey = stateKey;
        }

        public abstract void EnterState();

        public abstract void UpdateState(float delta);

        public abstract void ExitState();

        public abstract T GetNextStateKey();
    }
}