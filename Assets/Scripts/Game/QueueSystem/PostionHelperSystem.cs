using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Common;
using Assets.Scripts.Engine.ECS;
using Zenject;

namespace Scirpts
{
    public class PostionHelperSystem : GameSystem
    {
        public PostionHelperSystem(SignalBus signalBus) : base(signalBus)
        {
        }

        protected override bool ShouldProcessArchetype(Archetype archetype)
        {
            return archetype.HasTag("Player");
        }

        public override void OnEntityCreated(IEntity entity)
        {
            base.OnEntityCreated(entity);
            ShotPostionHelper.Player = entity.GetComponent<GameObjectComponent>().GameObject;
        }
    }
}