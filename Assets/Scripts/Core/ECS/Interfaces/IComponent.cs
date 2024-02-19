using Assets.Scripts.Engine.ECS;
using UnityEngine;

namespace Assets.Scripts.Core.ECS.Interfaces
{
    public interface IComponent : IActivable
    {
        public T Instantiate<T>() where T : ScriptableObject, IComponent;

        public void SetParent(IEntity entity);

        public IEntity GetParent();

    }
}