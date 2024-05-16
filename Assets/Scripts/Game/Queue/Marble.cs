using Combat;

namespace Game.Queue
{
    public class Marble
    {
        public float InQueueSpeed { get; set; }
        public float CurrentWaitingTime { get; set; }
        public int CurrentGoalIndex { get; set; }
        public ProjectileModel ProjectileModel { get; set; }


        public Marble(float inQueueSpeed, ProjectileModel projectileModel)
        {
            InQueueSpeed = inQueueSpeed;
            ProjectileModel = projectileModel;
        }
    }
}