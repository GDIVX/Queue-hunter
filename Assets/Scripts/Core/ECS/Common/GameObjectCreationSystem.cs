using Assets.Scripts.Engine.ECS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectCreationSystem : GameSystem
{
    protected override bool ShouldProcessEntity(Entity entity)
    {
        return entity.HasTag("HasGameObject");
    }


    public override void OnEntityAdded(Entity entity)
    {
        // Create the game object
        GameObject newGameObject = new GameObject(entity.ToString());
        entity.SetRootGameObject(newGameObject);

        //add an inspector
        newGameObject.AddComponent<EntityInspector>().Init(entity);
    }

}
