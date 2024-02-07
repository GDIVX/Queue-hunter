using Assets.Scripts.Engine.ECS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.ECS.Common
{
    public class EntityInspector : MonoBehaviour
    {
        [SerializeField] int entityID;
        [SerializeField] List<DataComponent> components;

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