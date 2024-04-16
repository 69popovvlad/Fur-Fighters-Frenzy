using Client.GameLogic.Inputs;
using FishNet.Object;
using UnityEngine;

namespace Client.Network.GameLogic.Characters
{
    public class CheckOwnerControl : NetworkBehaviour
    {
        [SerializeField, Tooltip("Components that should only be on the client character")]
        private InputListenerNetworkComponentBase[] _onlyOwnerInputListeners;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            var isOwner = Owner.IsLocalClient;
            for (int i = 0, iLen = _onlyOwnerInputListeners.Length; i < iLen; ++i)
            {
                _onlyOwnerInputListeners[i].InputsInitialize(isOwner);
            }

            Destroy(this);
        }
    }
}