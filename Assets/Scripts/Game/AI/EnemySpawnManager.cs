using System.Collections;
using System.Collections.Generic;
using Combat;
using Game.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AI
{
    public interface IReturner
    {
        void ReturnToPool(Enemy enemy);
    }

    public class EnemySpawnManager : Singleton<EnemySpawnManager>, IReturner 
    {
        [SerializeField, TabGroup("Settings")] private List<Wave> waves;
        [SerializeField, TabGroup("Settings")] private float spawnRadius;
        [SerializeField, TabGroup("Settings")] private List<GameObject> spawnPoints;

        [SerializeField, TabGroup("Events")] private UnityEvent onAllWavesFinished;
        [SerializeField, TabGroup("Events")] private UnityEvent<GameObject> onSpawn;

        [ShowInInspector, ReadOnly, TabGroup("Debug")]
        private int _currentWaveIndex = -1;

        private Wave CurrentWave => _currentWaveIndex >= waves.Count ? null : waves[_currentWaveIndex];

        private float _waveTime;

        private EnemySpawner _spawner;

        public void ReturnToPool(Enemy enemy)
        {
            _spawner.Return(enemy);
        }

        private void Start()
        {
            _spawner = new();

            StartCoroutine(StartNextWave());
        }
        
        private IEnumerator StartNextWave()
        {
            _currentWaveIndex++;

            if (_currentWaveIndex >= waves.Count)
            {
                onAllWavesFinished?.Invoke();
                yield break;
            }

            yield return new WaitForSeconds(CurrentWave.delayAtStart);
            SpawnWave();
        }

        private void SpawnWave()
        {
            foreach (WaveEntry entry in CurrentWave.entries)
            {
                Spawn(entry);
            }

            StartCoroutine(StartNextWave());
        }

        private void Spawn(WaveEntry entry)
        {
            for (int i = 0; i < entry.count; i++)
            {
                var spawnPoint = GetSpawnPoint();
                _spawner.Spawn(entry.enemyModel, spawnPoint);
            }
        }

        private Vector3 GetSpawnPoint()
        {
            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            var randPoint = Random.insideUnitSphere * spawnRadius;
            randPoint = new Vector3(randPoint.x + spawnPoint.transform.position.x
                , spawnPoint.transform.position.y,
                randPoint.z + spawnPoint.transform.position.z);
            return randPoint;
        }
    }
}