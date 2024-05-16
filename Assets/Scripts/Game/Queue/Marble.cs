using Combat;
using UnityEngine.Events;

namespace Game.Queue
{
    public class Marble
    {
        public float InQueueSpeed { get; set; }
        public float CurrentWaitingTime { get; set; }
        public int CurrentGoalIndex { get; set; }
        public UnityEvent<Marble> OnInstantiated { get; set; }


        public Marble(float inQueueSpeed, UnityEvent<Marble> onInstantiated)
        {
            InQueueSpeed = inQueueSpeed;
            OnInstantiated = onInstantiated;
        }
    }
}