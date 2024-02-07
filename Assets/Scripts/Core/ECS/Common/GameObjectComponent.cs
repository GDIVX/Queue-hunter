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

        void CreateGameObject()
        {
            if (Entity == null)
            {
                Debug.LogError("Entity is null");
                return;
            }

            GameObject = new GameObject(Entity.ToString());
            GameObject.AddComponent<EntityInspector>().Init(Entity);
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