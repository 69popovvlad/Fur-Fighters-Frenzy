using Core.Ioc;
using UnityEngine;

namespace Client.Audio
{
    public class AudioClipsHolder: MonoBehaviour
    {
        [SerializeField] private AudioPlayerView _audioPlayerPrefab;
        [SerializeField] private AudioClipInfo[] _clipsInfos;

        private void Awake()
        {
            var audioBucket = Ioc.Instance.Get<AudioPlayerService>();
            audioBucket.Initialize(_audioPlayerPrefab);

            for(int i = 0, iLen = _clipsInfos.Length; i < iLen; ++i)
            {
                audioBucket.RegisterClip(_clipsInfos[i]);   
            }

            Destroy(gameObject);
        }
    }
}