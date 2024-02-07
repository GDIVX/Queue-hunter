using Assets.Scripts.Core.ECS.Common;
using Assets.Scripts.Engine.ECS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// System that creates game objects for entities that have the "HasGameObject" tag.
/// </summary>
public class GameObjectCreationSystem : GameSystem
{
    HashSet<Guid> guids = new HashSet<Guid>();
    public GameObjectCreationSystem(SignalBus signalBus) : base(signalBus)
    {
    }

    public override void OnEntityAdded(IEntity entity)
    {
        // Create the game object
        GameObject newGameObject = new GameObject(entity.ToString());
        entity.SetRootGameObject(newGameObject);

        //add an inspector
        newGameObject.AddComponent<EntityInspector>().Init(entity);

        //Remove the entity from the system to avoid future processing
        guids.Add(entity.ID);
        RemoveEntity(entity);
    }

    public override void OnEntityDeleted(IEntity entity)
    {
        if (guids.Contains(entity.ID))
        {
            guids.Remove(entity.ID);
        }

    }

    protected override bool ShouldProcessEntity(IEntity entity)
    {
        return entity.HasTag("HasGameObject") && !guids.Contains(entity.ID);
    }
}
