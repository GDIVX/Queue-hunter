using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Core.ECS;
using Zenject;
using Assets.Scripts.ECS.Common;
using Assets.Scripts.Engine.ECS;
using Assets.Scripts.Engine.ECS.Common;
using UnityEngine.AddressableAssets;
using Assets.Scripts.Core.ECS.Interfaces;
using System.Threading.Tasks;
using Assets.Scripts.Core.ECS.Common;

public class GameSetup : MonoBehaviour
{
    //Test script to see if the ECS works

    //We will attempt to create a cube using the ECS

    //We will need to inject singletons
    [Inject]
    ISystemManager _systemManager;
    [Inject]
    IEntityFactory _entityFactory;
    [Inject]
    IComponentsFactory _componentsFactory;

    private void Start()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        // First, create all the systems that we need
        CreateSystem();

        // Then, create the components and entities
        yield return StartCoroutine(CreateComponentsCoroutine(components =>
        {
            // After components are created, create the entities
            CreateEntities(components);
        }));
    }

    private IEnumerator CreateComponentsCoroutine(Action<IComponent[]> callback)
    {
        List<IComponent> components = new List<IComponent>();

        // Flag to keep track of any load failure
        bool failed = false;

        // Define an action to handle failure, which sets the failed flag and exits the coroutine
        Action handleFailure = () => failed = true;

        // Load asynchronously a game object
        yield return LoadComponentAsync<GameObjectComponent>("GameObject", component => components.Add(component), handleFailure);
        if (failed) yield break;

        // Load asynchronously a model
        yield return LoadComponentAsync<ModelComponent>("Cube", component => components.Add(component), handleFailure);
        if (failed) yield break;

        // Load asynchronously a position
        yield return LoadComponentAsync<PositionComponent>("Position", component => components.Add(component), handleFailure);
        if (failed) yield break;

        // Callback with the loaded components
        callback?.Invoke(components.ToArray());
    }


    private IEnumerator LoadComponentAsync<T>(string address, Action<T> onSuccess, Action onFailure) where T : class, IComponent
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


    private void CreateEntities(IComponent[] components)
    {
        // Create a cube entity
        _entityFactory.Create("Cube", components, new string[] { });
    }

    private void CreateSystem()
    {
        //Object creation system
        //_systemManager.Create<GameObjectCreationSystem>();

        //Model system
        _systemManager.Create<ModelSystem>();

        //Position system
        _systemManager.Create<PositionSystem>();
    }
}
