using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CollisionComponent : DataComponent
{
    public override IComponent Instantiate()
    {
        throw new NotImplementedException();
    }
}
