using Combat;
using Game.Combat;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Queue
{
    public class Marble : IQueueable
    {
        public float InQueueSpeed { get; set; }
        public float CurrentTravelTime { get; set; }
        public int CurrentGoalIndex { get; set; }
        public Sprite Sprite { get; set; }

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