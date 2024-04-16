using System;
using Client.GameLogic.Throwing.Taking;
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

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            _entity = new EatingItemEntity(ObjectId.ToString());
            Initialize(_entity);
        }
    }
}