using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using Assets.Scripts.ECS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Engine.ECS
{
    public interface IEntity : IActivable
    {
        Guid ID { get; }
        List<ITag> Tags { get; }

        Archetype Archetype { get; }

        IComponent AddComponent(IComponent component);
        IEntity Clone();
        bool Equals(object obj);
        int ComponentsCount { get; }
        int GetHashCode();
        void AddTag(ITag tag);
        string ToString();
        void Initialize(Archetype archetype);
        bool HasComponent<T>();
        T GetComponent<T>();
        IComponent[] GetComponents();
    }
}