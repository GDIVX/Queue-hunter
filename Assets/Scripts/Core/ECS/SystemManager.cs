using Assets.Scripts.Engine.ECS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.ECS
{
    /// <summary>
    /// Provides a way to create and manage game systems.
    /// </summary>
    public class SystemManager : ISystemManager
    {
        List<IGameSystem> systems = new List<IGameSystem>();
        DiContainer _container;

        public SystemManager(DiContainer container)
        {
            _container = container;
        }

        public T Create<T>() where T : IGameSystem
        {
            // If we already have a system of this type, return it
            T existingSystem = GetSystem<T>();
            if (existingSystem != null)
            {
                return existingSystem;
            }

            T system = _container.Instantiate<T>(new object[] { _container.Resolve<SignalBus>() });
            system.Initialize();
            systems.Add(system);

            // Register to on destroyed event
            system.OnDestroyed += (IGameSystem s) => systems.Remove(s);

            // Manually register tickable and late tickable
            var tickableManager = _container.Resolve<TickableManager>();
            tickableManager.Add(system);
            tickableManager.AddLate(system);

            Debug.Log($"Created system {typeof(T).Name}");

            return system;
        }




        public T GetSystem<T>() where T : IGameSystem
        {
            return (T)systems.Find(x => x.GetType() == typeof(T));
        }
    }
}