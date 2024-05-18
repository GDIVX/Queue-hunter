using Combat;
using Game.Combat;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Queue
{
    public class Marble : IQueueable
    {
        public float InQueueSpeed { get; set; }
        public float CurrentWaitingTime { get; set; }
        public int CurrentGoalIndex { get; set; }
        public GameObject UIView { get; private set; }

        public ProjectileModel ProjectileModel { get; private set; }

        public Marble(float inQueueSpeed, ProjectileModel projectileModel, GameObject UIView)
        {
            InQueueSpeed = inQueueSpeed;
            ProjectileModel = projectileModel;
            this.UIView = UIView;
        }
    }
}