using Assets.Scripts.Core.ECS.Interfaces;
using Zenject;

namespace Assets.Scripts.Core.ECS
{
    public interface IEntityFactory : IFactory<Archetype, Entity>, IFactory<string, Entity> , IFactory<string , IComponent[], string[], Entity>
    {

    }
}