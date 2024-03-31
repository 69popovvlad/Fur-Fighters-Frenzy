using Client.GameLogic.Health;
using Client.GameLogic.Inputs;
using Client.Network.GameLogic.Characters;
using Core.Assets;
using Core.Ioc;
using FishNet.Object;

namespace Client.Network.GameLogic
{
    public partial class CharacterSpawner : NetworkBehaviour
    {
        private CharacterOwnerBucket _characterOwnerBucket;
        private AssetsLoader _assetsLoader;
        private HealthBucket _healthBucket;
        private InputBucket _inputBucket;

        private void Awake()
        {
            var ioc = Ioc.Instance;
            _assetsLoader = ioc.Get<AssetsLoader>();
            _characterOwnerBucket = ioc.Get<CharacterOwnerBucket>();
            _healthBucket = ioc.Get<HealthBucket>();
            _inputBucket = ioc.Get<InputBucket>();
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            if (IsServerInitialized)
            {
                OnServerSideInitialized();
            }
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            if (IsOwner)
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
        }

        public override void OnStopClient()
        {
            base.OnStopClient();

            if (IsOwner)
            {
                OnClientSideDeinitialized();
            }
        }
    }
}