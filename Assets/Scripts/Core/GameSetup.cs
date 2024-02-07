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

        // Load asynchronously a model
        var c1Task = _componentsFactory.CreateComponentAsync<ModelComponent>("Cube");
        yield return new WaitUntil(() => c1Task.IsCompleted); // Wait for completion
        if (c1Task.Status == TaskStatus.RanToCompletion)
        {
            components.Add(c1Task.Result);
        }
        else
        {
            Debug.LogError("Failed to load ModelComponent.");
            yield break; // Exit early if failed
        }

        // Load asynchronously a position
        var c2Task = _componentsFactory.CreateComponentAsync<PositionComponent>("Position");
        yield return new WaitUntil(() => c2Task.IsCompleted); // Wait for completion
        if (c2Task.Status == TaskStatus.RanToCompletion)
        {
            components.Add(c2Task.Result);
        }
        else
        {
            Debug.LogError("Failed to load PositionComponent.");
            yield break; // Exit early if failed
        }

        // Callback with the loaded components
        callback?.Invoke(components.ToArray());
    }

    private void CreateEntities(IComponent[] components)
    {
        // Create a cube entity
        _entityFactory.Create("Cube", components, new string[] { "HasGameObject" });
    }

    private void CreateSystem()
    {
        //Object creation system
        _systemManager.Create<GameObjectCreationSystem>();

        //Model system
        _systemManager.Create<ModelSystem>();

        //Position system
        _systemManager.Create<PositionSystem>();
    }
}
