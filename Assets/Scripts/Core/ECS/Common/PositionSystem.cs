using Zenject;
using UnityEngine;
using Assets.Scripts.Core.ECS.Common;
using Assets.Scripts.Core.ECS;
using System.Linq;
using System;

namespace Assets.Scripts.Engine.ECS.Common
{
    public class PositionSystem : GameSystem
    {
        public PositionSystem(SignalBus signalBus) : base(signalBus)
        {
        }

        protected override bool ShouldProcessArchetype(Archetype archetype)
        {
            //we should process any entity with a position component and a model component
            return archetype.HasComponents<PositionComponent, GameObjectComponent>();
        }



        protected override void OnUpdate(Archetype archetype)
        {

            //get the position component
            var positionBatch = archetype.GetComponents<PositionComponent>();

            //Get the game object component
            var gameObjectBatch = archetype.GetComponents<GameObjectComponent>();

            //Update the position of the game object
            for (int i = 0; i < archetype.Count; i++)
            {
                UpdatePosition(gameObjectBatch[i], positionBatch[i]);
            }

        }

        private void UpdatePosition(GameObjectComponent gameObjectComponent, PositionComponent positionComponent)
        {
            gameObjectComponent.GameObject.transform.position = positionComponent.Position;
        }
    }
}