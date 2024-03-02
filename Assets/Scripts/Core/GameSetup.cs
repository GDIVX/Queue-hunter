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
using Assets.Scripts.ECS;
using System.Reflection;
using UnityEngine.Rendering.VirtualTexturing;
using System.Linq;
using Assets.Scripts.Game.Movement;
using Assets.Scripts.Game.Input;

public class GameSetup : MonoBehaviour
{
    [Inject]
    protected ISystemManager _systemManager;
    [Inject]
    protected IEntityFactory _entityFactory;
    [Inject]
    protected IComponentsFactory _componentsFactory;

    [SerializeField] List<ArchetypeAsset> _archetypes;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        CreateSystems();

        foreach (var archetype in _archetypes)
        {
            CreateArchetype(archetype);
        }
    }

    private void CreateArchetype(ArchetypeAsset archetype)
    {
        //Create components
        var components = new List<IComponent>();
        foreach (var component in archetype.components)
        {
            IComponent initComponent = _componentsFactory.Instantiate(component);
            components.Add(initComponent);
        }

        //Create entity
        IEntity entity = _entityFactory.Create(archetype.name, components.ToArray(), archetype.Tags.ToArray());
    }

    protected void CreateSystems()
    {
        //// Get all types in the current assembly that implement IGameSystem and are class (not interface)
        //var gameSystemTypes = Assembly.GetExecutingAssembly().GetTypes()
        //    .Where(t => typeof(IGameSystem).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        //foreach (var type in gameSystemTypes)
        //{
        //    MethodInfo createMethod = _systemManager.GetType().GetMethod(nameof(ISystemManager.Create), BindingFlags.Instance | BindingFlags.Public);
        //    if (createMethod != null)
        //    {
        //        // Assuming Create<T>() is a generic method we want to call on _systemManager
        //        MethodInfo genericMethod = createMethod.MakeGenericMethod(type);
        //        genericMethod.Invoke(_systemManager, null);
        //    }
        //    else
        //    {
        //        Debug.LogError($"Create method not found in {_systemManager.GetType().Name}.");
        //    }
        //}

        _systemManager.Create<ModelSystem>();
        _systemManager.Create<PositionSystem>();
        _systemManager.Create<RotationSystem>();
        _systemManager.Create<MovementSystem>();
        _systemManager.Create<InputSystem>();
        _systemManager.Create<DashSystem>();
    }
}
