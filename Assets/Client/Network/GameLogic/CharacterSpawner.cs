using Client.GameLogic.Health;
using Client.GameLogic.Inputs;
using Client.Network.GameLogic.Characters;
using Core.Assets;
using Core.Ioc;
using FishNet.Object;
using UnityEngine;

namespace Client.Network.GameLogic
{
    public partial class CharacterSpawner : NetworkBehaviour
    {
        [SerializeField] private PlayerInputHandler _inputPrefab;

        private CharacterOwnerBucket _characterOwnerBucket;
        private AssetsLoader _assetsLoader;
        private HealthBucket _healthBucket;
        private PlayerInputHandler _input;

        private void Awake()
        {
            var ioc = Ioc.Instance;
            _assetsLoader = ioc.Get<AssetsLoader>();
            _characterOwnerBucket = ioc.Get<CharacterOwnerBucket>();
            _healthBucket = ioc.Get<HealthBucket>();
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            if (IsServerInitialized)
            {
                OnServerSideInitialized();
            }
            
            if (IsClientInitialized)
            {
                OnClientSideInitialized();
            }
        }

        public override void OnStopNetwork()
        {
            base.OnStopNetwork();
            
            if (IsServerInitialized)
            {
                OnServerSideDeinitialized();
            }
            
            if (IsClientInitialized)
            {
                OnClientSideDeinitialized();
            }
        }
    }
}