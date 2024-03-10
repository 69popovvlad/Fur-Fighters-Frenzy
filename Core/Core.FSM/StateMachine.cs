using System;
using System.Collections.Generic;

namespace Core.FSM
{
    public abstract class StateMachine<T> where T: Enum
    {
        protected readonly Dictionary<T, StateBase<T>> States = new Dictionary<T, StateBase<T>>();

        protected StateBase<T> CurrentState;
        protected bool IsTransitioning;

        public virtual void Update(float delta)
        {
            var nextStateKey = CurrentState.GetNextStateKey();
            if (nextStateKey.Equals(CurrentState.StateKey))
            {
                CurrentState.UpdateState(delta);    
                return;
            }
            
            
            if(!IsTransitioning)
            {
                TransitionToState(nextStateKey);
            }
        }

        protected void Start()
        {
            CurrentState.EnterState();
        }

        protected void AddState(StateBase<T> state)
        {
            States[state.StateKey] = state;
        }

        private void TransitionToState(T stateKey)
        {
            IsTransitioning = true;
            {
                CurrentState.ExitState();
                CurrentState = States[stateKey];
                CurrentState.EnterState();
            }
            IsTransitioning = false;
        }
    }
}