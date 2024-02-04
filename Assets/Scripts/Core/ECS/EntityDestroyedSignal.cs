using Assets.Scripts.Engine.ECS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Signal for when an entity is destroyed
/// </summary>
public class EntityDestroyedSignal
{
    public EntityDestroyedSignal(Entity entity)
    {
        Entity = entity;
    }

    public Entity Entity { get; private set; }
}
