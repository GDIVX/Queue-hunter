using Assets.Scripts.Engine.ECS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameObjectCreationSystem : GameSystem
{



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
