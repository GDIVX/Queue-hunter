using Assets.Scripts.Engine.ECS;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.ECS.Common
{
    public class EntityInspector : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] int entityID;
        [SerializeReference] List<DataComponent> components;

        public Guid EntityGuid { get; private set; }

        public void Init(IEntity entity)
        {
            entityID = entity.ID.GetHashCode();

            EntityGuid = entity.ID;

            components = new();

            foreach (var component in entity.GetComponents())
            {
                components.Add(component as DataComponent);
            }
        }
    }
}