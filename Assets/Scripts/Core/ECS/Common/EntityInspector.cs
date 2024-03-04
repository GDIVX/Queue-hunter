using Assets.Scripts.Engine.ECS;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.ECS.Common
{
    public class EntityInspector : MonoBehaviour
    {
        [ShowInInspector , ReadOnly] int entityID;
        [SerializeReference] List<DataComponent> components;

        public void Init(IEntity entity)
        {
            entityID = entity.ID.GetHashCode();

            components = new();

            foreach (var component in entity.GetComponents())
            {
                components.Add(component as DataComponent);
            }
        }
    }
}