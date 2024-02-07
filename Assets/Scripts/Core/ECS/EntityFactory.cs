using Assets.Scripts.Core.ECS.Interfaces;
using Assets.Scripts.ECS;
using Assets.Scripts.Engine.ECS;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using Zenject;
using IComponent = Assets.Scripts.Core.ECS.Interfaces.IComponent;

namespace Assets.Scripts.Core.ECS
{
    public class EntityFactory : IEntityFactory
    {
        private readonly DiContainer _container;

        public EntityFactory(DiContainer container)
        {
            _container = container;
        }

        private Entity InitializeEntity(string name, IComponent[] components, string[] tags = null)
        {
            Entity entity = _container.Instantiate<Entity>(new object[] { components.ToList(), _container.Resolve<SignalBus>() });


            if (tags != null)
            {
                foreach (string tagName in tags)
                {
                    Tag tag = new(tagName);
                    entity.AddTag(tag);
                }
            }
            var archetype = Archetype.Create(name, entity);
            entity.Initialize(archetype);
            return entity;
        }


        /// <summary>
        /// Create an entity from an archetype
        /// </summary>
        /// <param name="archetype"></param>
        /// <returns>A new entity that match the archetype</returns>
        public Entity Create(Archetype archetype)
        {
            var entity = archetype.CreateEntity<Entity>();
            entity.Initialize(archetype);
            return entity;
        }

        /// <summary>
        /// Create an entity from a list of components. Initialize a new archetype if it doesn't exist
        /// </summary>
        /// <param name="name">The name of the archetype</param>
        /// <param name="components">A list of components to add to the entity</param>
        /// <param name="tags">A list of tags to add to the entity</param>
        /// <returns>A new entity</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Entity Create(string name, IComponent[] components, string[] tags = null)
        {
            //Dose the archetype exist?
            if (!Archetype.IsArchetypeExist(name))
            {
                //initialize a new archetype
                return InitializeEntity(name, components, tags);
            }

            //Get the archetype
            Archetype archetype = Archetype.GetArchetype(name);

            //Is this a valid composition?

            if (!archetype.IsValidComposition(components, tags))
            {
                //Create a new archetype with a new name
                name = name + "_New";
                return InitializeEntity(name, components, tags);
            }

            return Create(archetype);
        }

        public bool TryCreateEntity(string name, out Entity entity)
        {
            entity = null;

            if (!Archetype.IsArchetypeExist(name)) return false;

            Archetype archetype = Archetype.GetArchetype(name);
            entity = archetype.CreateEntity<Entity>();
            entity.Initialize(archetype);

            return true;
        }

        public Entity Create(string name)
        {
            if (TryCreateEntity(name, out Entity entity))
            {
                return entity;
            }
            Debug.LogError($"Failed to create entity with name {name}");
            return null;
        }
    }
}