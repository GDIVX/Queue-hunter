using Zenject;
using UnityEngine;
using Assets.Scripts.Core.ECS.Common;

namespace Assets.Scripts.Engine.ECS.Common
{
    public class PositionSystem : GameSystem
    {
        public PositionSystem(SignalBus signalBus) : base(signalBus)
        {
        }

        protected override bool ShouldProcessEntity(IEntity entity)
        {
            //we should process any entity with a position component and a model component
            return entity.HasComponent<PositionComponent, GameObjectComponent>();
        }



        protected override void OnUpdate(IEntity entity)
        {
            //get the position component
            var positionComponent = entity.GetComponent<PositionComponent>();
            var gameObjectComponent = entity.GetComponent<GameObjectComponent>();


            //update the entity root game object
            gameObjectComponent.GameObject.transform.position = positionComponent.Position;

        }
    }
}