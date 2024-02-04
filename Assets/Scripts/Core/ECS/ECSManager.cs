using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets.Scripts.Engine.ECS
{
    /// <summary>
    /// The EntityManager is responsible for creating and destroying entities.
    /// </summary>
    public class ECSManager
    {
        public event Action<Entity> OnEntityCreated;
        public event Action<Entity> OnEntityModified;
        public Action<Entity> OnEntityInitialized;
        public event Action<Entity> OnEntityDeleted;


        //singleton
        private static ECSManager _instance;
        public static ECSManager Instance => _instance ?? (_instance = new ECSManager());

        //Dictionary of entities
        Dictionary<int, Entity> _entities = new();

        List<GameSystem> _systems = new();

        internal Entity CreateEntity(Action<Entity> onComplete = null)
        {
            var entity = new Entity();

            _entities.Add(entity.ID, entity);


            onComplete?.Invoke(entity);

            OnEntityCreated?.Invoke(entity);
            return entity;
        }

        internal void DestroyEntity(Entity entity)
        {

            entity.OnDestroyed?.Invoke(entity);
            OnEntityDeleted?.Invoke(entity);

            _entities.Remove(entity.ID);

        }

        public Entity Find(int id)
        {
            if (!_entities.ContainsKey(id))
                throw new Exception($"Entity with id {id} does not exist");

            return _entities[id];
        }

        public Entity[] FindAllEntitiesWithComponent<T>() where T : IComponent
        {
            var entities = new List<Entity>();

            foreach (var entity in _entities.Values)
            {
                if (entity.HasComponent<T>())
                {
                    entities.Add(entity);
                }
            }

            return entities.ToArray(); ;
        }

        public Entity[] FindAllEntitiesWithTag(string tag)
        {
            var entities = new List<Entity>();

            foreach (var entity in _entities.Values)
            {
                if (entity.HasTag(tag))
                {
                    entities.Add(entity);
                }
            }

            return entities.ToArray(); ;
        }

        internal void RegisterSystem(GameSystem gameSystem)
        {
            if (gameSystem is null)
            {
                throw new ArgumentNullException(nameof(gameSystem));
            }
            //subscribe to the events
            OnEntityInitialized += gameSystem.OnEntityCreatedOrModified;
            OnEntityDeleted += gameSystem.OnEntityDeleted;

            //iterate through all entities and add those that meet the criteria
            foreach (var entity in _entities.Values)
            {
                gameSystem.OnEntityCreatedOrModified(entity);
            }
            OnEntityCreated += gameSystem.OnEntityCreatedOrModified;
            OnEntityModified += gameSystem.OnEntityCreatedOrModified;

            _systems.Add(gameSystem);
        }

        public T GetSystem<T>() where T : GameSystem
        {
            foreach (var system in _systems)
            {
                if (system is T)
                {
                    return (T)system;
                }
            }

            return null;
        }

        public int GenerateUniqueId()
        {
            int id;
            do
            {
                id = Guid.NewGuid().GetHashCode(); // Use Guid to reduce the risk of collisions
            }
            while (_entities.ContainsKey(id));

            return id;
        }


        public static IEnumerator CreateEntitiesCoroutine(String addressableKey, int amount, Action<Entity> onLoaded = default)
        {
            // Start by loading the entity definition
            AsyncOperationHandle<Archetype> handle = Addressables.LoadAssetAsync<Archetype>(addressableKey);

            // Wait until the archetype is loaded
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Archetype archetype = handle.Result;
                for (int i = 0; i < amount; i++)
                {
                    Entity newEntity = archetype.CreateEntity();
                    onLoaded?.Invoke(newEntity);

                    // Yield return null will wait for the next frame before continuing the loop
                    yield return null;
                }
            }
            else
            {
                Debug.LogError($"Failed to load archetype with addressable key: {addressableKey}");
            }

            Addressables.Release(handle);
        }

        public static IEnumerator CreateEntitiesCoroutine(AssetReferenceT<Archetype> addressableRefrence, int amount, Action<Entity> onLoaded = default)
        {
            // Start by loading the entity definition
            AsyncOperationHandle<Archetype> handle = Addressables.LoadAssetAsync<Archetype>(addressableRefrence);

            // Wait until the archetype is loaded
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Archetype archetype = handle.Result;
                for (int i = 0; i < amount; i++)
                {
                    Entity newEntity = archetype.CreateEntity();
                    onLoaded?.Invoke(newEntity);

                    // Yield return null will wait for the next frame before continuing the loop
                    yield return null;
                }
            }
            else
            {
                Debug.LogError($"Failed to load archetype with addressable key: {addressableRefrence}");
            }

            // Optionally, release the handle if no longer needed
            Addressables.Release(handle);
        }
    }
}