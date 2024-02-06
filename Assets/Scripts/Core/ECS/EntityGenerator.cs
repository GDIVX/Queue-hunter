using Assets.Scripts.ECS;
using Assets.Scripts.Engine.ECS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Core.ECS
{
    public class EntityGenerator : IEntityGenerator
    {

        private Entity InitializeEntity(string name, IComponent[] components, string[] tags = null)
        {
            Entity entity = new(components.ToList());

            if (tags != null)
            {
                foreach (string tagName in tags)
                {
                    Tag tag = new(tagName);
                    entity.AddTag(tag);
                }
            }
            Archetype.Create(name, entity);
            return entity;
        }


        /// <summary>
        /// Create an entity from an archetype
        /// </summary>
        /// <param name="archetype"></param>
        /// <returns>A new entity that match the archetype</returns>
        public Entity CreateEntity(Archetype archetype)
        {
            return archetype.CreateEntity<Entity>();
        }

        /// <summary>
        /// Create an entity from a list of components. Initialize a new archetype if it doesn't exist
        /// </summary>
        /// <param name="name">The name of the archetype</param>
        /// <param name="components">A list of components to add to the entity</param>
        /// <param name="tags">A list of tags to add to the entity</param>
        /// <returns>A new entity</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Entity CreateEntity(string name, IComponent[] components, string[] tags = null)
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

            return CreateEntity(archetype);
        }

        public bool TryCreateEntity(string name, out Entity entity)
        {
            entity = null;

            if (!Archetype.IsArchetypeExist(name)) return false;

            Archetype archetype = Archetype.GetArchetype(name);
            entity = archetype.CreateEntity<Entity>();

            return true;
        }
    }
}