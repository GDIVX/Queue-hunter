using Assets.Scripts.Engine.ECS;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.ECS.Common
{
    public class GameObjectHandlerSystem : GameSystem
    {
        public GameObjectHandlerSystem(SignalBus signalBus) : base(signalBus)
        {
        }

        protected override bool ShouldProcessArchetype(Archetype archetype)
        {
            return archetype.HasComponent<GameObjectComponent>();
        }

        public override void OnEntityCreated(IEntity entity)
        {
            base.OnEntityCreated(entity);

            GameObjectComponent com = entity.GetComponent<GameObjectComponent>();

            GameObject gameObject = CreateGameObject(com);
        }

        GameObject CreateGameObject(GameObjectComponent component)
        {
            string name = component.Entity == null ? component.GameObject == null ? "GameObject" : component.GameObject.name : component.Entity.ToString();

            component.GameObject = component.GameObject == null ? new GameObject(name) : Object.Instantiate(component.GameObject);

            if (component.Entity != null)
            {
                component.GameObject.AddComponent<EntityInspector>().Init(component.Entity);
            }

            return component.GameObject;
        }
    }
}