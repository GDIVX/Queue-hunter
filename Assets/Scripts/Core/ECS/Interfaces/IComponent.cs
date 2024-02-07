namespace Assets.Scripts.Engine.ECS
{
    public interface IComponent : IActivable
    {
        public IComponent Clone();

        public void SetParent(IEntity entity);

        public IEntity GetParent();

    }
}