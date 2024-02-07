using Assets.Scripts.Core.ECS.Common;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Engine.ECS.Common
{
    public class RotationSystem : GameSystem
    {
        public RotationSystem(SignalBus signalBus) : base(signalBus)
        {
        }

        protected override bool ShouldProcessEntity(IEntity entity)
        {
            //we should process any entity with a position component and a model component
            return entity.HasComponent<RotationComponent, GameObjectComponent>();
        }

        protected override void OnUpdate(IEntity entity)
        {
            //get the position component
            var rotationComponent = entity.GetComponent<RotationComponent>();
            var gameObjectComponent = entity.GetComponent<GameObjectComponent>();

            //update the entity root game object
            gameObjectComponent.GameObject.transform.rotation = Quaternion.Euler(rotationComponent.Rotation);

        }
    }
}