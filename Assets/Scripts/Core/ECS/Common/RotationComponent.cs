using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using System.Collections;
using UnityEngine;


namespace Assets.Scripts.Engine.ECS.Common
{
    //[CreateAssetMenu(fileName = "RotationComponent", menuName = "ECS/Components/RotationComponent")]
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