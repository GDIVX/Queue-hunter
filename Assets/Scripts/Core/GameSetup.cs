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

        //Model system
        _systemManager.Create<ModelSystem>();

        //Position system
        _systemManager.Create<PositionSystem>();

        //rotation
        _systemManager.Create<RotationSystem>();
    }
}
