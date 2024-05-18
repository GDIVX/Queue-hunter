namespace Game.Queue
{
    public interface IQueueable
    {
        public float InQueueSpeed { get; set; }
        public float CurrentTravelTime { get; set; }
        public int CurrentGoalIndex { get; set; }
    }
}