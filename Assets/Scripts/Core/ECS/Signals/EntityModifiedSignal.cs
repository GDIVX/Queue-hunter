using Assets.Scripts.Core.ECS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityModifiedSignal
{
    public EntityModifiedSignal(Entity entity)
    {
        Entity = entity;
    }
    public Entity Entity { get; private set; }

}
