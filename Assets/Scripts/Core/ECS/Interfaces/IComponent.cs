using Assets.Scripts.Engine.ECS;

namespace Assets.Scripts.Core.ECS.Interfaces
{
    public interface IComponent : IActivable
    {
        public IComponent Clone();

        public void SetParent(IEntity entity);

        public IEntity GetParent();

    }
}