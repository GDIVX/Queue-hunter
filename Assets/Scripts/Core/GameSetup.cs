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
using Assets.Scripts.Core;

public class GameSetup : GameSetupBase
{

    protected override IEnumerator CreateComponentsCoroutine(Action<IComponent[]> callback)
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
    protected override void CreateEntities(IComponent[] components)
    {
        // Create a cube entity
        _entityFactory.Create("Cube", components, new string[] { });
    }

    protected override void CreateSystems()
    {

        //Model system
        _systemManager.Create<ModelSystem>();

        //Position system
        _systemManager.Create<PositionSystem>();
    }
}
