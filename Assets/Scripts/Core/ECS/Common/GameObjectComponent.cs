using UnityEngine;
using Sirenix.OdinInspector;
using Assets.Scripts.Core.ECS.Interfaces;

namespace Assets.Scripts.Core.ECS.Common
{
    [System.Serializable]
    public class GameObjectComponent : DataComponent
    {
        [SerializeField]
        private GameObject gameObject;

        [SerializeField]
        bool createOnAwake;

        public GameObject GameObject
        {
            get
            {
                if (gameObject == null)
                {
                    CreateGameObject();
                }
                return gameObject;
            }
            private set
            {
                SafeSet(ref gameObject, value);
            }
        }

        public override IComponent Instantiate()
        {

            GameObjectComponent component = new();

            if (gameObject != null)
            {
                component.GameObject = gameObject;
            }

            if (createOnAwake)
            {
                component.CreateGameObject();
            }

            return component;
        }

        protected override void OnSetActive(bool value)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(value);
            }
        }

        void CreateGameObject()
        {
            string name = Entity == null ? gameObject == null ? "GameObject" : gameObject.name : Entity.ToString();

            gameObject = gameObject == null ? new GameObject(name) : Object.Instantiate(gameObject);

            if (Entity != null)
            {
                gameObject.AddComponent<EntityInspector>().Init(Entity);
            }
        }
    }
}