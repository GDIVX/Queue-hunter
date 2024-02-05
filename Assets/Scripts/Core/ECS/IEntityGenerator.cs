using Assets.Scripts.Engine.ECS;

namespace Assets.Scripts.Core.ECS
{
    public interface IEntityGenerator
    {
        Archetype CreateArchetype(string name, IComponent[] components, string[] tags = null);
        Entity CreateEntity(Archetype archetype);
        Archetype FindArchetype(string name);
        bool IsArchetypeExist(string name);
    }
}