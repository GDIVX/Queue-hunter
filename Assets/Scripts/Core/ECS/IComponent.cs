namespace Assets.Scripts.Engine.ECS
{
    public interface IComponent
    {
        public IComponent Clone();

        public void SetParent(IEntity entity);

        public IEntity GetParent();

        bool IsActive { get; set; }
    }
}