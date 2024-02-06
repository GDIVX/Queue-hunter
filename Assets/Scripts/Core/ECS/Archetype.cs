using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Engine.ECS
{
    public class Archetype : IArchetype
    {
        //An archetype is a matrix of components in relation to entities
        // the rows are the entities and the columns are the components
        // To find an entity, we use it guid
        // To find a component, we use it type
        public Dictionary<Guid, Dictionary<Type, IComponent>> Matrix { get; private set; } = new();

        public IEntity FirstEntity
        {
            get
            {
                return GetEntity(Matrix.Keys.First());
            }
        }

        //We also want to give a name to the archetype to be able to find it easily
        public string Name { get; private set; }

        //a static list of all the archetypes
        public static Dictionary<string, Archetype> Archetypes { get; private set; } = new();

        private Archetype()
        {
        }


        /// <summary>
        /// Create an archetype from a list of components
        /// </summary>
        /// <param name="name"></param>
        /// <param name="components"></param>
        /// <returns></returns>
        public static Archetype Create(string name, List<IComponent> components)
        {
            Archetype archetype = new();
            archetype.Name = name;

            foreach (IComponent component in components)
            {
                archetype.AddComponent(component);
            }

            Archetypes.Add(name, archetype);

            return archetype;
        }


        /// <summary>
        /// Create an archetype from an entity
        /// </summary>
        /// <param name="name"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Archetype Create(string name, IEntity entity)
        {
            Archetype archetype = new();
            archetype.Name = name;

            foreach (IComponent component in entity.GetComponents())
            {
                archetype.AddComponent(component);
            }

            Archetypes.Add(name, archetype);

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
            var component = Matrix[id].Keys.First() as IComponent;

            if (component == null)
            {
                Debug.LogError("No component found for this entity");
                return null;
            }

            return component.GetParent();
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

        /// <summary>
        /// Search for all the entities of a specific archetype
        /// </summary>
        /// <param name="name">The name of the archetype</param>
        /// <param name="entities">A list of the entities found</param>
        /// <returns>True if found entities</returns>
        public static bool FindEntitiesOfArchetype(string name, out IEntity[] entities)
        {
            entities = new IEntity[0];
            if (!IsArchetypeExist(name))
            {
                return false;
            }

            Archetype archetype = GetArchetype(name);
            entities = archetype.GetEntities();

            return true;
        }

        public T GetComponent<T>(Guid id) where T : IComponent
        {
            if (!Matrix[id].ContainsKey(typeof(T)))
            {
                Debug.LogError("No component found for this entity");
                return default;
            }

            return (T)Matrix[id][typeof(T)];

        }

        public T CreateEntity<T>() where T : IEntity
        {
            T clone = (T)FirstEntity.Clone();
            AddEntity(clone);
            return clone;
        }

        /// <summary>
        /// Add an entity to the archetype
        /// Warning: Do not use during gameplay, it will lead to unexpected behavior.
        /// </summary>
        /// <param name="entity"></param>
        public void AddEntity(IEntity entity)
        {
            if (Matrix.ContainsKey(entity.ID))
            {
                return;
            }
            if (IsValidEntity(entity) == false)
            {
                Debug.LogError("The entity is not valid for this archetype");
                return;
            }

            Matrix.Add(entity.ID, new());
            entity.Archetype = this;
        }

        /// <summary>
        /// Remove an entity from the archetype. 
        /// Warning: Do not use during gameplay, it will lead to unexpected behavior.
        /// </summary>
        /// <param name="id"></param>
        public void RemoveEntity(Guid id)
        {
            //do we have this entity?
            if (!HasEntity(id)) return;

            //If this is the first entity, only deactivate it.
            IEntity entity = GetEntity(id);
            if (FirstEntity == entity)
            {
                entity.IsActive = false;
                return;
            }

            //remove the entity
            Matrix.Remove(id);
        }

        #region Components

        /// <summary>
        /// Check if the entities of this archetype have a specific tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool HasTag(string tag)
        {
            //Since all the entities of an archetype have the same tags, we can just check the first one
            return FirstEntity.HasTag(tag);
        }

        /// <summary>
        /// Check if the entities of this archetype have a specific component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool HasComponent<T>() where T : IComponent
        {
            //Since all the entities of an archetype have the same components, we can just check the first one
            return FirstEntity.HasComponent<T>();
        }
        private void AddComponent(IComponent component)
        {
            //We need to find the entity of the component
            IEntity entity = component.GetParent();

            //If the entity is not in the archetype, we add it
            if (!Matrix.ContainsKey(entity.ID))
            {
                AddEntity(entity);
            }

            //We add the component to the entity
            Matrix[entity.ID].Add(component.GetType(), component);
        }
        public bool IsValidComposition(IComponent[] components, string[] tags)
        {
            return FirstEntity.HasSameComposition(components, tags);
        }
        #endregion


        #region Entities
        /// <summary>
        /// Get all the entities of this archetype
        /// </summary>
        /// <returns>Array of <see cref="IEntity"</see>/> with the same composition </returns>
        public IEntity[] GetEntities()
        {
            return Matrix.Keys.Select(id => GetEntity(id)).ToArray();
        }

        public bool HasEntity(Guid id)
        {
            return Matrix.ContainsKey(id);
        }
        public bool IsValidEntity(IEntity entity)
        {
            //If the given entity has the same components as the first entity, it is valid
            return FirstEntity.HasSameComposition(entity);
        }

        #endregion




    }
}