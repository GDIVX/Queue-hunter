namespace Assets.Scripts.Engine.ECS
{
    public class EntityCreatedSignal
    {
        public EntityCreatedSignal(Entity entity)
        {
            Entity = entity;
        }

        public Entity Entity { get; private set; }
    }
}
