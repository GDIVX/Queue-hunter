using Assets.Scripts.Engine.ECS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.ECS
{
    /// <summary>
    /// Provides a way to create and manage game systems.
    /// </summary>
    public class SystemManager : ISystemManager
    {
        List<IGameSystem> systems = new List<IGameSystem>();

        public T CreateSystem<T>() where T : IGameSystem, new()
        {
            //If we already have a system of this type, return it

            T existingSystem = GetSystem<T>();
            if (existingSystem != null)
            {
                return existingSystem;
            }

            T system = new T();
            system.Initialize();
            systems.Add(system);

            //register to on destroyed event
            system.OnDestroyed += (IGameSystem system) => systems.Remove(system);

            return system;
        }


        public T GetSystem<T>() where T : IGameSystem
        {
            return (T)systems.Find(x => x.GetType() == typeof(T));
        }
    }
}