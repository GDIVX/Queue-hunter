using Assets.Scripts.Core.ECS;
using Assets.Scripts.Engine.ECS;
using Assets.Scripts.Engine.ECS.Common;
using Assets.Scripts.Game.Movement;
using Queue.Tools.Debag;
using UnityEngine;
using Zenject;

namespace Game.AI
{
    public class FollowEntitySystem : GameSystem
    {
        public FollowEntitySystem(SignalBus signalBus) : base(signalBus)
        {
        }

        protected override bool ShouldProcessArchetype(Archetype archetype)
        {
            return archetype.HasComponents<FindEntitiesOfArchetypeComponent , PathfinderComponent>() && archetype.HasTag("FollowEntity");
        }

        protected override void OnUpdate(Archetype archetype)
        {
            var findBatch = archetype.GetComponents<FindEntitiesOfArchetypeComponent>();
            var pathfinderBatch = archetype.GetComponents<PathfinderComponent>();

            for (int i = 0; i < archetype.Count; i++)
            {
                //if the entity hasn't found anything, return
                if (findBatch[i].Entities.Count == 0) return;

                Vector3 followPosition = GetFollowPosition(findBatch[i]);
                pathfinderBatch[i].Target = followPosition;
                
                CoreLogger.Log(followPosition);
            }
        }

        private Vector3 GetFollowPosition(FindEntitiesOfArchetypeComponent findCom)
        {
            //if we only have a single entity, select it. Otherwise, select one at random
            var entities = findCom.Entities;
            IEntity entity = entities.Count > 1 ? entities[Random.Range(0, entities.Count)] : entities[0];

            //if the entity has a position, return it. Otherwise,return default
            if (entity.TryGetComponent(out PositionComponent positionComponent))
            {
                return positionComponent.Position;
            }

            return default;
        }
    }
}