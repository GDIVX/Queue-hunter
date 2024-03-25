using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Common;
using Assets.Scripts.Engine.ECS;
using Assets.Scripts.Engine.ECS.Common;
using Queue.Tools.TimeSystem;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Game.Projectiles
{
    public class ProjectileMovementSystem : GameSystem
    {
        public ProjectileMovementSystem(SignalBus signalBus) : base(signalBus)
        {
        }

        protected override bool ShouldProcessArchetype(Archetype archetype)
        {
            return archetype.HasComponents<ProjectileComponent, PositionComponent, GameObjectComponent>();
        }

        protected override void OnUpdate(Archetype archetype)
        {
            //get a batch of the projectile, position and rotation components

            var projectileBatch = archetype.GetComponents<ProjectileComponent>();
            var positionBatch = archetype.GetComponents<PositionComponent>();
            var gameObjectBatch = archetype.GetComponents<GameObjectComponent>();

            for (var i = 0; i < archetype.Count; i++)
            {
                var forward = gameObjectBatch[i].GameObject.transform.forward;
                var translation = CalculateTranslation(projectileBatch[i].speed, forward);
                positionBatch[i].Position += translation;
            }
        }

        private static Vector3 CalculateTranslation(float speed, Vector3 forward)
        {
            return (speed * GAME_TIME.GameDeltaTime) * forward;
        }
    }
}