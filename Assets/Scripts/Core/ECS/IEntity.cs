using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Engine.ECS
{
    public interface IEntity
    {
        List<IComponent> Components { get; }
        Guid ID { get; }
        List<string> Tags { get; }

        IComponent AddComponent(IComponent component);
        void AddTag(string tag);
        Entity Clone();
        void Destroy();
        bool Equals(object obj);
        T GetComponent<T>() where T : IComponent;
        T[] GetComponents<T>() where T : IComponent;
        GameObject GetGameObject();
        int GetHashCode();
        bool HasComponent<T>() where T : IComponent;
        bool HasTag(string tag);
        void RemoveComponent(IComponent component);
        void RemoveTag(string tag);
        void SetRootGameObject(GameObject gameObject);
        string ToString();
        bool TryGetComponent<T>(out T component) where T : IComponent;
    }
}