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

        IArchetype Archetype { get; set; }

        IComponent AddComponent(IComponent component);
        IEntity Clone();
        void Destroy();
        bool Equals(object obj);
        T GetComponent<T>() where T : IComponent;
        int ComponentsCount { get; }
        IComponent[] GetComponents();
        T[] GetComponents<T>() where T : IComponent;
        GameObject GetGameObject();
        int GetHashCode();
        bool HasComponent<T>() where T : IComponent;
        void AddTag(ITag tag);
        bool HasTag(string tag);
        void RemoveTag(ITag tag);
        void RemoveComponent<T>() where T : IComponent;
        void SetRootGameObject(GameObject gameObject);
        string ToString();
        bool TryGetComponent<T>(out T component) where T : IComponent;
        bool HasSameComposition(IEntity entity);
        bool HasSameComposition(IComponent[] components, string[] tags);
        bool HasComponent(IComponent component);
    }
}