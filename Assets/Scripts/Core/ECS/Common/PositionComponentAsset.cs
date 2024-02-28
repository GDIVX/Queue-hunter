using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Core.ECS.Common
{
    [CreateAssetMenu(fileName = "PositionComponent", menuName = "ECS/Common/Position")]
    public class PositionComponentAsset : ComponentAsset<PositionComponent>
    {
        public override object Instantiate()
        {
            return Value.Clone();
        }
    }
}