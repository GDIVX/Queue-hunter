using Assets.Scripts.Core.ECS.Interfaces;
using Assets.Scripts.Engine.ECS;
using System;
using UnityEngine;

namespace Assets.Scripts.Core.ECS.Common
{
    [System.Serializable]
    public class CollisionComponent : UnityComponentToECS<BoxCollider>
    {
        public Collider Collider => UnityComponent;

        public event Action<IEntity, IEntity> OnTriggerEnter;
        public event Action<IEntity, IEntity> OnTriggerStay;
        public event Action<IEntity, IEntity> OnTriggerExit;

        public override Component Install(GameObject gameObject)
        {
            base.Install(gameObject);

            // Register Unity collision event handlers
            var triggerEventBridge = gameObject.AddComponent<CollisionEventBridge>();
            triggerEventBridge.Initialize(this);

            return UnityComponent;
        }

        // This method is called by the TriggerEventBridge when a trigger event occurs in Unity.
        internal void OnUnityTriggerEnter(Collider other)
        {
            var otherEntityID = other.GetComponent<EntityInspector>().EntityGuid;
            if (Archetype.FindEntity(out var otherEntity, otherEntityID))
            {
                OnTriggerEnter?.Invoke(this.Entity, otherEntity);
            }
        }

        internal void OnUnityTriggerStay(Collider other)
        {
            var otherEntityID = other.GetComponent<EntityInspector>().EntityGuid;
            if (Archetype.FindEntity(out var otherEntity, otherEntityID))
            {
                OnTriggerStay?.Invoke(this.Entity, otherEntity);
            }
        }

        internal void OnUnityTriggerExit(Collider other)
        {
            var otherEntityID = other.GetComponent<EntityInspector>().EntityGuid;
            if (Archetype.FindEntity(out var otherEntity, otherEntityID))
            {
                OnTriggerExit?.Invoke(this.Entity, otherEntity);
            }
        }

        public override IComponent Instantiate()
        {
            CollisionComponent component = new CollisionComponent();
            return component;
        }
    }

}
