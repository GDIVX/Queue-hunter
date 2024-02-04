using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Engine.ECS.Common
{
    public class RotationSystem : GameSystem
    {

        protected override bool ShouldProcessEntity(Entity entity)
        {
            //we should process any entity with a position component and a model component
            return entity.HasComponent<RotationComponent>()
                && entity.HasTag("HasGameObject");
        }

        private void Update()
        {
            UpdateEntities();
        }

        protected override void OnUpdate(Entity entity)
        {
            //get the position component
            var rotationComponent = entity.GetComponent<RotationComponent>();


            //update the entity root game object
            entity.GetRootGameObject().transform.rotation = Quaternion.Euler(rotationComponent.Rotation);

        }
    }
}