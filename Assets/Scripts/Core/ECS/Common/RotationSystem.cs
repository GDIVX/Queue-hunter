using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Engine.ECS.Common
{
    public class RotationSystem : GameSystem
    {

        protected override bool ShouldProcessEntity(IEntity entity)
        {
            //we should process any entity with a position component and a model component
            return entity.HasComponent<RotationComponent>()
                && entity.HasTag("HasGameObject");
        }

        protected override void OnUpdate(IEntity entity)
        {
            //get the position component
            var rotationComponent = entity.GetComponent<RotationComponent>();


            //update the entity root game object
            entity.GetGameObject().transform.rotation = Quaternion.Euler(rotationComponent.Rotation);

        }
    }
}