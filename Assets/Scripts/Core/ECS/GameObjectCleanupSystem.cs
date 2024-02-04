using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Engine.ECS
{
    public class GameObjectCleanupSystem : GameSystem
    {

        protected override bool ShouldProcessEntity(IEntity entity)
        {
            return entity.HasTag("HasGameObject");
        }

        public override void OnEntityDeleted(IEntity entity)
        {
            base.OnEntityDeleted(entity);
            UnityEngine.Object.Destroy(entity.GetGameObject());
        }
    }
}