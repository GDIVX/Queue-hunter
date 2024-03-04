using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Combat
{
    [Serializable]
    public class CollisionComponent : DataComponent
    {
        [SerializeField] ColliderTriggerHandler handler;

        public ColliderTriggerHandler Handler
        {
            get => handler;
        }
        public override IComponent Instantiate()
        {
            return new CollisionComponent();
        }
    }
}