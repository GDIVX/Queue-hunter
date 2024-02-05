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
        public Archetype CreateArchetype(string name, IComponent[] components, string[] tags = null)
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
            return Archetype.Create(name, entity);
        }

        public bool IsArchetypeExist(string name)
        {
            return Archetype.Archetypes.ContainsKey(name);
        }

        public Archetype FindArchetype(string name)
        {
            if (!IsArchetypeExist(name))
            {
                Debug.LogError($"{name} is not an existing archetype");
                return null;
            }

            return Archetype.Archetypes[name];
        }

        public Entity CreateEntity(Archetype archetype)
        {
            return archetype.CreateEntity<Entity>();
        }
    }
}