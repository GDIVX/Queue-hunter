using Assets.Scripts.Engine.ECS.Common;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Engine.ECS
{
    /// <summary>
    /// An entity is a container for components
    /// It represent a game object in the ECS system
    /// </summary>
    [Serializable]
    public class Entity
    {
        /// <summary>
        /// Unique ID for this entity
        /// </summary>
        [ShowInInspector, ReadOnly]
        public int ID { get; private set; }

        [ShowInInspector, ReadOnly]
        public List<IComponent> Components { get; private set; } = new();

        [ShowInInspector, ReadOnly]
        public List<string> Tags { get; private set; } = new();

        [ShowInInspector, ReadOnly]
        GameObject _rootGameObject;

        public event Action<Entity> OnModified;

        public Action<Entity> OnDestroyed;

        internal Entity()
        {
            this.ID = GetHashCode();
        }

        /// <summary>
        /// Create a new blank entity
        /// </summary>
        /// <returns></returns>
        public static Entity Create()
        {
            return ECSManager.Instance.CreateEntity();
        }

        /// <summary>
        /// Clones the entity, creating a new entity with the same components.
        /// </summary>
        /// <returns></returns>
        public Entity Clone()
        {
            //Create a new entity
            var entity = Create();

            //clone each component and attach to the new entity
            foreach (var component in Components)
            {
                IComponent newComponent = component.Clone();
                entity.AddComponent(newComponent);
            }

            ECSManager.Instance.OnEntityInitialized?.Invoke(entity);

            return entity;
        }

        /// <summary>
        /// Adds a component to the entity.
        /// </summary>
        /// <param name="component">The component to add</param>
        /// <returns>The added component.</returns>
        public IComponent AddComponent(IComponent component)
        {
            Components.Add(component);
            component.SetParent(this);
            OnModified?.Invoke(this);
            return component;
        }

        /// <summary>
        /// Remove a component for the entity.
        /// </summary>
        /// <param name="component">The component to remove</param>
        public void RemoveComponent(IComponent component)
        {
            Components.Remove(component);
            component.SetParent(null);
            OnModified?.Invoke(this);
        }

        public void AddTag(string tag)
        {
            Tags.Add(tag);
            OnModified?.Invoke(this);
        }

        public void RemoveTag(string tag)
        {
            Tags.Remove(tag);
            OnModified?.Invoke(this);
        }

        // <summary>
        /// Gets a component of the specified type attached to the entity.
        /// </summary>
        /// <typeparam name="T">The type of component to retrieve.</typeparam>
        /// <returns>The component of the specified type if found; otherwise, the default value of the type.</returns>
        public T GetComponent<T>() where T : IComponent
        {
            foreach (var component in Components)
            {
                if (component is T t)
                {
                    return t;
                }
            }
            return default;
        }

        public bool HasComponent<T>() where T : IComponent
        {
            foreach (var component in Components)
            {
                if (component is T)
                {
                    return true;
                }
            }
            return false;
        }

        public T GetOrAddComponent<T>() where T : IComponent, new()
        {
            var component = GetComponent<T>();
            if (component == null)
            {
                component = new T();
                AddComponent(component);
            }
            return component;
        }

        public bool TryGetComponent<T>(out T component) where T : IComponent
        {
            if (HasComponent<T>())
            {
                component = GetComponent<T>();
                return true;
            }
            component = default;
            return false;
        }

        public T[] GetComponents<T>() where T : IComponent
        {
            var components = new List<T>();
            foreach (var component in Components)
            {
                if (component is T t)
                {
                    components.Add(t);
                }
            }
            return components.ToArray();
        }

        public GameObject GetRootGameObject()
        {
            return _rootGameObject;
        }

        public void SetRootGameObject(GameObject gameObject)
        {
            if (_rootGameObject != null)
            {
                //we made a game object by accident.
                Debug.LogWarning($"{gameObject.name} game object was created but not needed. It is being deactivated.");
                gameObject.SetActive(false);
            }
            _rootGameObject = gameObject;
        }

        public bool HasTag(string tag)
        {
            return Tags.Contains(tag);
        }

        public void Destroy()
        {
            ECSManager.Instance.DestroyEntity(this);
            if (_rootGameObject != null)
            {
                GameObject.Destroy(_rootGameObject);
            }
        }


        public override string ToString()
        {
            return $"Entity {ID}";
        }

        public override bool Equals(object obj)
        {
            return obj is Entity entity &&
                   ID == entity.ID;
        }

        public override int GetHashCode()
        {
            return ECSManager.Instance.GenerateUniqueId();
        }

    }

}