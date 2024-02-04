using Assets.Scripts.Engine.ECS.Common;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;

namespace Assets.Scripts.Engine.ECS
{


    /// <summary>
    /// An entity is a container for components
    /// It represent a game object in the ECS system
    /// </summary>
    [Serializable]
    public class Entity : ICloneable, IEntity
    {
        /// <summary>
        /// Unique ID for this entity
        /// </summary>
        [ShowInInspector, ReadOnly]
        public Guid ID { get; private set; }

        [ShowInInspector, ReadOnly]
        public List<IComponent> Components { get; private set; } = new();

        [ShowInInspector, ReadOnly]
        public List<string> Tags { get; private set; } = new();

        [ShowInInspector, ReadOnly]
        GameObject _GameObject;

        [Inject]
        private SignalBus _signalBus;

        public Entity(List<IComponent> components)
        {
            this.ID = Guid.NewGuid();
            Components = components;
            _signalBus.Fire(new EntityCreatedSignal(this));
        }


        /// <summary>
        /// Clones the entity, creating a new entity with the same components.
        /// </summary>
        /// <returns></returns>
        public Entity Clone()
        {
            List<IComponent> components =
            (from component in Components
             let newComponent = component.Clone()
             select newComponent).ToList();

            Entity clone = new(components);

            return clone;
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
            OnModified();
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
            OnModified();
        }

        public void AddTag(string tag)
        {
            Tags.Add(tag);
            OnModified();
        }

        public void RemoveTag(string tag)
        {
            Tags.Remove(tag);
            OnModified();
        }
        private void OnModified()
        {
            _signalBus.Fire(new EntityModifiedSignal(this));
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

        public GameObject GetGameObject()
        {
            return _GameObject;
        }

        public void SetRootGameObject(GameObject gameObject)
        {
            if (_GameObject != null)
            {
                //we made a game object by accident.
                Debug.LogWarning($"{gameObject.name} game object was created but not needed. It is being deactivated.");
                gameObject.SetActive(false);
            }
            _GameObject = gameObject;
        }

        public bool HasTag(string tag)
        {
            return Tags.Contains(tag);
        }

        public void Destroy()
        {
            _signalBus.Fire(new EntityDestroyedSignal(this));
            if (_GameObject != null)
            {
                UnityEngine.Object.Destroy(_GameObject);
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
            return HashCode.Combine(ID, Components, Tags);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }

}