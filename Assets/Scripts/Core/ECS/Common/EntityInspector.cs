using Assets.Scripts.Engine.ECS;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.ECS.Common
{
    public class EntityInspector : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] string entityID;
        [SerializeReference] List<DataComponent> components;

        public IEntity Entity { get; private set; }

        public Guid EntityGuid { get; private set; }

        public void Init(IEntity entity)
        {
            entityID = entity.ID.ToString();

            EntityGuid = entity.ID;

            components = new();

            Entity = entity;

            foreach (var component in entity.GetComponents())
            {
                components.Add(component as DataComponent);
            }
        }
    }
}