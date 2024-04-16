using System;
using Client.Audio;
using Client.GameLogic.Throwing.Taking;
using Core.Ioc;
using UnityEngine;

namespace Client.GameLogic.Eating
{
    public partial class EatingItemView : TakingItemViewBase
    {
        public override event Action OnTaken;
        public override event Action OnDropped;

        [SerializeField] private int _healPoints = 1;
        [SerializeField] private GameObject _destroyParticlePrefab;

        private EatingItemEntity _entity;
        private AudioPlayerService _audioPlayerService;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            _entity = new EatingItemEntity(ObjectId.ToString());
            Initialize(_entity);
        }

        private void Awake()
        {
            _audioPlayerService = Ioc.Instance.Get<AudioPlayerService>();
        }
    }
}