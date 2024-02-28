using Assets.Scripts.Core.ECS.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Core.ECS.Common
{
    [System.Serializable]
    public struct PositionComponent : IComponent
    {
        public Vector3 Position;

        public PositionComponent(Vector3 position) : this()
        {
            Position = position;
            IsActive = true;
            IsDirty = true;
        }

        public bool IsActive { get; set; }
        public bool IsDirty { get; set; }

        public object Clone()
        {
            PositionComponent component = new PositionComponent(Position);
            return component;
        }

    }
}