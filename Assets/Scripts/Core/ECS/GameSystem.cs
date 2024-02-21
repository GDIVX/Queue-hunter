using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Engine.ECS
{
    public abstract class GameSystem : IGameSystem
    {
        List<Archetype> _archetypes = new();

        [Inject]
        SignalBus _signalBus;

        [Inject]
        IRequestable _requestHandler;
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
        protected GameSystem(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        event Action<IGameSystem> OnDestroyed;

        public bool IsActive { get; set; } = true;


        /// <summary>
        /// Called when the system is created
        /// </summary>
        public virtual void Initialize()
        {
            //register to archetype creations

            Archetype.OnArchetypeCreated += (arch) =>
            {
                if (ShouldProcessArchetype(arch))
                {
                    AddArchetype(arch);
                }
            };

            //subscribe to the entity created signal
            _signalBus.Subscribe<EntityCreatedSignal>(x => OnEntityCreated(x.Entity));
            _signalBus.Subscribe<EntityDestroyedSignal>(x => OnEntityRemoved(x.Entity));

        }


        protected abstract bool ShouldProcessArchetype(Archetype archetype);

        /// <summary>
        /// Called when an entity is added to the system.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void OnEntityCreated(IEntity entity)
        {
            _requestHandler.Schedule(() => OnLateEntityCreated(entity));
        }
        /// <summary>
        /// Called a frame after an entity is added to the system.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void OnLateEntityCreated(IEntity entity)
        {
        }

        public virtual void OnEntityRemoved(IEntity entity)
        {
        }

        /// <summary>
        /// Pause or resume the system. A system would not process entities while inactive.
        /// </summary>
        /// <param name="active"></param>
        public void SetActive(bool active)
        {
            IsActive = active;
        }
        public void Tick()
        {
            if (!IsActive) return;
            foreach (var archetype in _archetypes)
            {
                OnUpdate(archetype);
            }

        }
        public void LateTick()
        {
            if (!IsActive) return;
            foreach (var archetype in _archetypes)
            {
                OnLateUpdate(archetype);
            }
        }






        /// <summary>
        /// Destroy the system
        /// </summary>
        protected void AddEntity(IEntity entity)
        {
            OnEntityCreated(entity);

            //Use a request to defer OnLateEntityAdded
            RequestHandler.Schedule(() => OnLateEntityCreated(entity));

        }
        public void Destroy()
        {
            RequestHandler.Schedule(() =>
            {
                OnDestroy();
            });
        }

        /// <summary>
        /// Called for each entity when is being updated.
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void OnUpdate(Archetype archetype) { }
        public virtual void OnLateUpdate(Archetype archetype)
        {
        }

        protected virtual void OnDestroy()
        {
            OnDestroyed?.Invoke(this);
        }
        private void AddArchetype(Archetype archetype)
        {
            if (!_archetypes.Contains(archetype))
            {
                _archetypes.Add(archetype);
            }
        }

    }
}
