using UnityEngine;

namespace Assets.Scripts.Core.ECS
{
    public abstract class ComponentAsset<T> : ComponentAsset
    {
        [SerializeField] protected T Value;

    }

    public abstract class ComponentAsset : ScriptableObject 
    {
        public abstract object Instantiate();

    }
}