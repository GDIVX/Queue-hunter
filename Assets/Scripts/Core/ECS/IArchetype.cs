using System;
using System.Collections.Generic;

namespace Assets.Scripts.Engine.ECS
{
    public interface IArchetype
    {
        Dictionary<Guid, Dictionary<Type, IComponent>> Matrix { get; }
        string Name { get; }

        IEntity CreateEntity<T>();
        void RemoveEntity(Guid id);
        T GetComponent<T>(Guid id) where T : IComponent;

        bool HasEntity(Guid id);
        bool HasTag(string tag);
        bool IsValidEntity(IEntity entity);
        bool HasComponent<T>() where T : IComponent;
        IEntity GetEntity(Guid id);
        IEntity[] GetEntities();
    }
}