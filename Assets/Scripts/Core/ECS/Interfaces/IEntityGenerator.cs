using Assets.Scripts.Engine.ECS;

namespace Assets.Scripts.Core.ECS
{
    public interface IEntityGenerator
    {
        Entity CreateEntity(Archetype archetype);
        bool TryCreateEntity(string name, out Entity entity);
        Entity CreateEntity(string name, IComponent[] components, string[] tags = null);
    }
}