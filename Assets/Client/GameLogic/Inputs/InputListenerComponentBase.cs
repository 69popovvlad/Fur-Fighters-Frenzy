using UnityEngine;

namespace Client.GameLogic.Inputs
{
    public abstract class InputListenerNetworkComponentBase: MonoBehaviour
    {
        public abstract void InputsInitialize(bool isOwner);
    }
}