using Assets.Scripts.Core.ECS;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Engine.ECS.Common
{
    [CreateAssetMenu(fileName = "PositionComponent", menuName = "ECS/Components/Position")]
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
    }
}