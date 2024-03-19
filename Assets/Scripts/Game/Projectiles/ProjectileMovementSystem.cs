using Assets.Scripts.Core.ECS;
using Assets.Scripts.Engine.ECS;
using Assets.Scripts.Engine.ECS.Common;
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
            return archetype.HasComponents<ProjectileComponent, PositionComponent, RotationComponent>();
        }

        protected override void OnUpdate(Archetype archetype)
        {
        }
    }
}