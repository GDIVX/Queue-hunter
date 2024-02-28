using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Game.Movement
{
    public struct MovementParamsComponent : IComponent
    {
        public float speed;

        public Vector3 lastDir;

        public MovementParamsComponent(float speed) : this()
        {
            this.speed = speed;

            IsActive = true;
            IsDirty = true;
        }

        public bool IsActive { get; set; }
        public bool IsDirty { get; set; }

        public object Clone()
        {
            return new MovementParamsComponent(this.speed);
        }
    }
}