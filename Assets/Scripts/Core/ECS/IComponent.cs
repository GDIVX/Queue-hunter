namespace Assets.Scripts.Engine.ECS
{
    public interface IComponent
    {
        public IComponent Clone();

        public void SetParent(Entity entity);

        public Entity GetParent();

        bool IsActive { get; set; }
    }
}