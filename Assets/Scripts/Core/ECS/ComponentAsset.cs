using UnityEngine;

namespace Assets.Scripts.Core.ECS
{
    public abstract class ComponentAsset<T> : ComponentAsset
    {
        [SerializeField] protected T _value;

    }

    public abstract class ComponentAsset : ScriptableObject 
    {
        public abstract object Instantiate();

    }
}