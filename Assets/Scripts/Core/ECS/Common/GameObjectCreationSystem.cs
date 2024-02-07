using Assets.Scripts.Core.ECS.Common;
using Assets.Scripts.Engine.ECS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// System that creates game objects for entities that have the "HasGameObject" tag.
/// </summary>
public class GameObjectCreationSystem : GameSystem
{
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
    }

    protected override bool ShouldProcessEntity(IEntity entity)
    {
        return entity.HasTag("HasGameObject");
    }
}
