using System;
using AI;
using Game.AI;
using Game.Combat;
using Game.Utility;
using JetBrains.Annotations;
using UnityEngine;

namespace Combat
{
    public class PoolOnDeath : MonoBehaviour, IInit<EnemyModel>
    {
        [SerializeField] private Health health;
        [SerializeField] private Enemy enemy;

        public void Init(EnemyModel input)
        {
            health ??= GetComponent<Health>();
            enemy ??= GetComponent<Enemy>();
            EnemySpawnManager spawnManager = EnemySpawnManager.Instance;
            spawnManager.ReturnToPool(enemy);
        }
    }
}