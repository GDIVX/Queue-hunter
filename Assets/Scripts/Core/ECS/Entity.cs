using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using Assets.Scripts.ECS;
using Assets.Scripts.Engine.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using System.Reflection;

namespace Assets.Scripts.Core.ECS
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
        public Archetype Archetype { get; private set; }

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

        public int ComponentsCount => Components.Count;

        private DiContainer _container;


        [Inject]
        private SignalBus _signalBus;

        private bool isActive;
        #endregion

        #region Lifecycle
        public Entity(List<IComponent> components, SignalBus signalBus , DiContainer container)
        {
            _container = container;
            _signalBus = signalBus;
            ID = Guid.NewGuid();
            foreach (var component in components)
            {
                Components[component.GetType()] = component;
                component.SetParent(this);
            }
        }



        public void Initialize(Archetype archetype)
        {
            if (archetype is null)
            {
                throw new ArgumentNullException(nameof(archetype));
            }
            Archetype = archetype;
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
            List<IComponent> components = new List<IComponent>();

            foreach (var pair in Components)
            {
                Type componentType = pair.Value.GetType();

                // Get the generic method definition from the type
                var methodInfo = componentType.GetMethod("Instantiate", BindingFlags.Public | BindingFlags.Instance);

                if (methodInfo == null)
                {
                    Debug.LogError($"Instantiate method not found on type {componentType.Name}");
                    continue;
                }

                // Make the generic method specific by providing the component's type
                var genericMethodInfo = methodInfo.MakeGenericMethod(new Type[] { componentType });

                // Invoke the method with no parameters as it's a parameterless generic method
                IComponent clonedComponent = (IComponent)genericMethodInfo.Invoke(pair.Value, null);

                if (clonedComponent != null)
                {
                    components.Add(clonedComponent);
                }
                else
                {
                    Debug.LogError($"Failed to clone component of type {componentType.Name}");
                }
            }

            if (_container == null)
            {
                _container = new DiContainer();
            }

            // Assuming _container and Archetype are accessible and correctly set up for this context
            Entity clone = _container.Instantiate<Entity>(new object[] { components, _container.Resolve<SignalBus>() });

            Archetype.AddEntity(clone);

            return clone;
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
        public bool HasComponent<T1, T2>()
           where T1 : IComponent
           where T2 : IComponent
        {
            return HasComponent<T1>() && HasComponent<T2>();
        }

        public bool HasComponent<T1, T2, T3>()
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
        {
            return HasComponent<T1, T2>() && HasComponent<T3>();
        }

        public bool HasComponent<T1, T2, T3, T4>()
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
        {
            return HasComponent<T1, T2, T3>() && HasComponent<T4>();
        }

        public bool HasComponent<T1, T2, T3, T4, T5>()
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
        {
            return HasComponent<T1, T2, T3, T4>() && HasComponent<T5>();
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

        public IComponent[] GetComponents()
        {
            return Components.Values.ToArray();
        }

        public void RemoveComponent(DataComponent dataComponent)
        {
            //find the component
            var component = Components.Values.FirstOrDefault(c => c.GetType() == dataComponent.GetType());
            if (component != null)
            {
                Components.Remove(component.GetType());
                component.SetParent(null);
                OnModified();
            }
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

        #endregion
    }

}