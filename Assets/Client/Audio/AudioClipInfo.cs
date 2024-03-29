using System;
using UnityEngine;

namespace Client.Audio
{
    [Serializable]
    public struct AudioClipInfo
    {
        public string Key;
        public AudioClip Clip;
    }
}