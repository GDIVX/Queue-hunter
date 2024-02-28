using Assets.Scripts.Engine.ECS.Common;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Core.ECS.Common
{
    [CreateAssetMenu(fileName = "RotationComponent", menuName = "ECS/Components/RotationComponent")]
    public class RotationAsset : ComponentAsset<RotationComponent>
    {
        public override object Instantiate()
        {
            return Value.Clone();
        }
    }
}