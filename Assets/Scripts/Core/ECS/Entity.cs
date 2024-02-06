using Assets.Scripts.Engine.ECS.Common;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using Assets.Scripts.ECS;

namespace Assets.Scripts.Engine.ECS
{


    /// <summary>
    /// An entity is a container for components
    /// It represent a game object in the ECS system
    /// </summary>
    [Serializable]
    public sealed class Entity : ICloneable, IEntity
    {
        #region Properties
        /// <summary>
        /// Unique ID for this entity
        /// </summary>
        public Guid ID { get; private set; }
        public IArchetype Archetype { get; set; }

        public Dictionary<Type, IComponent> Components { get; private set; } = new();

        public List<ITag> Tags { get; private set; } = new();
        public bool IsActive
        {
            get => isActive;
            set
            {
                //iterate throughout the components and tags and toggle them
                foreach (var component in Components)
                {
                    component.Value.IsActive = value;
                }

                foreach (var tag in Tags)
                {
                    tag.IsActive = value;
                }
                isActive = value;
            }
        }

        public int ComponentsCount => throw new NotImplementedException();

        GameObject _GameObject;

        [Inject]
        private SignalBus _signalBus;

        private bool isActive;
        #endregion

        #region Lifecycle
        public Entity(List<IComponent> components)
        {
            this.ID = Guid.NewGuid();
            Components = components.ToDictionary(c => c.GetType(), c => c);
            _signalBus.Fire(new EntityCreatedSignal(this));
        }
        private void OnModified()
        {
            _signalBus.Fire(new EntityModifiedSignal(this));
        }


        /// <summary>
        /// Clones the entity, creating a new entity with the same components.
        /// </summary>
        /// <returns></returns>
        public IEntity Clone()
        {
            List<IComponent> components = new();
            components.AddRange(from component in Components
                                select component.Value.Clone());
            Entity clone = new(components);

            Archetype.AddEntity(clone);

            return clone as IEntity;
        }
        public void Destroy()
        {
            _signalBus.Fire(new EntityDestroyedSignal(this));

            Archetype.RemoveEntity(ID);
            Archetype = null;

            if (_GameObject != null)
            {
                UnityEngine.Object.Destroy(_GameObject);
            }
        }
        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region Components
        /// <summary>
        /// Adds a component to the entity.
        /// </summary>
        /// <param name="component">The component to add</param>
        /// <returns>The added component.</returns>
        public IComponent AddComponent(IComponent component)
        {
            Components[component.GetType()] = component;
            component.SetParent(this);
            OnModified();
            return component;
        }

        /// <summary>
        /// Remove a component for the entity.
        /// </summary>
        /// <param name="component">The component to remove</param>
        public void RemoveComponent<T>() where T : IComponent
        {
            var component = Components[typeof(T)];
            Components.Remove(typeof(T));
            component.SetParent(null);
            OnModified();
        }
        public T GetComponent<T>() where T : IComponent
        {
            return (T)Components[typeof(T)];
        }

        public bool HasComponent<T>() where T : IComponent
        {
            return Components.ContainsKey(typeof(T));
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
        #endregion

        #region Tags
        public void AddTag(ITag tag)
        {
            Tags.Add(tag);
            OnModified();
        }

        public void RemoveTag(ITag tag)
        {
            Tags.Remove(tag);
            OnModified();
        }
        public bool HasTag(string tagName)
        {
            foreach (var tag in Tags)
            {
                if (tag.Name == tagName)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region GameObject
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
        #endregion


        #region Management


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

        public bool HasSameComposition(IComponent[] components, string[] tags)
        {
            if (Components.Count != components.Length)
            {
                return false;
            }

            if (Tags.Count != tags.Length)
            {
                return false;
            }

            foreach (IComponent component in components)
            {
                if (!HasComponent(component))
                {
                    return false;
                }
            }

            for (int i = 0; i < Tags.Count; i++)
            {
                if (Tags[i].Name != tags[i])
                {
                    return false;
                }
            }

            return true;
        }

        public bool HasComponent(IComponent component)
        {
            foreach (var pair in Components)
            {
                if (pair.Value == component)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasSameComposition(IEntity entity)
        {
            //We can do a simple 1 to 1 comparison of components and tags
            if (Components.Count != entity.ComponentsCount)
            {
                return false;
            }

            if (Tags.Count != entity.Tags.Count)
            {
                return false;
            }

            var arr = entity.GetComponents();

            foreach (var component in arr)
            {
                if (!HasComponent(component)) return false;
            }

            for (int i = 0; i < Components.Count; i++)
            {
                if (Tags[i].Name != entity.Tags[i].Name)
                {
                    return false;
                }
            }

            return true;
        }

        public IComponent[] GetComponents()
        {
            throw new NotImplementedException();
        }


        #endregion




    }

}