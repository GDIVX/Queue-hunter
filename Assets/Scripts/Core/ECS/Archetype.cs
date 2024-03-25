using Assets.Scripts.Core.ECS.Interfaces;
using Assets.Scripts.ECS;
using Assets.Scripts.Engine.ECS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.ECS
{
    public class Archetype
    {
        //An archetype is a matrix of components in relation to entities
        // the rows are the entities and the columns are the components
        // To find an entity, we use it guid
        // To find a component, we use it type
        public string Name { get; private set; }
        public int Count => entityIds.Count;

        private List<Guid> entityIds = new List<Guid>();
        private Dictionary<Type, IList> componentsByType = new Dictionary<Type, IList>();
        private Dictionary<Guid, IEntity> entities = new Dictionary<Guid, IEntity>();

        private List<ITag> tags = new();

        // Stores the definitive set of component types for entities in this archetype
        private HashSet<Type> componentTypesSignature = new HashSet<Type>();
        private bool signatureDefined = false;

        // A static registry for easy access to all archetypes by name
        public static Dictionary<string, Archetype> Archetypes = new Dictionary<string, Archetype>();

        public static event Action<Archetype> OnArchetypeCreated;

        private Archetype(string name)
        {
            Name = name;
        }


        /// <summary>
        /// Create an archetype from an entity
        /// </summary>
        /// <param name="name"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Archetype Create(string name, IEntity entity)
        {
            Archetype archetype = new(name);
            archetype.Name = name;


            //populate tag list
            archetype.tags = entity.Tags;

            archetype.AddEntity(entity);

            Archetypes.Add(name, archetype);

            OnArchetypeCreated?.Invoke(archetype);

            return archetype;
        }

        public static bool IsArchetypeExist(string name)
        {
            return Archetypes.ContainsKey(name);
        }

        public static Archetype GetArchetype(string name)
        {
            if (!IsArchetypeExist(name))
            {
                Debug.LogError($"{name} is not an existing archetype");
                return null;
            }

            return Archetypes[name];
        }

        public IEntity GetEntity(Guid id)
        {
            if (!entities.ContainsKey(id))
            {
                Debug.LogError($"Archetype {Name} dose not contain entity with the ID {id}.");
                return null;
            }

            return entities[id];
        }

        /// <summary>
        /// Search for an entity in all the archetypes
        /// </summary>
        /// <param name="entity">Return the entity if one was found</param>
        /// <param name="id">The Guid of the entity</param>
        /// <returns>True if an entity was found</returns>
        public static bool FindEntity(out IEntity entity, Guid id)
        {
            entity = null;
            foreach (var archetype in Archetypes.Values)
            {
                if (archetype.HasEntity(id))
                {
                    entity = archetype.GetEntity(id);
                    return true;
                }
            }

            return false;
        }


        public IEntity CreateEntity(DiContainer container, SignalBus signalBus)
        {
            //Create a list of components
            var components = new IComponent[componentTypesSignature.Count].ToList();

            //Create the entity
            IEntity entity = new Entity(components, signalBus, container);

            AddEntity(entity);
            return entity;
        }


        /// <summary>
        /// Add an entity to the archetype
        /// Warning: Do not use during gameplay, it will lead to unexpected behavior.
        /// </summary>
        /// <param name="entity"></param>
        void AddEntity(Guid entityId, IComponent[] components)
        {
            if (entityIds.Contains(entityId))
            {
                Debug.LogError("Entity already exists in this archetype.");
                return;
            }

            // Define the archetype's component signature if it hasn't been defined yet
            if (!signatureDefined)
            {
                foreach (var component in components)
                {
                    componentTypesSignature.Add(component.GetType());
                }

                signatureDefined = true;
            }
            else if (!ValidateComponentsMatchArchetype(components))
            {
                Debug.LogError($"Entity's components do not match the archetype's composition for archetype {Name}.");
                return;
            }

            int index = Count;
            entityIds.Add(entityId);

            foreach (var component in components)
            {
                AddComponent(component.GetType(), component, index);
            }
        }

        private void AddComponent(Type type, IComponent component, int index)
        {
            IList componentList;
            if (!componentsByType.TryGetValue(type, out componentList))
            {
                // Create a new list for this component type if it doesn't exist
                Type listType = typeof(List<>).MakeGenericType(type);
                componentList = (IList)Activator.CreateInstance(listType);
                componentsByType[type] = componentList;
            }

            // Ensure the list is extended to accommodate the new component at the correct index
            while (componentList.Count <= index)
            {
                componentList.Add(null);
            }

            componentList[index] = component;
        }

        public void AddEntity(IEntity entity)
        {
            AddEntity(entity.ID, entity.GetComponents());
            entities.Add(entity.ID, entity);
            entity.Initialize(this);
        }


        #region Components

        /// <summary>
        /// Get all components of a specific type associated with the archetype
        /// </summary>
        /// <typeparam name="T">The type of component to queary</typeparam>
        /// <returns>An array of IComponent of type <typeparam name="T"> </returns>
        public T[] GetComponents<T>() where T : IComponent
        {
            if (componentsByType.TryGetValue(typeof(T), out IList componentList))
            {
                return componentList.Cast<T>().ToArray();
            }

            return Array.Empty<T>();
        }

        /// <summary>
        /// Check if the entities of this archetype have a specific tag
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public bool HasTag(string tagName)
        {
            foreach (var tag in tags)
            {
                if (tag.Name == tagName)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if the entities of this archetype have a specific component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool HasComponent<T>() where T : IComponent
        {
            return componentsByType.ContainsKey(typeof(T));
        }

        public bool HasComponents<T1, T2>() where T1 : IComponent where T2 : IComponent
        {
            return HasComponent<T1>() && HasComponent<T2>();
        }

        public bool HasComponents<T1, T2, T3>() where T1 : IComponent where T2 : IComponent where T3 : IComponent
        {
            return HasComponent<T1>() && HasComponents<T2, T3>();
        }

        public bool HasComponents<T1, T2, T3, T4>()
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
        {
            return HasComponent<T1>() && HasComponents<T2, T3, T4>();
        }

        public bool HasComponents<T1, T2, T3, T4, T5>()
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
        {
            return HasComponent<T1>() && HasComponents<T2, T3, T4, T5>();
        }

        private bool ValidateComponentsMatchArchetype(IComponent[] components)
        {
            var componentTypes = new HashSet<Type>(components.Select(c => c.GetType()));

            // Check if the entity's components exactly match the archetype's signature
            return componentTypes.SetEquals(componentTypesSignature);
        }

        public bool IsValidComposition(IComponent[] components, string[] tags)
        {
            string[] tagNames = this.tags.Select(t => t.Name).ToArray();
            return tags.Equals(tags) && ValidateComponentsMatchArchetype(components);
        }

        #endregion


        #region Entities

        /// <summary>
        /// Get all the entities of this archetype
        /// </summary>
        /// <returns>Array of <see cref="IEntity"</see>/> with the same composition </returns>
        public IEntity[] GetEntities()
        {
            return entities.Select(x => x.Value).ToArray();
        }

        public bool HasEntity(Guid id)
        {
            return entityIds.Contains(id);
        }

        #endregion
    }
}