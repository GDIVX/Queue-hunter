using Zenject;

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
            return entity.HasComponent<PositionComponent>()
                && entity.HasTag("HasGameObject");
        }



        protected override void OnUpdate(IEntity entity)
        {
            //get the position component
            var positionComponent = entity.GetComponent<PositionComponent>();


            //update the entity root game object
            entity.GetGameObject().transform.position = positionComponent.Position;

        }
    }
}