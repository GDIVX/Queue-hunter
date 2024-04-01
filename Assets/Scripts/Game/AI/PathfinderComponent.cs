using System;
using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Common;
using Assets.Scripts.Core.ECS.Interfaces;
using UnityEngine;

namespace Game.AI
{
    [Serializable]
    public class PathfinderComponent : DataComponent
    {
        public Vector3 Target { get; set; }

        public override IComponent Instantiate()
        {
            PathfinderComponent component = new PathfinderComponent();

            return component;
        }
    }
}