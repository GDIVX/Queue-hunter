using System;
using System.Collections.Generic;
using AI;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "Wave", menuName = "Game/Wave", order = 0)]
    public class Wave : ScriptableObject
    {
        public List<WaveEntry> entries;
        [Min(0)] public float delayAtStart;
        public int spawnPointsToUseCount;
    }

    [Serializable]
    public struct WaveEntry
    {
        public EnemyModel enemyModel;
        public int count;
        [Min(0)] public float spawnWindup;
        public float delayAfterEntry;

        public WaveEntry Clone()
        {
            WaveEntry clone = new WaveEntry
            {
                enemyModel = enemyModel,
                count = count
            };
            return clone;
        }
    }
}