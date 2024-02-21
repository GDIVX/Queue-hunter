using UnityEngine;
using Sirenix.OdinInspector;

namespace Assets.Scripts.Core.ECS.Common
{
    [CreateAssetMenu(fileName = "GameObjectComponent", menuName = "ECS/GameObjectComponent")]
    public class GameObjectComponent : DataComponent
    {
        [ShowInInspector, ReadOnly]
        private GameObject gameObject;

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

        protected override void OnSetActive(bool value)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(value);
            }
        }

        void CreateGameObject()
        {
            if (Entity == null)
            {
                Debug.LogError("Entity is null");
                return;
            }

            gameObject = new GameObject(Entity.ToString());
            gameObject.AddComponent<EntityInspector>().Init(Entity);
        }

        private void OnDestroy()
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
}