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

            foreach (IComponent component in entity.Components)
            {
                archetype.AddComponent(component);
            }

            Archetypes.Add(name, archetype);

            return archetype;
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


        public IEntity[] GetEntities()
        {
            return Matrix.Keys.Select(id => GetEntity(id)).ToArray();
        }

        public bool HasEntity(Guid id)
        {
            return Matrix.ContainsKey(id);
        }

        public bool HasTag(string tag)
        {
            //Since all the entities of an archetype have the same tags, we can just check the first one
            return FirstEntity.HasTag(tag);
        }

        public bool HasComponent<T>() where T : IComponent
        {
            //Since all the entities of an archetype have the same components, we can just check the first one
            return FirstEntity.HasComponent<T>();
        }

        public bool IsValidEntity(IEntity entity)
        {
            //If the given entity has the same components as the first entity, it is valid
            return FirstEntity.HasSameComposition(entity);
        }


    }
}