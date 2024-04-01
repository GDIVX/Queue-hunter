using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Common;
using Assets.Scripts.Engine.ECS;
using Assets.Scripts.Engine.ECS.Common;
using Assets.Scripts.Game.Movement;
using Queue.Tools.TimeSystem;
using UnityEngine;
using Zenject;

namespace Game.AI
{
    public class AIMovementSystem : GameSystem
    {
        public AIMovementSystem(SignalBus signalBus) : base(signalBus)
        {
        }

        protected override bool ShouldProcessArchetype(Archetype archetype)
        {
            return archetype
                .HasComponents<MovementComponent, PositionComponent, RotationComponent, PathfinderComponent,
                    GameObjectComponent>();
        }

        protected override void OnUpdate(Archetype archetype)
        {
            var gameObjectBatch = archetype.GetComponents<GameObjectComponent>();
            var pathfindingBatch = archetype.GetComponents<PathfinderComponent>();
            var positionBatch = archetype.GetComponents<PositionComponent>();
            var movementBatch = archetype.GetComponents<MovementComponent>();
            var rotationBatch = archetype.GetComponents<RotationComponent>();

            for (int i = 0; i < archetype.Count; i++)
            {
                //calculate the movement
                Vector3 direction = pathfindingBatch[i].Target - positionBatch[i].Position;
                var movement = direction.normalized * (movementBatch[i].Speed * GAME_TIME.GameDeltaTime);

                //characterController.Move(movement);

                positionBatch[i].Position += movement;

                if (movement != Vector3.zero)
                {
                    //rotate the agent towards the movement direction
                    var rotation = Quaternion.LookRotation(direction.normalized , Vector3.up).eulerAngles;
                    rotationBatch[i].Rotation = rotation;
                }
            }
        }
    }
}