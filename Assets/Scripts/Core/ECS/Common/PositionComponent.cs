using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Engine.ECS.Common
{
    [System.Serializable]
    public class PositionComponent : DataComponent
    {
        Vector3 position;

        [ShowInInspector]
        public Vector3 Position
        {
            get
            {
                return position;
            }

            set
            {
                //when disabled, the component is read only
                if (!IsActive) return;

                position = value;
                IsDirty = true;
            }
        }

        public override IComponent Instantiate()
        {
            PositionComponent component = new PositionComponent()
            {
                Position = position
            };
            return component;
        }
    }
}