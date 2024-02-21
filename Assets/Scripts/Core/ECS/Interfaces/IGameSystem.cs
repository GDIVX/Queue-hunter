using Assets.Scripts.Core.ECS;
using System;
using Zenject;

namespace Assets.Scripts.Engine.ECS
{
    public interface IGameSystem : IActivable, ITickable, ILateTickable
    {
        void Initialize();
        void OnEntityCreated(IEntity entity);
        void OnEntityRemoved(IEntity entity);
        void OnLateEntityCreated(IEntity entity);        
        void Destroy();

        event Action<IGameSystem> OnDestroyed;

    }
}