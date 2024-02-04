using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Engine.ECS
{
    public class GameObjectCleanupSystem : GameSystem
    {
        protected override bool ShouldProcessEntity(Entity entity)
        {
            return entity.HasTag("HasGameObject");
        }

        public override void OnEntityAdded(Entity entity)
        {
            entity.OnDestroyed += OnEntityDestroyed;
        }

        void OnEntityDestroyed(Entity entity)
        {
            //destroy the game object
            Destroy(entity.GetRootGameObject());
        }
    }
}