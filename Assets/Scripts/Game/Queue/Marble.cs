using System;
using Game.Combat;
using UnityEngine;

namespace Game.Queue
{
    [Serializable]
    public class Marble : IQueueable
    {
        public float InQueueSpeed { get; set; }
        public float CurrentTravelTime { get; set; }
        public int CurrentGoalIndex { get; set; }
        public Sprite Sprite { get; set; }
        public float EndY { get; set; }
        public float CurrY { get; set; }

        public ProjectileModel ProjectileModel { get; private set; }
        public float TotalTravelTime { get; set; }

        public Marble(float inQueueSpeed, ProjectileModel projectileModel, Sprite sprite)
        {
            InQueueSpeed = inQueueSpeed;
            ProjectileModel = projectileModel;
            Sprite = sprite;
        }
    }
}