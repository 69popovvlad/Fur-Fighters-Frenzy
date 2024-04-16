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
        [SerializeField] private Transform[] _path;

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

            var instance = Instantiate(_eatingRatPrefab, _path[0].position, Quaternion.identity);
            ServerManager.Spawn(instance.gameObject, Owner);
            // var pathIndex = Random.Range(0, _paths.Length);
            instance.InitializePath(_path, _pathDuration);

            _delayLeft = Random.Range(_minSpawnDelay, _maxSpawnDelay);
        }
    }
}