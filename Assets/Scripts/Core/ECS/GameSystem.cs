using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Engine.ECS
{
    public abstract class GameSystem : MonoBehaviour
    {
        [SerializeField] protected List<Entity> entitiesToProcess = new List<Entity>();

        private void Awake()
        {
            //register this system with the entity manager
            ECSManager.Instance.RegisterSystem(this);
        }

        public virtual void OnEntityCreatedOrModified(Entity entity)
        {
            //schedule the entity to be checked
            RequestManager.Instance.Schedule(() =>
            {
                bool isEntityValid = ShouldProcessEntity(entity);

                //Do we processing this entity?
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
        public virtual void OnEntityAdded(Entity entity)
        {
        }

        /// <summary>
        /// Called a frame after an entity is added to the system.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void OnLateEntityAdded(Entity entity)
        {

        }

        public virtual void OnEntityRemoved(Entity entity)
        {
        }

        public virtual void OnEntityDeleted(Entity entity)
        {
            RequestManager.Instance.Schedule(() =>
            {
                // Remove the entity from the list
                RemoveEntity(entity);

            });
        }


        protected void AddEntity(Entity entity)
        {
            if (!ShouldProcessEntity(entity) || entitiesToProcess.Contains(entity)) return;

            entitiesToProcess.Add(entity);

            OnEntityAdded(entity);

            //use a coroutine to call the late on entity added
            StartCoroutine(LateOnEntityAddedCoroutine(entity));
        }

        private IEnumerator LateOnEntityAddedCoroutine(Entity entity)
        {
            yield return new WaitForEndOfFrame();
            OnLateEntityAdded(entity);

        }

        protected void RemoveEntity(Entity entity)
        {
            //do we have this entity?
            if (entitiesToProcess.Contains(entity))
            {
                OnEntityRemoved(entity);
                entitiesToProcess.Remove(entity);
            }
        }

        /// <summary>
        /// Update all the entities in the system. Recommended to be called in the Update method for the game.
        /// </summary>
        protected void UpdateEntities()
        {
            for (int i = 0; i < entitiesToProcess.Count; i++)
            {
                Entity entity = entitiesToProcess[i];
                OnUpdate(entity);
            }
        }

        /// <summary>
        /// Called for each entity when is being updated.
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void OnUpdate(Entity entity) { }

        /// <summary>
        /// Define the criteria weather or not to include an entity in the system.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected abstract bool ShouldProcessEntity(Entity entity);



        void OnDestroy()
        {
            // Unsubscribe from all the entities
            for (int i = 0; i < entitiesToProcess.Count; i++)
            {
                Entity entity = entitiesToProcess[i];
            }
        }

    }
}
