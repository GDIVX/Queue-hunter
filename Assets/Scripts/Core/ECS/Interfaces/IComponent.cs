using Assets.Scripts.Engine.ECS;
using UnityEngine;

namespace Assets.Scripts.Core.ECS.Interfaces
{
    public interface IComponent : IActivable
    {

        public void SetParent(IEntity entity);

        public IEntity GetParent();
        IComponent Instantiate();
    }
}