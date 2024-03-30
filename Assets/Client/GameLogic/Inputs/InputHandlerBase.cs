using System;
using UnityEngine;

namespace Client.GameLogic.Inputs
{
    public abstract class InputHandlerBase: MonoBehaviour
    {
        public string OwnerKey { get; private set; }
        
        public string CurrentEntityKey { get; protected set; }

        public abstract void Initialize();
        
        public abstract void SetEnable(bool enable);

        protected virtual void AwakeInternal()
        {
            /* Nothing to do */
        }
        
        private void Awake()
        {
            OwnerKey = Guid.NewGuid().ToString();
            
            AwakeInternal();
        }
    }
}