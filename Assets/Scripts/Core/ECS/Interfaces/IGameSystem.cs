﻿using System;
using Zenject;

namespace Assets.Scripts.Engine.ECS
{
    public interface IGameSystem : IActivable, ITickable, ILateTickable
    {
        void Initialize();
        void OnEntityAdded(IEntity entity);
        void OnEntityCreatedOrModified(IEntity entity);
        void OnEntityDeleted(IEntity entity);
        void OnEntityRemoved(IEntity entity);
        void OnLateEntityAdded(IEntity entity);
        void OnLateUpdate(IEntity entity);

        void Destroy();

        event Action<IGameSystem> OnDestroyed;

    }
}