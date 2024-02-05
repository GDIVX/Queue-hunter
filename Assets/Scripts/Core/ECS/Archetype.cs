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

        public IEntity CreateEntity<T>()
        {
            IEntity clone = FirstEntity.Clone();
            AddEntity(clone);
            return clone;
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

        private void AddEntity(IEntity entity)
        {
            Matrix.Add(entity.ID, new());
        }
    }
}