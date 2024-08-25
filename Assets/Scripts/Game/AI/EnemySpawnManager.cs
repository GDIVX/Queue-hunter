using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Game.Combat;
using Game.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

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
        [SerializeField, TabGroup("Settings")] private Transform playerTransform;
        [SerializeField, TabGroup("Settings")] private List<GameObject> spawnPoints;

        [SerializeField, TabGroup("Events")] private UnityEvent onAllWavesFinished;
        [SerializeField, TabGroup("Events")] private UnityEvent<GameObject> onSpawn;

        [ShowInInspector, ReadOnly, TabGroup("Debug")]
        private int _currentWaveIndex = -1;

        private Wave CurrentWave => _currentWaveIndex >= waves.Count ? null : waves[_currentWaveIndex];

        private float _waveTime;

        private EnemySpawner _spawner;
        private int _enemyCountInWave;


        public void ReturnToPool(Enemy enemy)
        {
            _spawner.Return(enemy);
            enemy.gameObject.SetActive(false);
            UpdateEnemyCount(_enemyCountInWave - 1);
        }

        private void UpdateEnemyCount(int count)
        {
            _enemyCountInWave = count;

            if (count <= 0)
            {
                StartCoroutine(StartNextWave());
            }
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
            StartCoroutine(SpawnWave());
        }

        private IEnumerator SpawnWave()
        {
            foreach (WaveEntry entry in CurrentWave.entries)
            {
                StartCoroutine(Spawn(entry));
                yield return new WaitForSeconds(entry.delayAfterEntry);
            }
        }

        private IEnumerator Spawn(WaveEntry entry)
        {
            for (var i = 0; i < entry.count; i++)
            {
                var spawnPoint = GetSpawnPoint();
                var enemy = _spawner.Spawn(entry.enemyModel, spawnPoint);
                enemy.GetComponent<Health>().OnDestroyed += destroyable => ReturnToPool(enemy);
                enemy.gameObject.SetActive(true);
                yield return new WaitForSeconds(entry.delayBetweenSpawns);
            }
        }

        private Vector3 GetSpawnPoint()
        {
            var spawnPointsNearThePlayer = spawnPoints
                .OrderBy(p => Vector3.Distance(p.transform.position, playerTransform.position))
                .Take(CurrentWave.spawnPointsToUseCount).ToList();

            var spawnPoint = spawnPointsNearThePlayer[Random.Range(0, spawnPointsNearThePlayer.Count)];
            var randPoint = Random.insideUnitSphere * spawnRadius;
            randPoint = new Vector3(randPoint.x + spawnPoint.transform.position.x
                , spawnPoint.transform.position.y,
                randPoint.z + spawnPoint.transform.position.z);
            return randPoint;
        }
    }
}