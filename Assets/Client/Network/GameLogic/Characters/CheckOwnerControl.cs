using FishNet.Object;
using UnityEngine;

namespace Client.Network.GameLogic.Characters
{
    public class CheckOwnerControl : NetworkBehaviour
    {
        [SerializeField, Tooltip("Components that should only be on the client character")]
        private Component[] _onlyOwnerComponents;

        public override void OnStartClient()
        {
            if (IsOwner)
            {
                return;
            }

            for (int i = 0, iLen = _onlyOwnerComponents.Length; i < iLen; ++i)
            {
                Destroy(_onlyOwnerComponents[i]);
            }

            Destroy(this);
        }
    }
}