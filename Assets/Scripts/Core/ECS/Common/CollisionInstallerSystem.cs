using Assets.Scripts.Engine.ECS;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.ECS.Common
{
    public class CollisionInstallerSystem : GameSystem
    {
        public CollisionInstallerSystem(SignalBus signalBus) : base(signalBus)
        {
        }

        protected override bool ShouldProcessArchetype(Archetype archetype)
        {
            return archetype.HasComponents<CollisionComponent, GameObjectComponent>();
        }


        public override void OnLateEntityCreated(IEntity entity)
        {
            base.OnLateEntityCreated(entity);

            var collisionCom = entity.GetComponent<CollisionComponent>();
            var gameObjectCom = entity.GetComponent<GameObjectComponent>();

            collisionCom.Install(gameObjectCom.GameObject);
        }

    }
}