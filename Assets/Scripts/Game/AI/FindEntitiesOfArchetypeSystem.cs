using System.Linq;
using Assets.Scripts.Core.ECS;
using Assets.Scripts.Engine.ECS;
using Queue.Tools.Debag;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

namespace Game.AI
{
    public class FindEntitiesOfArchetypeSystem : GameSystem
    {
        public FindEntitiesOfArchetypeSystem(SignalBus signalBus) : base(signalBus)
        {
        }

        protected override bool ShouldProcessArchetype(Archetype archetype)
        {
            return archetype.HasComponent<FindEntitiesOfArchetypeComponent>();
        }

        protected override void OnUpdate(Archetype archetype)
        {
            var batch = archetype.GetComponents<FindEntitiesOfArchetypeComponent>();

            for (int i = 0; i < archetype.Count; i++)
            {
                var component = batch[i];

                if (component.Entities != null) continue;
                if(component.ArchetypeName.IsNullOrWhitespace()) continue;

                //Look for 
                var playerArch = Archetype.GetArchetype(component.ArchetypeName);
                var entities = playerArch.GetEntities();

                component.Entities = entities.ToList();
                
            }
        }
    }
}