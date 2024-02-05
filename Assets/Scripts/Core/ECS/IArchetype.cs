using System;
using System.Collections.Generic;

namespace Assets.Scripts.Engine.ECS
{
    public interface IArchetype
    {
        Dictionary<Guid, Dictionary<Type, IComponent>> Matrix { get; }
        string Name { get; }

        IEntity CreateEntity<T>();
        T GetComponent<T>(Guid id) where T : IComponent;
        IEntity GetEntity(Guid id);
    }
}