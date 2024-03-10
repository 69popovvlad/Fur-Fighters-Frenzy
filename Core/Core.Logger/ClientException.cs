using System;
using UnityEngine;

namespace Core.Logger
{
    public class ClientException: Exception
    {
        public ClientException(string message): base(message)
        {
            Debug.LogError(message);
        }
    }
}
