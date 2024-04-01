using Assets.Scripts.Core.ECS.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Core.ECS.Common
{
    public abstract class UnityComponentToECS<T> : UnityComponentToECS where T : Component
    {
        [ReadOnly, SerializeField] T _unityComponent;
        public T UnityComponent { get => _unityComponent; private set => _unityComponent = value; }

        public override Component Install(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out T unityComponent))
            {
                UnityComponent = unityComponent;
            }
            else
            {
                UnityComponent = gameObject.GetComponentInChildren<T>();
                if (UnityComponent == null)
                {
                    UnityComponent = gameObject.AddComponent<T>();
                }
            }

            return unityComponent;
        }

        public abstract override IComponent Instantiate();
    }

    public abstract class UnityComponentToECS : DataComponent
    {
        public abstract Component Install(GameObject gameObject);
    }
}