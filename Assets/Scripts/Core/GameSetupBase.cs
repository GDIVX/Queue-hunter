using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core
{
    public abstract class GameSetupBase : MonoBehaviour
    {

        [Inject]
        protected ISystemManager _systemManager;
        [Inject]
        protected IEntityFactory _entityFactory;
        [Inject]
        protected IComponentsFactory _componentsFactory;

        private void Start()
        {
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            // First, create all the systems that we need
            CreateSystems();

            // Then, create the components and entities
            yield return StartCoroutine(CreateComponentsCoroutine(components =>
            {
                // After components are created, create the entities
                CreateEntities(components);
            }));
        }

        protected IEnumerator LoadComponentAsync<T>(string address, Action<T> onSuccess, Action onFailure) where T : ScriptableObject, IComponent
        {
            Task<T> task = _componentsFactory.CreateComponentAsync<T>(address);
            yield return new WaitUntil(() => task.IsCompleted);

            if (task.Status == TaskStatus.RanToCompletion && task.Result != null)
            {
                onSuccess?.Invoke(task.Result);
            }
            else
            {
                Debug.LogError($"Failed to load {typeof(T).Name} with identifier \"{address}\".\n {task.Exception}");
                onFailure?.Invoke();
            }
        }

        protected abstract void CreateSystems();
        protected abstract IEnumerator CreateComponentsCoroutine(Action<IComponent[]> callback);

        protected abstract void CreateEntities(IComponent[] components);

    }
}