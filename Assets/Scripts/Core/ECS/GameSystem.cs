﻿using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Engine.ECS
{
    public abstract class GameSystem : ITickable, ILateTickable, IGameSystem
    {
        [SerializeField] protected List<IEntity> entitiesToProcess = new List<IEntity>();

        [Inject]
        SignalBus _signalBus;

        [Inject]
        IRequestable _requestHandler;

        bool _isActive = true;

        event Action<IGameSystem> OnDestroyed;

        protected IRequestable RequestHandler { get => _requestHandler; private set => _requestHandler = value; }

        event Action<IGameSystem> IGameSystem.OnDestroyed
        {
            add
            {
                OnDestroyed += value;
            }

            remove
            {
                OnDestroyed -= value;
            }
        }

        /// <summary>
        /// Called when the system is created
        /// </summary>
        public virtual void Initialize()
        {
            //subscribe to the entity created signal
            _signalBus.Subscribe<EntityCreatedSignal>(x => OnEntityCreatedOrModified(x.Entity));
            _signalBus.Subscribe<EntityModifiedSignal>(x => OnEntityCreatedOrModified(x.Entity));
        }

        public virtual void OnEntityCreatedOrModified(IEntity entity)
        {
            //schedule the entity to be checked
            RequestHandler.Schedule(() =>
            {
                bool isEntityValid = ShouldProcessEntity(entity);

                //Are we processing this entity?
                if (entitiesToProcess.Contains(entity) && !isEntityValid)
                {
                    //remove the entity from the list
                    RemoveEntity(entity);

                }
                else if (isEntityValid)
                {
                    //add the entity to the list
                    AddEntity(entity);
                }
            });

        }

        /// <summary>
        /// Called when an entity is added to the system.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void OnEntityAdded(IEntity entity)
        {
        }

        /// <summary>
        /// Pause or resume the system. A system would not process entities while inactive.
        /// </summary>
        /// <param name="active"></param>
        public void SetActive(bool active)
        {
            _isActive = active;
        }
        public void Tick()
        {
            if (!_isActive) return;

            for (int i = 0; i < entitiesToProcess.Count; i++)
            {
                IEntity entity = entitiesToProcess[i];
                OnUpdate(entity);
            }
        }
        public void LateTick()
        {
            if (!_isActive) return;

            for (int i = 0; i < entitiesToProcess.Count; i++)
            {
                IEntity entity = entitiesToProcess[i];
                OnLateUpdate(entity);
            }
        }


        /// <summary>
        /// Called a frame after an entity is added to the system.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void OnLateEntityAdded(IEntity entity)
        {
        }

        public virtual void OnEntityRemoved(IEntity entity)
        {
        }

        public virtual void OnEntityDeleted(IEntity entity)
        {
            RequestHandler.Schedule(() =>
            {
                // Remove the entity from the list
                RemoveEntity(entity);

            });
        }


        protected void AddEntity(IEntity entity)
        {
            if (!ShouldProcessEntity(entity) || entitiesToProcess.Contains(entity)) return;

            entitiesToProcess.Add(entity);

            OnEntityAdded(entity);

        }
        public virtual void OnLateUpdate(IEntity entity)
        {
        }


        protected void RemoveEntity(IEntity entity)
        {
            //do we have this entity?
            if (entitiesToProcess.Contains(entity))
            {
                OnEntityRemoved(entity);
                entitiesToProcess.Remove(entity);
            }
        }


        /// <summary>
        /// Called for each entity when is being updated.
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void OnUpdate(IEntity entity) { }

        /// <summary>
        /// Define the criteria weather or not to include an entity in the system.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected abstract bool ShouldProcessEntity(IEntity entity);

        /// <summary>
        /// Destroy the system
        /// </summary>
        public void Destroy()
        {
            RequestHandler.Schedule(() =>
            {
                OnDestroy();
                // Unsubscribe from all the entities
                for (int i = 0; i < entitiesToProcess.Count; i++)
                {
                    IEntity entity = entitiesToProcess[i];
                }
            });
        }
        protected virtual void OnDestroy()
        {
            OnDestroyed?.Invoke(this);
        }

    }
}