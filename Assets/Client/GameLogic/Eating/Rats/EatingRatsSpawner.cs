using FishNet.Object;
using UnityEngine;

namespace Client.GameLogic.Eating.Rats
{
    public class EatingRatsSpawner : NetworkBehaviour
    {
        [SerializeField] private float _minSpawnDelay = 5;
        [SerializeField] private float _maxSpawnDelay = 15;
        [SerializeField] private float _pathDuration = 10;
        [SerializeField] private EatingRatView _eatingRatPrefab;
        [SerializeField] private PathStructure[] _paths;

        private float _delayLeft;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            if (!IsServerInitialized && IsClientInitialized)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _delayLeft = Random.Range(_minSpawnDelay, _maxSpawnDelay);
        }

        private void Update()
        {
            _delayLeft -= Time.deltaTime;
            if (_delayLeft > 0)
            {
                return;
            }

            var pathIndex = Random.Range(0, _paths.Length);
            var path = _paths[pathIndex];
            var instance = Instantiate(_eatingRatPrefab, path.Points[0].position, Quaternion.identity);
            ServerManager.Spawn(instance.gameObject, Owner);
            instance.InitializePath(path.Points, _pathDuration);

            _delayLeft = Random.Range(_minSpawnDelay, _maxSpawnDelay);
        }
    }
}