﻿using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Common;
using Assets.Scripts.Engine.ECS;
using Assets.Scripts.Engine.ECS.Common;
using System;
using System.Diagnostics;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.ECS.Common
{
    public class ModelSystem : GameSystem
    {
        public ModelSystem(SignalBus signalBus) : base(signalBus)
        {
        }

        protected override bool ShouldProcessArchetype(Archetype archetype)
        {
            return archetype.HasComponents<ModelComponent, GameObjectComponent>();
        }

        public override void OnEntityRemoved(IEntity entity)
        {
            //remove the model

            if (entity.HasComponent<ModelComponent>())
            {
                var model = entity.GetComponent<ModelComponent>().Model;
                UnityEngine.Object.Destroy(model);
            }


        }

        public override void OnLateEntityCreated(IEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }


            //instantiate a model as a child of the entity's root
            var modelCom = entity.GetComponent<ModelComponent>();
            var gameObjectCom = entity.GetComponent<GameObjectComponent>();


            var prefab = modelCom.Model;
            var model = UnityEngine.Object.Instantiate(prefab, gameObjectCom.GameObject.transform);
            model.name = $"{entity}_Model";

            modelCom.Model = model;

            //set position as dirty 
            var com = entity.GetComponent<PositionComponent>();
            com.IsDirty = true;

        }
    }
}