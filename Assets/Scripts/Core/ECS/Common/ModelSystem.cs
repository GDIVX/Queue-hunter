using Assets.Scripts.Engine.ECS;
using Assets.Scripts.Engine.ECS.Common;
using System;
using System.Diagnostics;
using UnityEngine;

namespace Assets.Scripts.ECS.Common
{
    public class ModelSystem : GameSystem
    {
        protected override bool ShouldProcessEntity(Entity entity)
        {
            var hasModel = entity.HasComponent<ModelComponent>();
            var hasGameObject = entity.HasTag("HasGameObject");
            return hasModel && hasGameObject;
        }

        private void Start()
        {
            UpdateEntities();
        }

        public override void OnEntityAdded(Entity entity)
        {
            base.OnEntityAdded(entity);
            UpdateEntities();
        }

        private void Update()
        {
            UpdateEntities();
        }

        public override void OnEntityRemoved(Entity entity)
        {
            //remove the model
            if (entity.HasComponent<ModelComponent>())
            {
                var model = entity.GetComponent<ModelComponent>().Model;
                Destroy(model);
            }


        }

        public override void OnLateEntityAdded(Entity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }


            //instantiate a model as a child of the entity's root
            var modelCom = entity.GetComponent<ModelComponent>();

            if (modelCom == null)
            {
                UnityEngine.Debug.LogError($"{entity} was not assigned a model component");
                return;
            }

            var prefab = modelCom.Model;
            var model = Instantiate(prefab, entity.GetRootGameObject().transform);
            model.name = $"{entity}_Model";

            modelCom.Model = model;

            //set position as dirty 
            entity.GetComponent<PositionComponent>().IsDirty = true;

            //when the model is dirty, it might be because it is deactivated.
            //if it is deactivated, we need to deactivate the entity
            modelCom.OnDirty += (component, isDirty) =>
            {
                if (isDirty)
                {
                    model.SetActive(modelCom.IsActive);
                }
            };
        }
    }
}