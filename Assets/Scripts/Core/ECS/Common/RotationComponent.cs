using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using System;
using System.Collections;
using UnityEngine;


namespace Assets.Scripts.Engine.ECS.Common
{
    [Serializable]
    public struct RotationComponent : IComponent
    {
        public Vector3 rotation;

        public RotationComponent(Vector3 rotation) : this()
        {
            this.rotation = rotation;
            IsDirty = true;
            IsActive = true;
        }

        public bool IsActive { get; set; }
        public bool IsDirty { get; set; }

        public object Clone()
        {
            return new RotationComponent(rotation);
        }
    }
}